using UnityEngine;
using System.Collections.Generic;
using BehaviourTree;

public class EmuBT : BehaviourTree.Tree, IBehaviourTreeDependancies
{
    public Transform[] tragetPositions;
    [SerializeField] private float speed = 2;
    [SerializeField] private float fovRange = 6f;
    [SerializeField] private float minimumRange;
    [SerializeField] private float retreatRadius;

    public Animator Animator => GetComponent<Animator>();

    public float FOVRange => fovRange;

    public float Speed => speed;

    public Transform Transform => transform;

    public float MinimumDistance => minimumRange;
    public BehaviourTree.Tree Tree => this;
    public float RetreatRadius => retreatRadius;

    protected override Node SetUpTree()
    {
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
              new CheckForPlayerInRange(this),
              new GoToTarget(this)
            }),

            new Patrol(this,tragetPositions)
        }) ;

        return root;
    }
}
