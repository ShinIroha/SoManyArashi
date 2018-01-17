using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Character : MonoBehaviour
{
    bool isJumping = false;
    float initialY;
    Rigidbody2D rd;
    Controller controller;
    // Use this for initialization
    void OnEnable()
    {
        initialY = transform.position.y;
        if (rd == null)
            rd = GetComponent<Rigidbody2D>();
        if (controller == null)
        {
            controller = GameObject.Find("Controller").GetComponent<Controller>();
        }
        rd.velocity = new Vector2(Controller.rand.Next(200, 500) * -0.01f, 0);
        StartCoroutine(TryJump());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.x < -4)
        {
            StopAllCoroutines();
            gameObject.SetActive(false);
        }
        if (isJumping)
        {
            if (transform.position.y < initialY)
            {
                rd.velocity = new Vector2(rd.velocity.x, 0);
                transform.position = new Vector3(transform.position.x, initialY, transform.position.z);
                isJumping = false;
            }
            else
            {
                rd.AddForce(new Vector2(0, -50));
            }
        }
    }

    IEnumerator TryJump()
    {
        while (true)
        {
            if (!isJumping && Controller.rand.NextDouble() < 0.03)
                Jump();
            yield return new WaitForSeconds(0.1f);
        }
    }

    void Jump()
    {
        rd.velocity = new Vector2(rd.velocity.x, 10);
        isJumping = true;
        controller.OnCharacterJump();
    }
}
