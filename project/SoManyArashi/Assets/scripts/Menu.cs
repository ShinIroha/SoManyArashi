using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
    bool isClosed=true;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Open()
    {
        StartCoroutine(Move(1));
        isClosed = !isClosed;
    }

    public void Close()
    {
        StartCoroutine(Move(-1));
        isClosed = !isClosed;
    }

    IEnumerator Move(int direction)
    {
        for(int i = 0; i < 15; i++)
        {
            transform.position += new Vector3(0, 0.2f * direction);
            yield return 0;
        }
    }
}
