using UnityEngine;

public class Fly : MonoBehaviour {
    [SerializeField]private float speed = 5.0f;  
    [SerializeField]private Vector3 direction = Vector3.forward; 

    void Update() {
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
    }
}
