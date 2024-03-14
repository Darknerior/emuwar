using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private bool isDragging = false;
    private Vector2 dragStartPosition;
    private Vector3 cameraStartPosition;
    private Vector3 cameraOrgPosition;
    private Quaternion cameraOrgRotation;
    private GameObject mapCamera;
    private Camera camera;
    private float cameraDistanceOriginal;
    
    public float smoothSpeed = 5f;
    public float dragSpeed = 5f;
    public float cameraDistance = 25f;
    public Vector3 cameraRotation = new Vector3(80,0,0);

    public void OnEnable()
    {
        mapCamera = GameObject.FindGameObjectWithTag("MapCamera");
        cameraOrgPosition = mapCamera.transform.position;
        cameraOrgRotation = mapCamera.transform.rotation;
        camera = mapCamera.GetComponent<Camera>();
        cameraDistanceOriginal = camera.orthographicSize;
        camera.orthographicSize = cameraDistance;
        mapCamera.transform.rotation = Quaternion.Euler(cameraRotation);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
        if (eventData.button != PointerEventData.InputButton.Left) return;
        isDragging = true;
        dragStartPosition = eventData.position;
        cameraStartPosition = mapCamera.transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;
        var currentPosition = eventData.position;
        var dragDelta = (currentPosition - dragStartPosition) * dragSpeed;
        var newPosition = cameraStartPosition - new Vector3(dragDelta.x,0f, dragDelta.y) * Time.unscaledDeltaTime;
        mapCamera.transform.position = Vector3.Lerp(mapCamera.transform.position, newPosition, smoothSpeed * Time.unscaledDeltaTime);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }
    
    public void OnDisable()
    {
        if(mapCamera == null)return;
        mapCamera.transform.position = cameraOrgPosition;
        mapCamera.transform.rotation = cameraOrgRotation;
        camera.orthographicSize = cameraDistanceOriginal;
        
    }

    
}