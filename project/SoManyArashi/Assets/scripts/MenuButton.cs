using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEditor;

public class MenuButton : MonoBehaviour
{
    GameObject menu;
    GameObject clickZone;
    Sprite[,] sprites = new Sprite[2, 3];
    SpriteState[] states = new SpriteState[2];
    bool isClosed = true;
    // Use this for initialization
    void Start()
    {
        menu = GameObject.Find("Menu");
        clickZone = GameObject.Find("Click Zone");
        sprites[0, 0] = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/images/system/btn_menu_open.png");
        sprites[0, 1] = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/images/system/btn_menu_open_over.png");
        sprites[0, 2] = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/images/system/btn_menu_open_pressed.png");
        sprites[1, 0] = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/images/system/btn_menu_close.png");
        sprites[1, 1] = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/images/system/btn_menu_close_over.png");
        sprites[1, 2] = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/images/system/btn_menu_close_pressed.png");
        states[0].highlightedSprite = sprites[0, 1];
        states[0].pressedSprite = sprites[0, 2];
        states[1].highlightedSprite = sprites[1, 1];
        states[1].pressedSprite = sprites[1, 2];
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClick()
    {
        if (isClosed)
        {
            GetComponent<Image>().sprite = sprites[1, 0];
            GetComponent<Button>().spriteState = states[1];
            menu.GetComponent<Menu>().Open();
            clickZone.transform.localScale = new Vector3(6, 4.27f, 1);
            clickZone.transform.Translate(new Vector3(0, 1.5f, 0));
        }else
        {
            GetComponent<Image>().sprite = sprites[0, 0];
            GetComponent<Button>().spriteState = states[0];
            menu.GetComponent<Menu>().Close();
            clickZone.transform.localScale = new Vector3(6, 7.27f, 1);
            clickZone.transform.Translate(new Vector3(0, -1.5f, 0));
        }
        isClosed = !isClosed;
    }
}
