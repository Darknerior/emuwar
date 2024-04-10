using System.Collections.Generic;
using System.Linq;
using BehaviourTree;
using JetBrains.Annotations;
using UnityEngine;
using Tree = BehaviourTree.Tree;

public class Patrol : Node
{
    private Transform _agent;
    private List<Vector3> targetPositions = new();
    private Animator anim;
    private float speed;
    private Rigidbody rb;
    private int currentIndex = 0;
    private IBehaviourTreeDependancies tree;
    private float waitTime = 1.5f;
    private float waitCounter = 0f;
    private bool isWaiting = false;
    private Base @base;
    public Vector3 Position => _agent.position;
    public Patrol(IBehaviourTreeDependancies tree,Vector3[] positions)
    {
        _agent = tree.Transform;
        anim = tree.Animator;
        speed = tree.Speed;
        rb = tree.Rigidbody;
        this.tree = tree;
    }
    public override NodeState Evaluate()
    {
        var currentpos = _agent.position;
        Vector3 movement = Vector3.zero;
        if (targetPositions.Count == 0)
        {
            return NodeState.FAILED;
        }
        if(isWaiting)
        {
            waitCounter += Time.deltaTime;
            if(waitCounter >= waitTime) 
            { 
                isWaiting = false;
                anim.SetBool("Walking", true);
            }
        }
        else
        {
            Vector3 position = targetPositions[currentIndex];
            Vector3 fixedPosition = new Vector3(position.x, 0, position.z);
            Vector3 fixedEmuPos = new Vector3(_agent.position.x, 0, _agent.position.z);
            if(Vector3.Distance(fixedEmuPos, fixedPosition) <= 0.25f)
            {
                waitCounter = 0f;
                isWaiting = true;
                anim.SetBool("Walking", false);
                currentIndex = (currentIndex + 1) % targetPositions.Count;
            }
            else
            {
                
                movement = Vector3.MoveTowards(_agent.position, position, speed * Time.deltaTime);
                rb.MovePosition(movement);
                _agent.LookAt(new Vector3(position.x,Position.y,position.z));
            }
        }

        tree.Tree.SetData("Position", _agent.position);

        state = NodeState.RUNNING;
        return state;
    }

    private void Jump()
    {
        if (rb.velocity.y == 0)
            rb.velocity += new Vector3(0, 10, 0);
    }
    public void PatrolTargets(List<Vector3> positions)
    {
        targetPositions = new();
        targetPositions.AddRange(positions);
    }

    public void DrawGizmos()
    {
        Gizmos.color = Color.green;
        foreach (var thing in targetPositions)
        {
            Gizmos.DrawWireSphere(thing,.5f);
        }
    }
}
