using System.Collections;
using UnityEngine;
using Interfaces;

/// <summary>
/// Gives health to player
/// </summary>
[RequireComponent(typeof(Collider))]
public class HealthPickUp : MonoBehaviour {
    [SerializeField] private float healingAmount = 10f; 
    [SerializeField] private float rotationSpeed = 50f; // Speed of rotation
    private Coroutine rotationCoroutine;

    private void Start() {
        rotationCoroutine = StartCoroutine(RotatePickup());
    }
    
    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player"))return;
        var damageable = other.GetComponent<IDamageable>();
        if (damageable == null) return;
        damageable.TakeDamage(-healingAmount);
        Destroy(gameObject);
    }
    
    // Coroutine to rotate
    private IEnumerator RotatePickup() {
        while (true) {
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
            yield return null;
        }
        // ReSharper disable once IteratorNeverReturns
    }

    private void OnDestroy() {
        // Stop the rotation
        if (rotationCoroutine != null) {
            StopCoroutine(rotationCoroutine);
        }
    }
    
}
