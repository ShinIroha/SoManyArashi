using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class Controller : MonoBehaviour
{
    public static System.Random rand = new System.Random();
    Collider2D clickZone;
    Text scoreText;
    Text characterCountText;
    Text cheerCountText;
    Text[,] characterLevelText = new Text[Constants.CHARACTER_SERIES_COUNT, 5];
    Text[,] characterCostText = new Text[Constants.CHARACTER_SERIES_COUNT, 5];
    ObjectPool cheerEffectPool;
    ObjectPool[,] charPool = new ObjectPool[Constants.CHARACTER_SERIES_COUNT, 5];
    SaveDataController sav;

    //testing variables
    int cheerScore = 1;

    // Use this for initialization
    void Start()
    {
        //load character level buttons
        LoadCharacterLevelButtons();

        //find UI components
        clickZone = GameObject.Find("Click Zone").GetComponent<Collider2D>();
        scoreText = GameObject.Find("Score").GetComponent<Text>();
        characterCountText = GameObject.Find("Character Count").GetComponent<Text>();
        cheerCountText = GameObject.Find("Cheer Count").GetComponent<Text>();

        //load savedata
        sav = new SaveDataController();
        UIUpdateScore();
        UIUpdateCharacterCount();
        UIUpdateCheerCount();
        UIUpdateCharacterPanel();

        //load object pools
        cheerEffectPool = ObjectPool.GetObjectPool("prefabs/system/Cheer Effect");
        for (int series = 0; series < Constants.CHARACTER_SERIES_COUNT; series++)
        {
            for (int character = 0; character < 5; character++)
            {
                string path = "prefabs/characters/" + CharacterSeriesDatabase.data[series].name_lowercase + "_" + Constants.ALPHABET_NAMES_LOWERCASE[character];
                charPool[series, character] = ObjectPool.GetObjectPool(path, 5, 5);
            }
        }
        StartCoroutine(TryGenerate());
        StartCoroutine(AutoSave());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (clickZone.OverlapPoint(clickPosition))
            {
                Cheer(new Vector3(clickPosition.x, clickPosition.y, clickPosition.z + 0.5f));
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Cheer(new Vector3(0, 0, -9.5f));
        }

    }

    private void OnDestroy()
    {
        sav.Save();
    }

    void LoadCharacterLevelButtons()
    {
        GameObject prefab = Resources.Load<GameObject>("prefabs/system/Character Level Button");
        int[] shift = new int[5] { 1, 4, 2, 0, 3 };   //change color(UI) order into alphabet order
        string seriesName;
        Sprite[] backgroud = new Sprite[5];
        for (int series = 0; series < Constants.CHARACTER_SERIES_COUNT; series++)
        {
            seriesName = CharacterSeriesDatabase.data[series].name;
            for (int character = 0; character < 5; character++)
            {
                //load background image if necessary
                if (backgroud[character] == null)
                {
                    backgroud[character] = Resources.Load<Sprite>("images/system/char_lvl_bg_" + Constants.ALPHABET_NAMES_LOWERCASE[shift[character]]);
                }
                //instantiate object
                GameObject obj = Instantiate<GameObject>(prefab, GameObject.Find("Level Content").transform);
                obj.transform.localPosition = new Vector3(-265 + 110 * character, -25 - 140 * series, 0);
                //modify object name
                obj.name = seriesName + " " + Constants.ALPHABET_NAMES[shift[character]];
                //modify background image
                obj.GetComponent<Image>().sprite = backgroud[character];
                //modify character image
                Transform image = obj.transform.Find("Image");
                image.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/characters/" + seriesName.ToLower() + "_" + Constants.ALPHABET_NAMES_LOWERCASE[shift[character]]);
                //modify button click event
                Button button = obj.GetComponentInChildren<Button>();
                int tc = character; //temp variables
                int ts = series;
                button.onClick.AddListener(()=> { OnCharacterLevelButtonClick(ts,shift[tc]); });
                //modify level text
                Transform levelText = obj.transform.Find("Level").Find("Level Text");
                levelText.name = obj.name + " Level";
                characterLevelText[series, shift[character]] = levelText.GetComponent<Text>();
                //modify cost text
                Transform costText = obj.transform.Find("Cost").Find("Cost Text");
                costText.name = obj.name + " Cost";
                characterCostText[series, shift[character]] = costText.GetComponent<Text>();
            }
        }
    }

    void UIUpdateScore()
    {
        scoreText.text = sav.saveData.score.ToString() + "/" + sav.saveData.totalScore.ToString();
    }

    void UIUpdateCharacterCount()
    {
        characterCountText.text = sav.saveData.characterCountTotal + "(<color=purple>" + sav.saveData.characterCount[1]
        + "</color>+<color=red>" + sav.saveData.characterCount[4]
        + "</color>+<color=orange>" + sav.saveData.characterCount[2]
        + "</color>+<color=green>" + sav.saveData.characterCount[0]
        + "</color>+<color=blue>" + sav.saveData.characterCount[3] + "</color>)";
    }

    void UIUpdateCheerCount()
    {
        cheerCountText.text = sav.saveData.cheerCount.ToString();
    }

    void UIUpdateCharacterPanel()
    {
        for (int series = 0; series < Constants.CHARACTER_SERIES_COUNT; series++)
        {
            for (int character = 0; character < 5; character++)
            {
                UIUpdateCharacterPanel(series, character);
            }
        }
    }

    void UIUpdateCharacterPanel(int series, int character)
    {
        int level = sav.saveData.characterLevel[series][character];
        characterLevelText[series, character].text = level.ToString();
        if (level >= CharacterSeriesDatabase.data[series].maxLevel)
            characterCostText[series, character].text = "max";
        else
            characterCostText[series, character].text = CharacterSeriesDatabase.data[series].cost[level].ToString();
    }

    void Cheer(Vector3 position)
    {
        GameObject obj = cheerEffectPool.New(position);
        obj.SetActive(true);
        gainScore(cheerScore);
        sav.saveData.cheerCount++;
        UIUpdateCheerCount();
        //try to generate character from newer series with 1.5 times probability and stop when succeed
        for (int series = Constants.CHARACTER_SERIES_COUNT - 1; series >= 0; series--)
        {
            int generationCount = 0;
            for (int character = 0; character < 5; character++)
            {
                int level = sav.saveData.characterLevel[series][character];
                if (GenerateCharacter(series, character, CharacterSeriesDatabase.data[series].generationRate[level] * 1.5f))
                    generationCount++;
            }
            if (generationCount > 0)
                break;
        }
    }

    IEnumerator TryGenerate()
    {
        while (true)
        {
            //try to generate each character in each series independently
            for (int series = 0; series < Constants.CHARACTER_SERIES_COUNT; series++)
            {
                for (int character = 0; character < 5; character++)
                {
                    int level = sav.saveData.characterLevel[series][character];
                    GenerateCharacter(series, character, CharacterSeriesDatabase.data[series].generationRate[level]);
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    bool GenerateCharacter(int series, int character, double probability)
    {
        if (rand.NextDouble() < probability)
        {
            int level = sav.saveData.characterLevel[series][character];
            float y = rand.Next(67, 301) * -0.01f;
            GameObject obj = charPool[series, character].New(new Vector3(4, y, y + 3));
            Character component = obj.GetComponent<Character>();
            component.level = level;
            obj.SetActive(true);
            gainScore(CharacterSeriesDatabase.data[series].generationScore[level]);
            sav.saveData.characterCount[character]++;
            sav.saveData.characterCountTotal++;
            UIUpdateCharacterCount();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void gainScore(int score)
    {
        sav.saveData.score += score;
        sav.saveData.totalScore += score;
        UIUpdateScore();
    }

    public void OnCharacterJump(int series, int character)
    {
        gainScore(0);
    }

    public void OnCharacterLevelButtonClick(int series,int character)
    {
        int level = sav.saveData.characterLevel[series][character];
        if (level >= CharacterSeriesDatabase.data[series].maxLevel)
            return;
        int cost = CharacterSeriesDatabase.data[series].cost[level];
        if (cost <= sav.saveData.score)
        {
            sav.saveData.score -= cost;
            UIUpdateScore();
            sav.saveData.characterLevel[series][character]++;
            UIUpdateCharacterPanel();
        }
    }

    IEnumerator AutoSave()
    {
        while (true)
        {
            yield return new WaitForSeconds(Constants.AUTO_SAVE_PERIOD);
            sav.Save();
        }
    }

    void DummyFunction()
    {
        Debug.Log("dummy");
    }
}
