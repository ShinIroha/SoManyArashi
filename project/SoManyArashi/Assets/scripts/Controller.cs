using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Controller : MonoBehaviour
{
    public static System.Random rand = new System.Random();
    Collider2D clickZone;
    GameObject scoreText;
    GameObject characterCountText;
    GameObject clickCountText;
    ObjectPool clickEffectPool;
    ObjectPool[] dotCharPool = new ObjectPool[5];
    static long score = 0;
    int characterCountTotal = 0;
    int[] characterCount = new int[5];  //aiba,jun,nino,ohno,sho
    int clickCount = 0;

    int clickScore = 10;
    // Use this for initialization
    void Start()
    {
        clickZone = GameObject.Find("Click Zone").GetComponent<Collider2D>();
        scoreText = GameObject.Find("Score");
        characterCountText = GameObject.Find("Character Count");
        clickCountText = GameObject.Find("Click Count");
        clickEffectPool = ObjectPool.GetObjectPool("prefabs/system/Click Effect");
        dotCharPool[0] = ObjectPool.GetObjectPool("prefabs/characters/dot_aiba", 5, 5);
        dotCharPool[1] = ObjectPool.GetObjectPool("prefabs/characters/dot_jun", 5, 5);
        dotCharPool[2] = ObjectPool.GetObjectPool("prefabs/characters/dot_nino", 5, 5);
        dotCharPool[3] = ObjectPool.GetObjectPool("prefabs/characters/dot_ohno", 5, 5);
        dotCharPool[4] = ObjectPool.GetObjectPool("prefabs/characters/dot_sho", 5, 5);
        StartCoroutine(TryGenerate());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (clickZone.OverlapPoint(clickPosition))
            {
                clickEffectPool.New(new Vector3(clickPosition.x, clickPosition.y, clickPosition.z + 0.5f));
                gainScore(clickScore);
                clickCount++;
                GenerateCharacter(0.1);
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            clickEffectPool.New(new Vector3(0, 0, -9.5f));
            gainScore(clickScore);
            clickCount++;
            GenerateCharacter(0.1);
        }

        scoreText.GetComponent<Text>().text = score.ToString();
        characterCountText.GetComponent<Text>().text = characterCountTotal + "(<color=purple>" + characterCount[1]
            + "</color>+<color=red>" + characterCount[4]
            + "</color>+<color=orange>" + characterCount[2]
            + "</color>+<color=green>" + characterCount[0]
            + "</color>+<color=blue>" + characterCount[3] + "</color>)";
        clickCountText.GetComponent<Text>().text = clickCount.ToString();
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
            characterCount[character]++;
            gainScore(100);
            characterCountTotal++;
        }
    }

    public static void gainScore(int score)
    {
        Controller.score += score;
    }
}
