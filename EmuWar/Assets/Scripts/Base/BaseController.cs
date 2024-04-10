using System.Collections.Generic;
using UnityEngine;

public class BaseController 
{
    List<Base> bases = new List<Base>();
   public BaseController()
    {
        //bases.AddRange(GameObject.FindObjectsByType<Base>(FindObjectsSortMode.None).ToList());

        Debug.Log(bases.Count);
    }

    public void SetNewBase()
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
}
