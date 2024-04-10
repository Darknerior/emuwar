using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


namespace BehaviourTree {

    public enum NodeState
    {
        RUNNING, 
        SUCCESS,
        FAILED,
    }

    /// <summary>
    /// Node contains links to its parent and children, and contains logic to return a success of failed state. 
    /// This class is not used Directly, but is the base class which different behaviours will implement.
    /// </summary>
    public class Node 
    {
        protected NodeState state;

        public Node parent;
        protected List<Node> children = new();

        public List<Node> Children => children;

        public T NodeType<T>() where T : Node
        {
            foreach (var child in children)
            {
                if (child.GetType() == typeof(T))
                {
                    var thing = child as T;
                    return thing;
                }
            }

            return null;
        }

        public Node() 
        {
            parent = null;
        }
        public Node(List<Node> chidren) 
        {
            foreach (Node child in chidren)
            {
                Attatch(child);
            }
        }

        private void Attatch(Node node)
        {
            node.parent = this;
            children.Add(node);
        }

        public virtual NodeState Evaluate() => NodeState.FAILED;
        
    } 
}
