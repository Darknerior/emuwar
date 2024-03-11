using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Patrol : Node
{

    private Transform _agent;
    private Transform[] targetPositions;
    private Animator anim;
    private float speed;

    private int currentIndex = 0;

    private float waitTime = 1.5f;
    private float waitCounter = 0f;
    private bool isWaiting = false;
    public Patrol(IBehaviourTreeDependancies tree, Transform[] positions)
    {
        _agent = tree.Transform;
        targetPositions = positions;
        anim = tree.Animator;
        speed = tree.Speed;
    }
    public override NodeState Evaluate()
    {
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
            Transform position = targetPositions[currentIndex];
            if(Vector3.Distance(_agent.position, position.position) < 0.5f)
            {
                //_agent.position = position.position;
                waitCounter = 0f;
                isWaiting = true;
                anim.SetBool("Walking", false);
                currentIndex = (currentIndex + 1) % targetPositions.Length;
            }
            else
            {
                _agent.position = Vector3.MoveTowards(_agent.position, position.position, speed * Time.deltaTime);
                _agent.LookAt(position.position);
            }
        }

        state = NodeState.RUNNING;
        return state;
    }
}
