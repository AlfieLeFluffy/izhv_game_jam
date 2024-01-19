using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameControler : MonoBehaviour
{
    [Header("Keybindings")]

    public KeyCode menuKey = KeyCode.Escape;
    public KeyCode planeshiftKey = KeyCode.Q;

    [Header("BaseGameObjects")]
    
    public GameObject GameMenu;
    public GameObject Character;
    public GameObject Camera;

    [Header("Planes")]

    public GameObject[] planes;
    public int planeIndex;

    private bool locked;

    // Start is called before the first frame update
    void Start()
    {
        locked = true;
        GameMenu.SetActive(false);    
    }

    // Update is called once per frame
    void Update()
    {

        if(!locked){
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else{
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        PlayerInput();
    }

    private void FixedUpdate(){
        for(int i = 0; i < planes.Length; i++){
            if(i == planeIndex){
                planes[i].SetActive(true);
            }
            else{
                planes[i].SetActive(false);
            }
        }
    }

    private void PlayerInput(){
        if(Input.GetKeyDown(menuKey)){
            ToggleLocked();
            GameMenu.SetActive(!GameMenu.activeInHierarchy);
            Character.GetComponent<CharacterMovement>().ToggleLockMovement();
            Camera.GetComponent<CameraRotation>().ToggleLook();
        } 
        if(Input.GetKeyDown(planeshiftKey)){
            if(planes.Length == planeIndex + 1 ) {
                planeIndex = 0;
            }
            else{
                planeIndex++;
            }
        }  
    }

    public void ToggleLocked(){
        locked = !locked;
    }
}
