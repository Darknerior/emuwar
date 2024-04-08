using Interfaces;
using UnityEngine;

/// <summary>
/// Vehicle controller using character controller
/// </summary>
public class VehicleController : GameEntity, IInteractable
{
    public CharacterController controller;
    public Transform body;
    public float vehSpeed = 10f;
    public float vehHealth= 200f;
    public float rotationSpeed = 100f;
    public float gravity = -10f;
    public Transform wheel1;
    public Transform wheel2;
    public Transform wheel3;
    public Transform wheel4;
    public Transform seat;
    private GameObject player;
    public bool _enabled;
    [SerializeField]private KeyCode exitKey = KeyCode.LeftControl;

    private void Start()
    {
        speed = vehSpeed;
        health = vehHealth;
        player = GameManager.Instance.player;
        
    }

    private void FixedUpdate()
    {
        if(!_enabled)return;
        HandleMovement();
        HandleRotation();
        UpdateWheels();
    }

    private void Update()
    {
        if (Input.GetKeyDown(exitKey)) ExitVehicle();
    }

    private void ExitVehicle() {
        player.GetComponent<PlayerController>().inVehicle = false;
        player.transform.SetParent(null);
        _enabled = false;
    }
    
    /// <summary>
    /// Rotates the vehicle
    /// </summary>
    private void HandleRotation() {
        if(controller.velocity.magnitude <= 0.1)return;
        
        var rotateInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, rotateInput * rotationSpeed * Time.fixedDeltaTime);
    }
    
    /// <summary>
    /// Allows the vehicle to go forward and backward
    /// </summary>
    private void HandleMovement() {
        var moveInput = Input.GetAxis("Vertical");
        var moveDirection = body.right * (moveInput * speed)+ Vector3.down * gravity;
        controller.Move(moveDirection);
        
    }

    /// <summary>
    /// Spinning wheels
    /// </summary>
    private void UpdateWheels()
    {
        if (Input.GetKey("s"))
        {
            wheel1.Rotate(0, 500 * Time.deltaTime, 0);
            wheel2.Rotate(0, 500 * Time.deltaTime, 0);
            wheel3.Rotate(0, -500 * Time.deltaTime, 0);
            wheel4.Rotate(0, -500 * Time.deltaTime, 0);
        }

        if (Input.GetKey("w"))
        {
            wheel1.Rotate(0, -500 * Time.deltaTime, 0);
            wheel2.Rotate(0, -500 * Time.deltaTime, 0);
            wheel3.Rotate(0, 500 * Time.deltaTime, 0);
            wheel4.Rotate(0, 500 * Time.deltaTime, 0);
        }
    }
    
    /// <summary>
    /// Parent players to vehicle
    /// </summary>
    public void Interact() {
        player.GetComponentInChildren<Animator>().SetBool("isWalking", false);
        player.transform.SetParent(gameObject.transform);
        player.transform.position = seat.position;
        player.transform.rotation = seat.rotation;
        player.GetComponent<PlayerController>().inVehicle = true;
        _enabled = true;
        var keyStr = exitKey.ToString().ToUpper();
        player.GetComponent<RayCastInteractor>().UpdateInteractText("Press <color=yellow><b>"+keyStr+"</b></color> to exit vehicle");

    }

    public string GetText()
    {
        var keyStr = player.GetComponent<RayCastInteractor>().interactKey.ToString().ToUpper();
        return "Press <color=yellow><b>"+keyStr+"</b></color> to enter vehicle";
    }

    protected override void Die()
    {
        ExitVehicle();
        base.Die();
    }
}