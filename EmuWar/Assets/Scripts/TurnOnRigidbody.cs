using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TurnOnRigidbody : MonoBehaviour
{
    private Rigidbody rb;
    public float pushPower = 20f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void Enable()
    {
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.None;
        rb.freezeRotation = false;
    }
}