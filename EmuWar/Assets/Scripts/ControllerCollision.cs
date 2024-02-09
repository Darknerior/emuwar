using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ControllerCollision : MonoBehaviour
{
    private float pushPower;
    private float playerSpeed;
    private PlayerMovement playerMovement;
    [SerializeReference] private float pushPowerMultiplier = 1.5f;
    [SerializeReference] private float  minimumPlayerSpeed = 1.5f;

    private void Start()
    {
        playerMovement = this.GameObject().GetComponent<PlayerMovement>();
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        var body = hit.collider.attachedRigidbody;
        playerSpeed = playerMovement.playerSpeed;
        
        //Return if no rigid body or if the minimum player speed is not met
        if (body == null || body.isKinematic || playerSpeed < minimumPlayerSpeed)
            return;

        // We dont want to push objects below us
        if (hit.moveDirection.y < -0.3f)
            return;

        var turonrb = hit.transform.GetComponent<TurnOnRigidbody>();
        if(turonrb == null)return;
        turonrb.Enable(); 
        pushPower = playerSpeed * pushPowerMultiplier;
      


        // Calculate push direction from move direction,
        // we only push objects to the sides never up and down
        var pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // If you know how fast your character is trying to move,
        // then you can also multiply the push velocity by that.

        // Apply the push
        body.velocity = pushDir * pushPower;
    }
}
