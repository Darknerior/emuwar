using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree {
    /// <summary>
    /// Evaluates its child nodes in order, and returns success if any succeed.
    /// </summary>
    public class Selector : Node
    {
        public Selector() : base() { }
        public Selector(List<Node> nodes) : base(nodes) { }

        public override NodeState Evaluate()
        {
            for(int i = 0 ; i < children.Count; i ++)
            {
                switch (children[i].Evaluate())
                {
                    case NodeState.FAILED:
                        continue;
                    case NodeState.SUCCESS:
                        state = NodeState.SUCCESS;
                        return state;
                    case NodeState.RUNNING:
                        state = NodeState.RUNNING;
                        return state;
                    default:
                        continue;
                }
            }
            state = NodeState.FAILED;
            return state;
        }
    }
}
