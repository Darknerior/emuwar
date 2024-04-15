using System.Collections.Generic;
using System.Linq;
using BehaviourTree;
using JetBrains.Annotations;
using Tools;
using UnityEngine;
using Tree = BehaviourTree.Tree;

public class Patrol : Node
{
    private Transform _agent;
    private Transform target;
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
    private bool targetNotNull;
    private float distance;
    public Vector3 Position => _agent.position;
    public Patrol(IBehaviourTreeDependancies tree,Vector3[] positions)
    {
        _agent = tree.Transform;
        anim = tree.Animator;
        speed = tree.Speed;
        rb = tree.Rigidbody;
        this.tree = tree;
        distance = 0.25f;
    }
    public Patrol(IBehaviourTreeDependancies tree,Transform position, float followDistance)
    {
        _agent = tree.Transform;
        anim = tree.Animator;
        speed = tree.Speed;
        rb = tree.Rigidbody;
        this.tree = tree;
        target = position;
        distance = followDistance;
        targetPositions.Add(position.position);
        targetNotNull = true;
        waitTime = 0f;
    }
    public override NodeState Evaluate()
    {
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
                
                if (!targetNotNull) anim.SetBool("Walking", true);
            }
        }
        else
        {
            Vector3 position = targetPositions[currentIndex];
            if (targetNotNull) position = target.position;
            
            //ChangeAxisValue will return position, and _agent.position with the y axis set to zero.
            //This mitigates any elevation changes for the distance check
            Vector3 fixedPosition = position.ChangeAxisValue(Vector3.up, 0f);
            Vector3 fixedEmuPos = _agent.position.ChangeAxisValue(Vector3.up,0f);
            
            if(Vector3.Distance(fixedEmuPos, fixedPosition) <= distance)
            {
                waitCounter = 0f;
                isWaiting = true;
                if (!targetNotNull) anim.SetBool("Walking", false);
                currentIndex = (currentIndex + 1) % targetPositions.Count;
            }
            else
            {
                tree.Tree.SetData("Movement", position);
                tree.Tree.SetData("LookAt",new Vector3(position.x,Position.y,position.z));
              //  var movement = Vector3.MoveTowards(_agent.position, position, speed * Time.deltaTime);
              //  rb.MovePosition(movement);
              //  _agent.LookAt(new Vector3(position.x,Position.y,position.z));
            }
        }

        tree.Tree.SetData("Position", _agent.position);

        state = NodeState.RUNNING;
        return state;
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
