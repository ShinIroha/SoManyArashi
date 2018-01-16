using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuButton : MonoBehaviour
{
    GameObject menu;
    GameObject clickZone;
    GameObject btnOpen;
    GameObject btnClose;
    static bool isClosed = true;
    // Use this for initialization
    void Start()
    {
        menu = GameObject.Find("Menu");
        clickZone = GameObject.Find("Click Zone");
        btnOpen = GameObject.Find("Menu Button Open");
        btnClose = GameObject.Find("Menu Button Close");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClick()
    {
        if (isClosed)
        {
            btnOpen.transform.localPosition = new Vector3(0, -100);
            btnClose.transform.localPosition = new Vector3(0, 0);
            menu.GetComponent<Menu>().Open();
            clickZone.transform.localScale = new Vector3(6, 4.27f, 1);
            clickZone.transform.Translate(new Vector3(0, 1.5f, 0));
        }
        else
        {
            btnOpen.transform.localPosition = new Vector3(0, 0);
            btnClose.transform.localPosition = new Vector3(0, -100);
            menu.GetComponent<Menu>().Close();
            clickZone.transform.localScale = new Vector3(6, 7.27f, 1);
            clickZone.transform.Translate(new Vector3(0, -1.5f, 0));
        }
        isClosed = !isClosed;
    }
}
