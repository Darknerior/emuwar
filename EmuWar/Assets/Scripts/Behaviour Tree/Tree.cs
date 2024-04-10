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
        private IPoolable poolableImplementation;

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


        public Coroutine Begin(IEnumerator routine)
        {
           return StartCoroutine(routine);
        }

        public void End(Coroutine routine)
        {
            StopCoroutine(routine);
        }

        public void SetData(string key, object value)
        {
            sharedData[key] = value;
        }

        public object GetData(string key)
        {
            sharedData.TryGetValue(key, out object value);

            return value;
        }

        public bool ClearData(string key)
        {
              return  sharedData.Remove(key);
        }

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

