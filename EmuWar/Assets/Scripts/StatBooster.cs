using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

public class StatBooster : MonoBehaviour ,IUpHealth,IUpArmySize
{
    private int health;
    private int maxArmySize;
    private int healthIncrease;
    private int armySizeIncrease;
    private IStatOwner owner;
   [SerializeField] private ArmyDisplay display;
    public void SetUp(IStatOwner thisOwner)
    {
        owner = thisOwner;
        health = owner.Health;
        maxArmySize = owner.MaxArmySize;
        healthIncrease = owner.MaxHealthIncrease;
        armySizeIncrease = owner.ArmySizeIncrease;
        GameManager.Instance.Base.SetStats(this);
        UpdateDisplay();
    }


    public void UpHealth()
    {
        health += healthIncrease;
        owner.UpdateHealth(health);
    }

    public void UpArmy()
    {
        maxArmySize += armySizeIncrease;
        owner.UpdateArmy(maxArmySize);
        UpdateDisplay();
    }
    public void UpdateDisplay() => display.UpdateText(owner);
}
