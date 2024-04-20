using Interfaces;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
public class ArmyDisplay : MonoBehaviour
{
   private TextMeshProUGUI text;
   private GameObject parent;
   private void Start()
   {
      text = GetComponent<TextMeshProUGUI>();
      parent = gameObject.transform.parent.gameObject;
      GameManager.Instance.Subscribe(ShowText);
   }
   private void ShowText(bool isShowing) => parent.SetActive(!isShowing);
   public void UpdateText(IStatOwner item) => text.text = new string($"{item.ArmySize}/{item.MaxArmySize}");
}