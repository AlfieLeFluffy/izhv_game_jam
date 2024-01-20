using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    
    public GameObject gameMenu;
    public GameObject character;
    public Camera mainCamera;
    public GameObject overworldLight;

    [Header("Planes")]

    public GameObject[] planes;
    public TMP_Text[] linesUI;
    public string[] displayTimes;
    public float[] angles;
    public float[] intensities;
    public Color[] ambientColors;
    public int planeIndex;

    [Header("Planes Shifting")]
    public GameObject planeshiftEffect;
    public GameObject cooldownDot;
    public float cooldownTime = 2f;
    private float cooldownTimer = 0f;
    private bool offCooldown = true;

    [Header("Interactable")]

    public GameObject gameUI;
    public GameObject[] crosshairStates;
    public int crosshairIndex;
    public float detectDistance;

    [Header("Audio")]
    
    public AudioClip[] audioClips;

    private bool locked;
    private RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        locked = true;
        gameMenu.SetActive(false);    
        gameUI.SetActive(true);
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

        if(planeshiftEffect.GetComponent<Image>().color.a>0){
            planeshiftEffect.SetActive(true);
            var tempColour = planeshiftEffect.GetComponent<Image>().color;
            tempColour.a = tempColour.a - 0.1f;
            planeshiftEffect.GetComponent<Image>().color = tempColour;
        }
        else{
            planeshiftEffect.SetActive(false);
        }

        if(cooldownTimer>0){
            cooldownTimer -= Time.deltaTime;
            Vector3 angles = cooldownDot.GetComponent<RectTransform>().eulerAngles; 
            angles.z = cooldownTimer/cooldownTime*360;
            cooldownDot.GetComponent<RectTransform>().eulerAngles = angles;
        }
        else{
            offCooldown = true;
            cooldownDot.SetActive(false);
        }

    }

    private void ShiftPlanes(){
        for(int i = 0; i < planes.Length; i++){
            if(i == planeIndex){
                planes[i].SetActive(true);
                overworldLight.transform.eulerAngles = new Vector3(angles[i], overworldLight.transform.eulerAngles.y, overworldLight.transform.eulerAngles.z);
                linesUI[0].text = "//DIMENSION: "+planes[i].name;
                linesUI[1].text = "///TIME: "+displayTimes[i];
                overworldLight.GetComponent<Light>().intensity = intensities[i];
                overworldLight.GetComponent<Light>().color = ambientColors[i];
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
        Physics.Raycast( mainCamera.ScreenPointToRay( Input.mousePosition ), out hit, detectDistance );
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
            //gameUI.SetActive(false);
        }
        else{
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            //gameUI.SetActive(true);
        }
    }

    private void PlayerInput(){
        if(Input.GetKeyDown(menuKey) && allowedControls){
            ToggleLocked();
            gameMenu.SetActive(!gameMenu.activeInHierarchy);
            character.GetComponent<CharacterMovement>().ToggleLockMovement();
            mainCamera.GetComponent<CameraRotation>().ToggleLook();
        } 
        if(Input.GetKeyDown(planeshiftKey) && allowedControls && offCooldown){
            SoundManager.Instance.playEffSound(audioClips[0]);
            var tempColour = planeshiftEffect.GetComponent<Image>().color;
            tempColour.a = 1f;;

            offCooldown = false;
            cooldownTimer = cooldownTime;
            cooldownDot.SetActive(true);

            planeshiftEffect.GetComponent<Image>().color = tempColour;
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
