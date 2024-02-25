using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Serialization;
using Cinemachine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour {
    //Controller Local variables
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Transform playerTransform;
    private float defaultPlayerSpeed;
    private bool aimed;
    
    //Serialized Variables
    [SerializeField]public float playerSpeed =  1.12f;//EMU WALK SPEED 2.5MPH
    [SerializeField]private float maxPlayerSpeed = 13.85f;//TOP EMU SPEED 31MPH
    [SerializeField]private float playerAcceleration = 4.47f;//CHATGPT ESTIMATE
    [SerializeField]private float playerRotationSpeed = 0.5f;
    [SerializeField]private float playerSprintRotationSpeed = 2.0f;
    [SerializeField]private float jumpHeight = 2.1f;//EMU JUMP HEIGHT
    [SerializeField]private float gravityValue = -9.81f;
    [SerializeField]private KeyCode jumpKey = KeyCode.Space, sprintKey = KeyCode.LeftShift;
    [SerializeField]private GameObject camera;
    [SerializeField]private Camera cameraWP;
    [SerializeField]private Camera cameraPlayer;

    
    
    private void Start() {
        controller = GetComponent<CharacterController>();
        playerTransform = transform;
        defaultPlayerSpeed = playerSpeed;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    private void Update() {
        Move();
        Rotate();
    }
    
    
    /// <summary>
    /// Movement for the controller
    /// </summary>
    private void Move() {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)playerVelocity.y = 0f;
        

        // Calculate movement direction based on input
        if (Input.GetKeyDown(KeyCode.Mouse1)) {
            //CMcamera.m_RecenterToTargetHeading.m_enabled = true;
            cameraPlayer.gameObject.SetActive(false);
            cameraWP.gameObject.SetActive(true);
            aimed = true;

        }
        var moveDirection = Vector3.zero;
        if (Input.GetAxisRaw("Vertical") > 0) {
            //CMcamera.m_RecenterToTargetHeading.m_enabled = true;
            moveDirection = playerTransform.forward;
            if(playerSpeed < maxPlayerSpeed && Input.GetKey(sprintKey))playerSpeed += playerAcceleration * Time.deltaTime;//Accelerate
        }
        else if (Input.GetAxisRaw("Vertical") < 0) {
            moveDirection = -playerTransform.forward;
            
        }

        if (!Input.GetKey(KeyCode.Mouse1) && Input.GetAxisRaw("Vertical") <= 0)
        {
            //CMcamera.m_RecenterToTargetHeading.m_enabled = false;
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            cameraPlayer.gameObject.SetActive(true);
            cameraWP.gameObject.SetActive(false);
            aimed = false;
        }
        
        //Reset Player Speed    
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(sprintKey)) playerSpeed = defaultPlayerSpeed; 



        // Move the player using CharacterController
        controller.Move((playerSpeed * moveDirection + playerVelocity) * Time.deltaTime);
        
        // Changes the height position of the player..
        var canJump = playerSpeed <= defaultPlayerSpeed;
        if (Input.GetKeyDown(jumpKey) && groundedPlayer && canJump)playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

    }
    
    /// <summary>
    /// Rotation for the controller
    /// </summary>
    private void Rotate() {
        //Rotates based on horizontal input
        var rotateSpeed = playerSpeed > defaultPlayerSpeed ? playerSprintRotationSpeed : playerRotationSpeed;

        if (!aimed)
        {
            transform.Rotate(new Vector3(0,Input.GetAxisRaw("Horizontal") * rotateSpeed, 0));
        }
        else
        {
            transform.Rotate(new Vector3(0,Input.GetAxis("Mouse X") * rotateSpeed, 0));
        }
        
    }

}
