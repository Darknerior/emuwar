using Interfaces;
using UnityEngine;

public class CagedEmu : MonoBehaviour, IInteractable
{
    [SerializeField]private KeyCode exitKey = KeyCode.E;
    void ReleaseEmu()
    {
        gameObject.GetComponentInChildren<EmuNPC>().Release();
        Destroy(gameObject.transform.parent.gameObject);
    }

    public void Interact()
    {
        ReleaseEmu();
    }

    public string GetText()
    {
        var keyStr = exitKey.ToString().ToUpper();
        return "Press <color=yellow><b>"+keyStr+"</b></color> to release emu";
    }
}
