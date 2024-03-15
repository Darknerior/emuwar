using UnityEngine;

public class IdleCameraMovement : MonoBehaviour
{
    public float amplitude = 0.5f; // Maximum distance the camera will move from its original position
    public float frequency = 1f; // Speed of the idle movement
    public Vector3 axis = Vector3.up; // Axis along which the camera will move

    private Vector3 initialPosition; // Initial position of the camera
    private Animation introAnimation; // Reference to the Animation component

    void Start()
    {
        initialPosition = transform.position; // Store the initial position of the camera
        introAnimation = GetComponent<Animation>(); // Get the Animation component attached to the same GameObject
    }

    void Update() {
        if (introAnimation.isPlaying)return;
        
        // Calculate the amount of movement based on the sine function
        var idleMovement = Mathf.Sin(Time.time * frequency) * amplitude;

        // Calculate the target position based on the initial position and the axis of movement
        var targetPosition = initialPosition + axis * idleMovement;

        // Update the position of the camera
        transform.position = targetPosition;
    }
}