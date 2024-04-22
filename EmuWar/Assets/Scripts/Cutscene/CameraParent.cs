using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraParent : MonoBehaviour
{
    public List<Transform> parents; 
    public float interval = 15f; 
    private float timer; 
    private int currentindex = 0; 

    void Start() {
        timer = interval; 
    }

    void Update()
    {
        timer -= Time.deltaTime; 
        if (timer <= 0 && parents.Count > 0) {
            ChangeParent();
            timer = interval; 
        }
    }

    void ChangeParent() {
        Transform newParent = parents[currentindex];
        if (currentindex > parents.Count-2) currentindex = 0;
        else currentindex++;
        transform.SetParent(newParent, true);
        transform.position = newParent.position;
        transform.rotation = newParent.rotation;
    }
}
