using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class CharacterMovement : MonoBehaviour
{
    [Header("Movement")]
    public float movementSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public bool jumpCheck = true;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground")]
    public float playerHeight;
    public LayerMask groundLayer;
    public bool grounded;


    [Header("Orientation")]
    public Transform orientation;
    float horizontalInput;
    float verticalInput;

    [Header("Audio")]

    public AudioClip[] audioClips;

    Vector3 moveDirection;
    Rigidbody rb;

    bool locked;

    private void Start(){
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        jumpCheck = true;
    }

    private void Update(){
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight*0.5f+0.2f, groundLayer);
        PlayerInput();
        SpeedControl();

        if(grounded){
            rb.drag = groundDrag;
        }
        else{
            rb.drag= 0;
        }
    }

    private void FixedUpdate() {
        if(!locked) CharacterMove();

        if((rb.velocity.x < 1 && rb.velocity.z < 1) || !grounded)   SoundManager.Instance.stopMovSound();
    
    }

    public void ToggleLockMovement(){
        locked = !locked;
    }


    private void PlayerInput(){
        if(!locked){
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");

            if((horizontalInput > 0.1 || verticalInput>0.1) && grounded){
                if(!SoundManager.Instance.isPlayingMov()){
                    SoundManager.Instance.playMovSound(audioClips[1]);
                }
            }

            if(Input.GetKey(jumpKey) && jumpCheck && grounded){

                
                jumpCheck = false;
                Jump();
                Invoke(nameof(ResetJump), jumpCooldown);
            }   
        }
    }

    private void CharacterMove(){
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        
        if(grounded)
            rb.AddForce(moveDirection.normalized * movementSpeed * 10f, ForceMode.Force);

        else if(!grounded)

            SoundManager.Instance.stopMovSound();

            rb.AddForce(moveDirection.normalized * movementSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl(){
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if(flatVel.magnitude>movementSpeed){
            Vector3 limited = flatVel.normalized * movementSpeed;
            rb.velocity = new Vector3(limited.x, rb.velocity.y, limited.z);
        }
    }

    [YarnCommand("leap")]
    public void Jump()
    {
        SoundManager.Instance.playCharSound(audioClips[0]);
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        jumpCheck = true;
    }

}
