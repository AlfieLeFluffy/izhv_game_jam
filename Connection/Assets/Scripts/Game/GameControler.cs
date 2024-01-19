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
    public Camera Camera;

    [Header("Planes")]

    public GameObject[] planes;
    public int planeIndex;

    [Header("Interactable")]

    public GameObject crosshair;
    public GameObject[] crosshairStates;
    public int crosshairIndex;

    public float detectDistance;

    private bool locked;
    private RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        locked = true;
        GameMenu.SetActive(false);    
    }

    // Update is called once per frame
    void Update()
    {
        DetectInteractibility();
        Lockout();
        PlayerInput();
    }

    private void FixedUpdate(){
        ShiftPlanes();
        ShiftCrosshair();
    }

    private void ShiftPlanes(){
        for(int i = 0; i < planes.Length; i++){
            if(i == planeIndex){
                planes[i].SetActive(true);
            }
            else{
                planes[i].SetActive(false);
            }
        }
    }

    private void ShiftCrosshair(){
        for(int i = 0; i < crosshairStates.Length; i++){
            if(i == crosshairIndex){
                crosshairStates[i].SetActive(true);
            }
            else{
                crosshairStates[i].SetActive(false);
            }
        }
    }

    private void DetectInteractibility(){
        Physics.Raycast( Camera.main.ScreenPointToRay( Input.mousePosition ), out hit, detectDistance );
        if(hit.collider != null){
            if(hit.collider.CompareTag("Interactable")) {
                crosshairIndex = 1;
            }
            else{
                crosshairIndex = 0;
            }
        }
        else{
            crosshairIndex = 0;
        } 
        
    }

    private void Lockout(){
        if(!locked){
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            crosshair.SetActive(false);
        }
        else{
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            crosshair.SetActive(true);
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
