using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEditor.SearchService;

public class CheckForPlayerInRange : Node
{
   private BehaviourTree.Tree tree;
    private Transform _transform;
    private static LayerMask enemyLayer = 1 << 6;
    private float fovRange;
    public CheckForPlayerInRange(IBehaviourTreeDependancies tree)
    {
        _transform = tree.Transform;
        fovRange = tree.FOVRange;
    }

    public CheckForPlayerInRange(IBehaviourTreeDependancies tree, LayerMask mask)
    {
        this.tree = tree.Tree;
        _transform = tree.Transform;
        fovRange = tree.FOVRange;
        enemyLayer = mask;
    }


    public override NodeState Evaluate()
    {
        bool isSafe = true;
        Collider[] cols = Physics.OverlapSphere(_transform.position, fovRange, enemyLayer);
        tree.SetData("Colliders", cols);
            if (cols.Length > 0)
            {
                Transform target = cols[0].transform;
                 tree.SetData("Target", target);
               float distanceFromTarget = Vector3.Distance(target.position, _transform.position);
               isSafe = distanceFromTarget > (fovRange / 2);
               Debug.Log($"isSafe: {isSafe}");
                tree.SetData("Safe Distance", isSafe);
                 state = NodeState.SUCCESS;
                return state;
            }
        tree.SetData("Safe Distance", isSafe);
        tree.ClearData("Target");

            state = NodeState.RUNNING;
            return state;
    }
}
