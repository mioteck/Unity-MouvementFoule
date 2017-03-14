using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour {

    public float speedMove;

    private float right;
    private float left;
    private float up;
    private float down;

    void Start()
    {
        right = Screen.width * 0.9f;
        left = Screen.width * 0.1f;
        up = Screen.height * 0.9f;
        down = Screen.height * 0.1f;
    }

    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        Debug.Log(mousePosition);
        if(mousePosition.y >= up)
        {
            Debug.Log("Up");
            transform.Translate(Vector3.up * speedMove * Time.deltaTime);
        }
        if (mousePosition.y <= down)
        {
            Debug.Log("Down");
            transform.Translate(Vector3.down * speedMove * Time.deltaTime);
        }
        if (mousePosition.x <= left)
        {
            Debug.Log("Left");
            transform.Translate(Vector3.left * speedMove * Time.deltaTime);
        }
        if (mousePosition.x >= right)
        {
            Debug.Log("Right");
            transform.Translate(Vector3.right * speedMove * Time.deltaTime);
        }
    }

}

