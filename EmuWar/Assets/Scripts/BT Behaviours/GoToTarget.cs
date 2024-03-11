using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToTarget : BehaviourTree.Node
{
    private BehaviourTree.Tree tree;
    private Transform _transform;
    private Animator _animator;
    private float fovRange;
    private float speed;
    private float minDistance;

    public GoToTarget(IBehaviourTreeDependancies tree)
    {
        this.tree = tree.Tree;
        _transform = tree.Transform;
        _animator = tree.Animator;
        fovRange = tree.FOVRange;
        speed = tree.Speed;
        minDistance = tree.MinimumDistance;
    }


    public override NodeState Evaluate()
    {
        
        Transform target = (Transform)tree.GetData("Target");
        if (target == null) return NodeState.FAILED;
        float distanceFromTarget = Vector3.Distance(target.position, _transform.position);

        if (tree.GetData("Finding Cover") != null && (bool)tree.GetData("Finding Cover"))
        {
            _transform.LookAt(target.position);
            _animator.SetBool("Walking", true);
            return NodeState.SUCCESS;
        }

        if(distanceFromTarget > fovRange)
        {
            state = NodeState.FAILED;
        }
        else if (distanceFromTarget > minDistance)
        {
            _transform.position = Vector3.MoveTowards(_transform.position, target.position, speed * Time.deltaTime);
            _transform.LookAt(target.position);
            _animator.SetBool("Walking", true);
            state = NodeState.RUNNING;
        }
        else
        {
            _transform.LookAt(target.position);
            state = NodeState.SUCCESS;
            _animator.SetBool("Walking", false);
        }
        return state;
    }
}

public interface IBehaviourTreeDependancies
{
    BehaviourTree.Tree Tree { get; }
    Animator Animator { get; }
    float FOVRange { get; }
    float Speed {  get; }
    Transform Transform { get; }
    float MinimumDistance { get; }
    float RetreatRadius { get; }
}
