using System.Collections.Generic;
using UnityEngine;
using Interfaces;

public class ObjectPooler
{
    private Dictionary<ObjectList, List<GameObject>> GamePool = new();
    private Dictionary<ObjectList, GameObject> referencePool = new();
    

    /// <summary>
    /// Creates a new ObjectList with specified ObjectList type, and given GameObject. 
    /// Will not create duplicate lists.
    /// Creates a list of one unless size is specified
    /// </summary>
    /// <param name="objectType"> Enum to identify object in pool</param>
    /// <param name="obj"> GameObject to be pooled</param>
    /// <param name="size"> Number of Objects to be pooled (Can Be Resized)</param>
    public void CreateNewPool(ObjectList objectType, GameObject obj, int size = 1)
    {
        if(GamePool.ContainsKey(objectType))
        {
            return;
        }

        if(!obj.TryGetComponent(out IPoolable _) && obj.GetComponentInChildren<IPoolable>() == null)
        {
            Debug.Log("Not Added");
            return;
        }

        referencePool.Add(objectType, obj);
        List<GameObject> list = new List<GameObject>();

        for(int i = 0; i < size; i++)
        {
            GameObject newObject = GameObject.Instantiate(obj);
            newObject.SetActive(false);
            list.Add( newObject );
        }
        GamePool.Add(objectType, list);
    }

    public void ReturnAllToPool()
    {
        for(int i = 0;i < GamePool.Count;i++)
        {
            foreach(GameObject obj in GamePool[(ObjectList)i])
            {
                if(obj.TryGetComponent(out IPoolable poolable))
                {
                    poolable.ReturnToPool();
                }
            }
        }
    }

    /// <summary>
    /// Returns one object from specified list.
    /// Will create a new object if resizable is set to true and none are available.
    /// </summary>
    /// <param name="objectType">Identifier for object list</param>
    /// <param name="resizable">Can more objects be made? Default is false</param>
    /// <returns></returns>
    public GameObject Get(ObjectList objectType, bool resizable = false)
    {
        try
        {
            foreach (GameObject obj in GamePool[objectType])
            {
                if (!obj.activeInHierarchy)
                {
                    return obj;
                }
            }
            if (resizable)
            {
                GameObject extraObj = GameObject.Instantiate(referencePool[objectType]);
                extraObj.SetActive(false);
                GamePool[objectType].Add(extraObj);
                return extraObj;
            }
        }
        catch { }
        return null;
    }

    /// <summary>
    /// Returns Multiple of a specified object.
    /// Will create more objects if there is not enough available and resizable is set to true
    /// Returns null if the amount requested is not reached 
    /// </summary>
    /// <param name="objectType">Identifier for object</param>
    /// <param name="amount">Number of objects to return</param>
    /// <param name="resizable">Can more objects be made? Default is false</param>
    /// <returns></returns>
    public List<GameObject> GetMultiple(ObjectList objectType,int amount, bool resizable = false)
    {
        List<GameObject> result = new List<GameObject>();
        int count = 0;
        foreach(GameObject obj in GamePool[objectType])
        {
            if (!obj.activeInHierarchy) 
            {
                result.Add(obj);
                count++;
            }

            if (count == amount) break;
        }

        if(count < amount && resizable)
        {
            for(int i = count; i <= amount; i++)
            {
                GameObject newObject = Object.Instantiate(referencePool[objectType]);
                newObject.SetActive(false);
                GamePool[objectType].Add(newObject);
                result.Add(newObject);
            }
        }
        return result.Count == amount ? result : null;
    }
    /// <summary>
    /// Clears all objects currently contained in this pool and makes a new one.
    /// </summary>
    /// <param name="item"></param>
    public void ResetPool(ObjectList item)
    {
        GamePool[item] = new List<GameObject>();
        GameObject newObject = Object.Instantiate(referencePool[item]);
        newObject.SetActive(false);
        GamePool[item].Add(newObject);
    }
}

public enum ObjectList
{
    BULLET,
    ENEMY,
    CAGEDEMU
}

