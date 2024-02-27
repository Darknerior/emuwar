using UnityEngine;
using Cinemachine;
using UnityEngine.EventSystems;


[RequireComponent(typeof(CharacterController))]
public class PlayerController : GameEntity {
    //Controller Local variables
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Transform playerTransform;
    private float defaultPlayerSpeed;
    private bool aimed;
    private float xRotation;
    private CinemachineVirtualCamera virtualCamera;
    private Transform playerCamParent;
    private Vector3 moveDirection;
    
    //Serialized Variables
    [SerializeField]private float emuWalkSpeed = 1.12f;//TOP EMU SPEED 31MPH
    [SerializeField]private float maxPlayerSpeed = 13.85f;//TOP EMU SPEED 31MPH
    [SerializeField]private float playerAcceleration = 4.47f;//CHATGPT ESTIMATE
    [SerializeField]private float jumpHeight = 2.1f;//EMU JUMP HEIGHT
    [SerializeField]private float playerRotationSpeed = 0.5f, playerSprintRotationSpeed = 2.0f,gravityValue = -9.81f, maxPlayerHealth = 100f, startingPlayerHealth = 100f;
    [SerializeField]private KeyCode jumpKey = KeyCode.Space, sprintKey = KeyCode.LeftShift, aimkey = KeyCode.Mouse1;
    [SerializeField]private Camera cameraWp;
    [SerializeField]private Camera cameraPlayer;
    [SerializeField]private GameObject vCamera;
    [SerializeField]private float camWpClamp = 30f;
    
    private void Start(){
        //assign vars
        speed = emuWalkSpeed;
        health = startingPlayerHealth;
        controller = GetComponent<CharacterController>();
        playerTransform = transform;
        defaultPlayerSpeed = speed;
        virtualCamera = vCamera.GetComponent<CinemachineVirtualCamera>();
        playerCamParent = cameraPlayer.transform.parent;
        
        //Cursor settings
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    private void Update() {
        Movement();
        Rotate();
    }
    
    
    /// <summary>
    /// Movement for the controller
    /// </summary>
    protected override void Movement() {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)playerVelocity.y = 0f;
        
        // Calculate movement direction based on camera direction
        var cameraForward = cameraPlayer.transform.forward;
        var cameraRight = cameraPlayer.transform.right;
        cameraForward.y = 0f; // Remove vertical component for horizontal movement
        cameraRight.y = 0f; // Remove vertical component for horizontal movement
        cameraForward.Normalize();
        cameraRight.Normalize();
    
        moveDirection = cameraForward * Input.GetAxisRaw("Vertical") + cameraRight * Input.GetAxisRaw("Horizontal");

            
        
        
        //Acceleration
        if(speed < maxPlayerSpeed && Input.GetKey(sprintKey))speed += playerAcceleration * Time.deltaTime;
        
        //Switches the camera view when aiming
        if (Input.GetKeyDown(aimkey)) {
            // Reset the weapon camera to the default rotation
            cameraWp.transform.localRotation = Quaternion.identity;
            
            cameraWp.gameObject.SetActive(true);
            cameraPlayer.gameObject.SetActive(false);
            vCamera.SetActive(false);
            aimed = true;
        }
        else if (Input.GetKeyUp(aimkey)) {
            vCamera.SetActive(true);
            virtualCamera.ForceCameraPosition(cameraPlayer.transform.position, cameraPlayer.transform.rotation);
            cameraPlayer.gameObject.SetActive(true);
            cameraWp.gameObject.SetActive(false);
            aimed = false;
        }
        
        //Reset Player Speed when sprinting is ceased
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(sprintKey)) speed = defaultPlayerSpeed; 

        // Move the player using CharacterController
        controller.Move((speed * moveDirection + playerVelocity) * Time.deltaTime);
        
        // Changes the height position of the player..
        var canJump = speed <= defaultPlayerSpeed;
        if (Input.GetKeyDown(jumpKey) && groundedPlayer && canJump)playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

    }
    
    /// <summary>
    /// Rotation for the controller
    /// </summary>
    private void Rotate() {
        // Get the current rotation of the camera
        Quaternion cameraRotation;
        Quaternion targetRotation;
        
        //Rotates based on horizontal input
        var rotateSpeed = speed > defaultPlayerSpeed ? playerSprintRotationSpeed : playerRotationSpeed;
        // Rotates based on mouse input when aimed
        if (aimed) {
            // Get mouse input for rotation
            var mouseX = Input.GetAxis("Mouse X") * rotateSpeed;
            var mouseY = Input.GetAxis("Mouse Y") * rotateSpeed;
            
            //Apply clamped vertical local rotation to the weapon camera and horizontal rotation to the player.
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -camWpClamp, camWpClamp);
            cameraWp.transform.localRotation = Quaternion.Euler(xRotation,0f , 0f);
            transform.Rotate(Vector3.up * mouseX);
            
            //Set camera parent to this transform so it stays in the same relative position
            cameraPlayer.transform.SetParent(transform);

                
        }
        else {
            xRotation = 0;
            // Calculate the target rotation for the player to face the direction of the camera
            targetRotation =  Quaternion.LookRotation(moveDirection.normalized, Vector3.up);
        
            // Lerp the player's rotation towards the target rotation when moving
            if (moveDirection != Vector3.zero)transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            
            //Revert camera to original parent
            cameraPlayer.transform.SetParent(playerCamParent);

        }
        
       
    }
    
    public void Damage(float damage){
        TakeDamage(damage); // Call base class method
        if (health > maxPlayerHealth) health = maxPlayerHealth;//Ensure health does not exceed max
        else if(health <= 0)Die();//Die 
    }

    protected void Attack(float damage) {
        DealDamage(damage);
    }

}
