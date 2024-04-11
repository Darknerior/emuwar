using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

namespace BehaviourTree
{
    /// <summary>
    /// Base Class for Behaviour tree. This is where the root node for the tree begins.
    /// </summary>
    public abstract class Tree : GameEntity,IPoolable
    {
        private Node root = null;
        private Dictionary<string, object> sharedData = new();

        protected void Awake()
        {
            root = SetUpTree();
        }

        private void Update()
        {
            if (root == null) return;
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
            ReturnToPool();
        }
    }
}

