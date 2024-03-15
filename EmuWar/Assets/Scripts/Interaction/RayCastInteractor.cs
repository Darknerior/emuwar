using UnityEngine;
using Interfaces;
using TMPro;

public class RayCastInteractor : MonoBehaviour
{
    [SerializeField]private float interactRange = 5f;
    [SerializeField]private float sphereRadius = 0.5f;
    public KeyCode interactKey = KeyCode.E;
    public TextMeshProUGUI textMeshProText;
    

    private void Update()
    {
       
        
        RaycastHit hit;
        if (!Physics.SphereCast(transform.position, sphereRadius, transform.forward, out hit, interactRange)){if(!gameObject.GetComponent<PlayerController>().inVehicle) textMeshProText.SetText(""); return;}
        var interactable = hit.collider.GetComponent<IInteractable>();
        if (interactable != null)
        {
            textMeshProText.SetText(interactable.GetText());
            if (Input.GetKeyDown(interactKey))interactable.Interact();
        }
        
        
        
    }
    
    /// <summary>
    /// Update the interact text
    /// </summary>
    /// <param name="newtext"></param>
    public void UpdateInteractText(string newtext)
    {
        textMeshProText.SetText(newtext);
    }
}