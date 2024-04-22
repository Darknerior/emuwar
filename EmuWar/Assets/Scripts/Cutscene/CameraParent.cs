using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Changes the camera parent every x seconds
/// </summary>
public class CameraParent : MonoBehaviour {
    [SerializeField]private List<Transform> parents; 
    [SerializeField]private float interval = 15f; 
    private float timer; 
    private int currentindex;

    private void Start() {
        timer = interval; 
    }

    private void Update() {
        timer -= Time.deltaTime;
        if (!(timer <= 0) || parents.Count <= 0) return;
        ChangeParent();
        timer = interval;
    }

    private void ChangeParent() {
        var newParent = parents[currentindex];
        if (currentindex > parents.Count-2) currentindex = 0;
        else currentindex++;
        Transform transform1;
        (transform1 = transform).SetParent(newParent, true);
        transform1.position = newParent.position;
        transform1.rotation = newParent.rotation;
    }
}
