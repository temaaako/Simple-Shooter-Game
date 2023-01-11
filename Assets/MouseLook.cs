using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private float _mouseSensativity=300f;
    [SerializeField] private Transform _playerBody;
    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible= false;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensativity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensativity * Time.deltaTime;


        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        _playerBody.Rotate(Vector3.up * mouseX);
    }
}
