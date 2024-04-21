using Interfaces;
using UnityEngine;

public class CagedEmu : MonoBehaviour, IInteractable, IPoolable
{
    [SerializeField]private KeyCode exitKey = KeyCode.E;
    private Base owner;
    void ReleaseEmu()
    {
        if (gameObject.GetComponentInChildren<ICagedEmu>().Release())
        {
            if (owner != null)
            {
                owner.ClearCagedEmu();
            }
            ReturnToPool();
        }
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

    public void SetOwner(Base thing) => owner = thing;

    public void ReturnToPool()
    {
        GameManager.Instance.Pool.ResetPool(ObjectList.CAGEDEMU);
        Destroy(gameObject.transform.parent.gameObject);
    }
}
