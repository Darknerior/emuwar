using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Shows a markers on the minimap to indicate the direction of the objective
/// </summary>
public class ObjectiveMarker : MonoBehaviour
{
    private Transform MinimapCam;
    private Transform Player;
    public float SmallMinimapSize;
    private float minimapSize;
    public float BigMapSize;
    Vector3 TempV3;

    private void Start() {
        Player = GameManager.Instance.player.transform;
        foreach (Transform child in Player) {
            if (child.tag == "MapCamera")MinimapCam = child;
        }
    }

    void Update () {
        if(MinimapCam == null)return;
        TempV3 = transform.parent.transform.position;
        TempV3.y = transform.position.y;
        transform.position = TempV3;
        var yrot = Time.timeScale == 0 ? MinimapCam.rotation.y : Player.rotation.y;
        transform.rotation = new Quaternion(Player.rotation.x, yrot, Player.rotation.z, Player.rotation.w);
        minimapSize = Time.timeScale == 0 ? BigMapSize : SmallMinimapSize;

    }
    

    void LateUpdate () {
        if(MinimapCam == null)return;
        transform.position = new Vector3 (
            Mathf.Clamp(transform.position.x, MinimapCam.position.x-minimapSize, minimapSize+MinimapCam.position.x),
            transform.position.y,
            Mathf.Clamp(transform.position.z, MinimapCam.position.z-minimapSize, minimapSize+MinimapCam.position.z)
        );
    }

}
