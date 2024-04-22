using System.Linq;
using UnityEngine;
using BehaviourTree;
using Interfaces;

public class CheckForPlayerInRange : Node
{
    private BehaviourTree.Tree tree;
    private Transform _transform;
    private  LayerMask enemyLayer = 1 << 6;
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
        object info = tree.GetData("InVehicle");
        if (info != null)
        {
            bool inVehicle = (bool)info;
            if(inVehicle) return NodeState.FAILED;
        }
        bool isSafe = true;
        Collider[] cols = Physics.OverlapSphere(_transform.position, fovRange, enemyLayer)
            .Where(x =>
            {
                x.TryGetComponent(out ICagedEmu emu);
                if (emu == null) return true;
                return !emu.IsCaged;
            }).ToArray();
        tree.SetData("Colliders", cols);
        if (cols.Length > 0)
        {
                
            Transform target = cols[0].transform;
            tree.SetData("Target", target);
            float distanceFromTarget = Vector3.Distance(target.position, _transform.position);
            isSafe = distanceFromTarget > (fovRange / 2);
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