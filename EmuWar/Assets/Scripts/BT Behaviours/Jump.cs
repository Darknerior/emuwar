using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerStay (Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("ground"))
        {
            rb.velocity += new Vector3(0, 2, 0);
        }
    }
}
