using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Controlls the screen and window.
/// </summary>
public class ScreenController : MonoBehaviour
{
    int lastWidth;
    int lastHeight;
    bool isReseting = false;
    GameObject UIText;
    // Use this for initialization
    void Start()
    {
        UIText = GameObject.Find("DebugText");
        Screen.SetResolution(600, 960, false);
        lastWidth = Screen.width;
        lastHeight = Screen.height;
    }

    // Update is called once per frame
    void Update()
    {
        UIText.GetComponent<Text>().text = "lastWidth:" + lastWidth + " width:" + Screen.width + "\nlastHeight:" + lastHeight + " height:" + Screen.height;
        if (!isReseting && Mathf.Abs(Camera.main.aspect - Constants.ASPECT_RATIO) > 0.01f)
        {
            isReseting = true;
            StartCoroutine(ResetAspect());
        }
    }

    IEnumerator ResetAspect()
    {
        yield return new WaitForSeconds(0.5f);
        if (Screen.width != lastWidth)
        {
            UIText.GetComponent<Text>().text += " width changed";
            Screen.SetResolution(Screen.width, (int)(Screen.width / Constants.ASPECT_RATIO), false);
        }
        else
        {
            UIText.GetComponent<Text>().text += " height changed";
            Screen.SetResolution((int)(Screen.height * Constants.ASPECT_RATIO), Screen.height, false);
        }
        lastWidth = Screen.width;
        lastHeight = Screen.height;
        isReseting = false;
    }
}
