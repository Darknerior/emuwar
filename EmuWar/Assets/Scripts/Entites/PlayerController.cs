using UnityEngine;


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
        

        
        
        
        // Calculate movement direction based on input
        var moveDirection = Vector3.zero;
        if (Input.GetAxisRaw("Vertical") > 0) {
            moveDirection = playerTransform.forward;
            if(speed < maxPlayerSpeed && Input.GetKey(sprintKey))speed += playerAcceleration * Time.deltaTime;//Accelerate
        }
        else if (Input.GetAxisRaw("Vertical") < 0) {
            moveDirection = -playerTransform.forward;
        }
        
        //Switches the camera view when aiming
        if (Input.GetKeyDown(KeyCode.Mouse1)) {
            cameraPlayer.gameObject.SetActive(false);
            cameraWp.gameObject.SetActive(true);
            vCamera.SetActive(false);
            aimed = true;

            // Reset the weapon camera to the default rotation
            cameraWp.transform.localRotation = Quaternion.identity;
        }
        else if (Input.GetKeyUp(aimkey)) {
            cameraPlayer.gameObject.SetActive(true);
            cameraWp.gameObject.SetActive(false);
            vCamera.SetActive(true);
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
        //Rotates based on horizontal input
        var rotateSpeed = speed > defaultPlayerSpeed ? playerSprintRotationSpeed : playerRotationSpeed;
        // Rotates based on mouse input when aimed
        if (aimed) {
            // Get mouse input for rotation
            var mouseX = Input.GetAxis("Mouse X") * rotateSpeed;
            var mouseY = Input.GetAxis("Mouse Y") * rotateSpeed;
            

            // Rotate the player model horizontally based on mouse X input
            transform.Rotate(Vector3.up * mouseX);
            
            //Apply clamped vertical local rotation to the weapon camera
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -camWpClamp, camWpClamp);
            cameraWp.transform.localRotation = Quaternion.Euler(xRotation,0f , 0f);
                
        }
        else {
            xRotation = 0;
            // Rotates based on horizontal input when not aimed
            transform.Rotate(new Vector3(0, Input.GetAxisRaw("Horizontal") * rotateSpeed, 0));
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
