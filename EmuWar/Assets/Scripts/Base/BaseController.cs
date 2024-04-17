using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseController
{
    List<Base> bases = new List<Base>();
    private float waitTime;
   public BaseController(float timeBetweenBaseSpawn)
   {
        bases = GameObject.FindObjectsOfType(typeof(Base), true).Cast<Base>().ToList();
        foreach (var @base in bases)
        {
            @base.SetController(this);
        }

        waitTime = timeBetweenBaseSpawn;
        StartCountdown();
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
    }

    public void StartCountdown()
    {
        GameManager.Instance.BeginRoutine(NextBase());
    }

    private IEnumerator NextBase()
    {
        yield return new WaitForSeconds(waitTime);
        
        SetNewBase();
    }
}
