using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour {
    public float moveSpeed = 0.005f;
    public float altitude = 0;
    public float speedH = 3.0f;
    public float speedV = 3.0f;
    private float yaw = 0.0f;
    private float pitch = 30.0f;

    void Start()
    {
        altitude = gameObject.transform.position.y;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            yaw += speedH * Input.GetAxis("Mouse X");
            pitch -= speedV * Input.GetAxis("Mouse Y");
            transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
        }
        if (Input.GetKey(KeyCode.Z))
        {
            gameObject.transform.Translate(moveSpeed * Vector3.forward, Space.Self);
        }
        if (Input.GetKey(KeyCode.S))
        {
            gameObject.transform.Translate(moveSpeed * Vector3.back, Space.Self);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            gameObject.transform.Translate(moveSpeed * Vector3.left, Space.Self);
        }
        if (Input.GetKey(KeyCode.D))
        {
            gameObject.transform.Translate(moveSpeed * Vector3.right, Space.Self);
        }
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, altitude, gameObject.transform.position.z);
    }

}

