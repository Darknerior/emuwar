using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Interfaces;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

namespace BehaviourTree
{
    /// <summary>
    /// Base Class for Behaviour tree. This is where the root node for the tree begins.
    /// </summary>
    public abstract class Tree : GameEntity,IPoolable,ICagedEmu
    {
        private Node root = null;
        private Dictionary<string, object> sharedData = new();
        private bool isCaged;
        private IStatOwner statOwner;

        protected void Awake()
        {
            root = SetUpTree();
        }

        protected void Start()
        {
            if (gameObject.GetComponentInParent(typeof(CagedEmu))) isCaged = true;
        }

        private void Update()
        {
            if (root == null || isCaged) return;
            root.Evaluate();
        }

        protected abstract Node SetUpTree();

    /// <summary>
    /// Allows a non MonoBehaviour Class attached to this one to initiate is coroutine through MonoBehaviour.
    /// This is useful for scripts which do not inherit MonoBehaviour directly.
    /// This should only be used by classes whose lifetime is directly linked to the lifetime of this one 
    /// </summary>
    /// <param name="routine"></param>
    /// <returns></returns>
        public Coroutine Begin(IEnumerator routine)
        {
           return StartCoroutine(routine);
        }
    /// <summary>
    /// Allows a non MonoBehaviour class attached to this one to end any coroutine it owns. 
    /// </summary>
    /// <param name="routine"></param>
        public void End(Coroutine routine)
        {
            StopCoroutine(routine);
        }
    /// <summary>
    /// Stores some data with a string key.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
        public void SetData(string key, object value)
        {
            sharedData[key] = value;
        }
    /// <summary>
    /// Retrieves data using a string key. The string given must be an exact match.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
        public object GetData(string key)
        {
            sharedData.TryGetValue(key, out object value);
            return value;
        }
    /// <summary>
    /// Clears data using a string key. The string given must be an exact match, else the data will remain.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
        public bool ClearData(string key)
        {
              return  sharedData.Remove(key);
        }
    /// <summary>
    /// Disables this GameObject and returns it to the ObjectPool.
    /// </summary>
    public void ReturnToPool()
        {
            gameObject.SetActive(false);
        }

        protected override void Die()
        {
            statOwner?.RemoveFromArmy();
            ReturnToPool();
        }

        public bool Release()
        {
            Collider[] results = new Collider[10];
            int resultLen = Physics.OverlapSphereNonAlloc(transform.position, 3, results);
            if(resultLen == 0) return false;
            for (int i = 0; i < resultLen; i++)
            {
                if (results[i].gameObject.TryGetComponent(out IStatOwner stats) && stats.ArmySize < stats.MaxArmySize)
                {
                    statOwner = stats;
                    statOwner?.AddToArmy();
                    break;
                }
                if(i == resultLen - 1) return false;
            }
            
            isCaged = false;
            gameObject.transform.SetParent(null);
            return true;
        }
    }
}

