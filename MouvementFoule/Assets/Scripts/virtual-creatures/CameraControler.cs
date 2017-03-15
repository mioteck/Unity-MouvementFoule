using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    [Header("Camera 3rd")]
    public float speedMove;
    private float right;
    private float left;
    private float up;
    private float down;
    [Header("FPS Camera")]
    public float moveSpeed = 0.005f;
    public float altitude = 0;
    public float speedH = 3.0f;
    public float speedV = 3.0f;
    private float yaw = 0.0f;
    private float pitch = 30.0f;

    private bool isChangeCamera = false;
    private Vector3 positionCamera;
    private Vector3 rotationCamera;

    void Start()
    {
        right = Screen.width * 0.9f;
        left = Screen.width * 0.1f;
        up = Screen.height * 0.9f;
        down = Screen.height * 0.1f;
        positionCamera = transform.position;
        rotationCamera = transform.localEulerAngles;

        altitude = gameObject.transform.position.y;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isChangeCamera = !isChangeCamera;
            if (isChangeCamera)
            {
                //transform.position = new Vector3( positionCamera.x, transform.position.y, positionCamera.z);
                transform.localEulerAngles = rotationCamera;
            }
        }
        if (isChangeCamera)
        {
            Vector3 mousePosition = Input.mousePosition;
            if (mousePosition.y >= up)
            {
                Vector3 move = Vector3.up * speedMove * Time.deltaTime + Vector3.forward * speedMove * Time.deltaTime;
                transform.Translate(move);
            }
            if (mousePosition.y <= down)
            {
                Vector3 move = Vector3.down * speedMove * Time.deltaTime + Vector3.back * speedMove * Time.deltaTime;
                transform.Translate(move);
            }
            if (mousePosition.x <= left)
            {
                transform.Translate(Vector3.left * speedMove * Time.deltaTime);
            }
            if (mousePosition.x >= right)
            {
                transform.Translate(Vector3.right * speedMove * Time.deltaTime);
            }
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                yaw += speedH * Input.GetAxis("Mouse X");
                pitch -= speedV * Input.GetAxis("Mouse Y");
                transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
            }
            if (Input.GetKey(KeyCode.Z))
            {
                gameObject.transform.Translate(moveSpeed * Vector3.forward * Time.deltaTime, Space.Self);
            }
            if (Input.GetKey(KeyCode.S))
            {
                gameObject.transform.Translate(moveSpeed * Vector3.back * Time.deltaTime, Space.Self);
            }
            if (Input.GetKey(KeyCode.Q))
            {
                gameObject.transform.Translate(moveSpeed * Vector3.left * Time.deltaTime, Space.Self);
            }
            if (Input.GetKey(KeyCode.D))
            {
                gameObject.transform.Translate(moveSpeed * Vector3.right * Time.deltaTime, Space.Self);
            }
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, altitude, gameObject.transform.position.z);
        }
    }

}

