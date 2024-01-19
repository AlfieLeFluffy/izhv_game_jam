using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameControler : MonoBehaviour
{
    [Header("Keybindings")]

    public KeyCode menuKey = KeyCode.Escape;
    public KeyCode planeshiftKey = KeyCode.Q;

    public bool allowedControls = true;

    [Header("BaseGameObjects")]
    
    public GameObject GameMenu;
    public GameObject Character;
    public Camera Camera;
    public GameObject DirectionalLighting;

    [Header("Planes")]

    public GameObject[] planes;
    public float[] angles;
    public float[] intensities;
    public Color[] ambientColors;
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
                DirectionalLighting.transform.eulerAngles = new Vector3(angles[i], DirectionalLighting.transform.eulerAngles.y, DirectionalLighting.transform.eulerAngles.z);
                DirectionalLighting.GetComponent<Light>().intensity = intensities[i];
                DirectionalLighting.GetComponent<Light>().color = ambientColors[i];
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
        if(Input.GetKeyDown(menuKey) && allowedControls){
            ToggleLocked();
            GameMenu.SetActive(!GameMenu.activeInHierarchy);
            Character.GetComponent<CharacterMovement>().ToggleLockMovement();
            Camera.GetComponent<CameraRotation>().ToggleLook();
        } 
        if(Input.GetKeyDown(planeshiftKey) && allowedControls){
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
    
    public void ToggleAllowedControls(){
        allowedControls = !allowedControls;
    }
}
