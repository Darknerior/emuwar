using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    [DoNotSerialize] public ObjectPooler<GameObject> Pool;
    [SerializeField] private GameObject bullet;

    public void Awake()
    {
        Pool = new();
        Instance = this;
    }

    public void Start()
    {
        Pool.CreateNewPool(ObjectList.BULLET, bullet);
    }


}
