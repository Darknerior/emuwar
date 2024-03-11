
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BehaviourTree
{
    /// <summary>
    /// This series will run its children only if all previous ones have succeeded. 
    /// If any fail, the sequence fails. 
    /// If they all succeed, the sequence succeeds.
    /// 
    /// </summary>
    public class Sequence : Node
    {
        public Sequence() : base() { }
        public Sequence(List<Node> children) : base(children) { }

        public override NodeState Evaluate()
        {
            bool childrenAreRunning = false;

            for(int i = 0; i < children.Count; i++)
            {
                switch (children[i].Evaluate())
                {
                    case NodeState.FAILED:
                        state = NodeState.FAILED;
                        return state;
                    case NodeState.SUCCESS:
                        continue;
                    case NodeState.RUNNING:
                       // i--;
                        childrenAreRunning = true;
                        continue;
                    default:
                        state = NodeState.SUCCESS;
                        return state;
                }
            }
            state = childrenAreRunning ? NodeState.RUNNING : NodeState.SUCCESS;
            return state;
        }
    }
}
