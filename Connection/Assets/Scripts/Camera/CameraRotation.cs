using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;

    public GameObject crosshair;

    float xRotation;
    float yRotation;

    bool locked;

    public void Start(){
        locked = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Update(){
        
        
        if(locked){
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime *sensX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime *sensY;

            yRotation += mouseX;
            xRotation -= mouseY;
            xRotation = Math.Clamp(xRotation, -90f, 90f);

            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0 , yRotation, 0 );
        }
    }

    public void ToggleLook(){
        locked = !locked;
    }
}
