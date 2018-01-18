using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class Controller : MonoBehaviour
{
    public static System.Random rand = new System.Random();
    Collider2D clickZone;
    GameObject scoreText;
    GameObject characterCountText;
    GameObject cheerCountText;
    ObjectPool cheerEffectPool;
    ObjectPool[] dotCharPool = new ObjectPool[5];
    SaveDataController sav;

    //testing variables
    int cheerScore = 10;
    int jumpScore = 50;
    
    // Use this for initialization
    void Start()
    {
        sav = new SaveDataController();
        clickZone = GameObject.Find("Click Zone").GetComponent<Collider2D>();
        scoreText = GameObject.Find("Score");
        characterCountText = GameObject.Find("Character Count");
        cheerCountText = GameObject.Find("Cheer Count");
        UIUpdateScore();
        UIUpdateCharacterCount();
        UIUpdateCheerCount();
        //load object pools
        cheerEffectPool = ObjectPool.GetObjectPool("prefabs/system/Cheer Effect");
        dotCharPool[0] = ObjectPool.GetObjectPool("prefabs/characters/dot_aiba", 5, 5);
        dotCharPool[1] = ObjectPool.GetObjectPool("prefabs/characters/dot_jun", 5, 5);
        dotCharPool[2] = ObjectPool.GetObjectPool("prefabs/characters/dot_nino", 5, 5);
        dotCharPool[3] = ObjectPool.GetObjectPool("prefabs/characters/dot_ohno", 5, 5);
        dotCharPool[4] = ObjectPool.GetObjectPool("prefabs/characters/dot_sho", 5, 5);

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

    void UIUpdateScore()
    {
        scoreText.GetComponent<Text>().text = sav.saveData.score.ToString();
    }

    void UIUpdateCharacterCount()
    {
        characterCountText.GetComponent<Text>().text = sav.saveData.characterCountTotal + "(<color=purple>" + sav.saveData.characterCount[1]
        + "</color>+<color=red>" + sav.saveData.characterCount[4]
        + "</color>+<color=orange>" + sav.saveData.characterCount[2]
        + "</color>+<color=green>" + sav.saveData.characterCount[0]
        + "</color>+<color=blue>" + sav.saveData.characterCount[3] + "</color>)";
    }

    void UIUpdateCheerCount()
    {
        cheerCountText.GetComponent<Text>().text = sav.saveData.cheerCount.ToString();
    }

    void Cheer(Vector3 position)
    {
        cheerEffectPool.New(position);
        gainScore(cheerScore);
        sav.saveData.cheerCount++;
        UIUpdateCheerCount();
        GenerateCharacter(0.1);
    }

    IEnumerator TryGenerate()
    {
        while (true)
        {
            GenerateCharacter(0.07);
            yield return new WaitForSeconds(0.1f);
        }
    }

    void GenerateCharacter(double probability)
    {
        if (rand.NextDouble() < probability)
        {
            float y = rand.Next(67, 301) * -0.01f;
            int character = rand.Next(5);
            dotCharPool[character].New(new Vector3(4, y, y + 3));
            gainScore(100);
            sav.saveData.characterCount[character]++;
            sav.saveData.characterCountTotal++;
            UIUpdateCharacterCount();
        }
    }

    public void gainScore(int score)
    {
        sav.saveData.score += score;
        UIUpdateScore();
    }

    public void OnCharacterJump()
    {
        gainScore(jumpScore);
    }

    IEnumerator AutoSave()
    {
        while (true)
        {
            yield return new WaitForSeconds(Constants.AUTO_SAVE_PERIOD);
            sav.Save();
        }
    }
}
