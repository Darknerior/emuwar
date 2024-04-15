using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CheckIfBeingShotAt : BehaviourTree.Node
{
   private BehaviourTree.Tree tree;
    public CheckIfBeingShotAt(IBehaviourTreeDependancies tree)
    {
        this.tree = tree.Tree;
    }
    public override NodeState Evaluate()
    {
        bool danger = false;
        Collider[] dangers =  (Collider[])tree.GetData("Colliders");
            foreach (Collider collider in dangers)
            {
                if (collider.gameObject.TryGetComponent(out Bullet _))
                {
                   danger = true; 
                   break;
                }
            }
        tree.SetData("BeingShot", danger);
        return NodeState.SUCCESS;
    }

}
