using BehaviourTree;
using UnityEngine;

public class GoToTarget : Node
{
    private BehaviourTree.Tree tree;
    private Rigidbody rb;
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
        rb = tree.Rigidbody;
    }


    public override NodeState Evaluate()
    {
        
        Transform target = (Transform)tree.GetData("Target");
        
        if (target == null) return NodeState.FAILED;
        var targetPos = target.position;
        float distanceFromTarget = Vector3.Distance(targetPos, _transform.position);

        if (tree.GetData("Finding Cover") != null && (bool)tree.GetData("Finding Cover"))
        {
            _transform.LookAt(targetPos);
            _animator.SetBool("Walking", true);
            return NodeState.SUCCESS;
        }

        if(distanceFromTarget > fovRange)
        {
            state = NodeState.FAILED;
        }
        else if (distanceFromTarget > minDistance)
        {
            
            _transform.position = Vector3.MoveTowards(_transform.position, targetPos, speed * Time.deltaTime);
            _transform.LookAt(targetPos);
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
    Rigidbody Rigidbody { get; }
    float FOVRange { get; }
    float Speed {  get; }
    Transform Transform { get; }
    float MinimumDistance { get; }
    float RetreatRadius { get; }
}
