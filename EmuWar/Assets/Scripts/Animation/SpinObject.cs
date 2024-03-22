using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinObject : MonoBehaviour
{
    public Vector3 spinAxis = Vector3.up; 
    public float spinSpeed = 50f;

    void Update()
    {
        transform.Rotate(spinAxis, spinSpeed * Time.deltaTime);
    }
}
