using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseController
{
    List<Base> bases = new List<Base>();
    private float waitTime;
    private StatBooster stats;
    public Base ActiveBase { get; private set; }
   public BaseController(float timeBetweenBaseSpawn)
   {
        bases = GameObject.FindObjectsOfType(typeof(Base), true).Cast<Base>().ToList();
        foreach (var @base in bases)
        {
            @base.SetController(this);
        }
        waitTime = timeBetweenBaseSpawn;
        StartCountdown(BaseType.NONE);
   }

    private void SetNewBase()
    {
        List<Base> baseCopy = bases;
        Base obj;

        do
        {
            if (baseCopy.Count == 0) return;

            obj = baseCopy[Random.Range(0, baseCopy.Count)];
            baseCopy.Remove(obj);
        }
        while (obj.gameObject.activeInHierarchy);
       
        obj.gameObject.SetActive(true);
        ActiveBase = obj;
    }

    public void StartCountdown(BaseType type)
    {
        GameManager.Instance.BeginRoutine(NextBase());
        switch (type)
        {
            case BaseType.Army:
                stats.UpArmy();
                break;
            case BaseType.Farm:
                stats.UpHealth();
                break;
            default: return;
        }
    }

    private IEnumerator NextBase()
    {
        yield return new WaitForSeconds(waitTime);
        
        SetNewBase();
    }

    public void SetStats(StatBooster booster)
    {
        stats = booster;
    }
}

public enum BaseType
{
    NONE = -1,
    Farm,
    Army
}
