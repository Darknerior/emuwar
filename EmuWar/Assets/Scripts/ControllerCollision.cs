using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// This script allows the player controller to push over objects that have the turnonrb script
/// </summary>
public class ControllerCollision : MonoBehaviour
{
    private float pushPower;
    private float playerSpeed;
    private PlayerController playerMovement;
    [SerializeReference] private float pushPowerMultiplier = 1.5f;
    [SerializeReference] private float  minimumPlayerSpeed = 1.5f;

    private void Start() {
        playerMovement = this.GameObject().GetComponent<PlayerController>();
    }

    void OnControllerColliderHit(ControllerColliderHit hit) {
        var body = hit.collider.attachedRigidbody;
        playerSpeed = playerMovement.speed;
        
        //Return if no rigid body or if the minimum player speed is not met
        if (body == null || body.isKinematic || playerSpeed < minimumPlayerSpeed)
            return;

        //We dont want to push objects below us
        if (hit.moveDirection.y < -0.3f)
            return;
        
        //Turn on the rigidbody of the objects the controller is colliding with
        var turonrb = hit.transform.GetComponent<TurnOnRigidbody>();
        if(turonrb == null)return;
        turonrb.Enable(); 
        
        //Use the player speed to calculate a push power
        pushPower = playerSpeed * pushPowerMultiplier;
        
        // Calculate push direction from move direction,
        // we only push objects to the sides never up and down
        var pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // Apply the push
        body.velocity = pushDir * pushPower;
    }
}
