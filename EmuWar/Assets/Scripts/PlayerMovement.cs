using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Transform playerTransform;
    [SerializeField]float playerSpeed = 2.0f;
    [SerializeField]float topPlayerSpeed = 13.85184f;//TOP EMU SPEED 31MPH
    [SerializeField]float playerRotationSpeed = 2.0f;
    [SerializeField]private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;

    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    
    private void Start() {
        controller = GetComponent<CharacterController>();
        playerTransform = transform;
    }
    
    private void Update() {
        Move();
        Rotate();
    }

    private void Jump()
    {
        throw new System.NotImplementedException();
    }

    private void Move()
    {
        // Calculate movement direction based on input
        var moveDirection = Vector3.zero;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            moveDirection = playerTransform.forward;
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            moveDirection = -playerTransform.forward;

        // Move the player using CharacterController
        controller.Move(moveDirection * playerSpeed * Time.deltaTime);
        
        //ADD ACCELETATION

    }

    private void Rotate()
    {
        transform.Rotate(new Vector3(0,Input.GetAxisRaw("Horizontal") * playerRotationSpeed, 0));
    }

}
