using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuButton : MonoBehaviour
{
    GameObject menu;
    GameObject clickZone;
    Button button;
    Sprite spriteOpen;
    Sprite spriteClose;
    SpriteState ssOpen = new SpriteState();
    SpriteState ssClose = new SpriteState();
    static bool isClosed = true;
    // Use this for initialization
    void Start()
    {
        menu = GameObject.Find("Menu");
        clickZone = GameObject.Find("Click Zone");
        button = GetComponent<Button>();
        spriteOpen = Resources.Load<Sprite>("images/btn_menu_open");
        spriteClose = Resources.Load<Sprite>("images/btn_menu_close");
        ssOpen.highlightedSprite = Resources.Load<Sprite>("images/btn_menu_open_over");
        ssOpen.pressedSprite = Resources.Load<Sprite>("images/btn_menu_open_pressed");
        ssClose.highlightedSprite = Resources.Load<Sprite>("images/btn_menu_close_over");
        ssClose.pressedSprite = Resources.Load<Sprite>("images/btn_menu_close_pressed");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClick()
    {
        if (isClosed)
        {
            button.image.sprite = spriteClose;
            button.spriteState = ssClose;
            menu.GetComponent<Menu>().Open();
            clickZone.transform.localScale = new Vector3(6, 4.27f, 1);
            clickZone.transform.Translate(new Vector3(0, 1.5f, 0));
        }
        else
        {
            button.image.sprite = spriteOpen;
            button.spriteState = ssOpen;
            menu.GetComponent<Menu>().Close();
            clickZone.transform.localScale = new Vector3(6, 7.27f, 1);
            clickZone.transform.Translate(new Vector3(0, -1.5f, 0));
        }
        isClosed = !isClosed;
    }
}
