using Interfaces;
using UnityEngine;

public class GetToTheChoppa : MonoBehaviour, IInteractable
{
    [SerializeField]private KeyCode exitKey = KeyCode.E;

    void Escape(){
        GameManager.Instance.TryLoadExitScene();
    }

    public void Interact() {
        Escape();
    }

    public string GetText()
    {
        var keyStr = exitKey.ToString().ToUpper();
        if(GameManager.Instance.defeatedEnemies >= GameManager.Instance.enemiesToDefeat)return "Press <color=yellow><b>"+keyStr+"</b></color> to extract";
        
        var intstring = (GameManager.Instance.enemiesToDefeat - GameManager.Instance.defeatedEnemies).ToString();
        return "You must defeat <color=yellow><b>" + intstring + "</b></color> enemies to extract";
       
    }

}