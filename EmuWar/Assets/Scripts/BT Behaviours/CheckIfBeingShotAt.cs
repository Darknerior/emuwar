using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CheckIfBeingShotAt : BehaviourTree.Node
{
   private BehaviourTree.Tree tree;
    private LayerMask layer;
    Transform transform;
    Vector3 position;
    float safeRadius;
    float fovRange;
    public CheckIfBeingShotAt(IBehaviourTreeDependancies tree, LayerMask mask)
    {
        this.tree = tree.Tree;
        position = tree.Transform.position;
        transform = tree.Transform;
        safeRadius = tree.MinimumDistance*2;
        fovRange = tree.FOVRange;
        layer = mask;
    }
    public override NodeState Evaluate()
    {
        Collider[] dangers = (Collider[])tree.GetData("Colliders");
        if (dangers.Length == 0) return NodeState.SUCCESS;
        foreach (Collider collider in dangers)
        {
            if (collider.gameObject.TryGetComponent(out Bullet _))
            {
                tree.SetData("BeingShot", true);
                return NodeState.FAILED;
            }
        }

        tree.SetData("BeingShot", false);
        return NodeState.SUCCESS;
    }

}
