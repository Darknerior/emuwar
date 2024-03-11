using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBT : BehaviourTree.Tree, IBehaviourTreeDependancies
{
    [SerializeField] Transform[] targetLocations;
    [SerializeField] protected LayerMask playerMask;
    [SerializeField] private float fovRange;
    [SerializeField] private float speed;
    [SerializeField] private float minimumRange;
    [SerializeField] private float retreatRadius;

    public Animator Animator => gameObject.GetComponent<Animator>();

    public float FOVRange => fovRange;

    public float Speed => speed;

    public Transform Transform => transform;

    public float MinimumDistance => minimumRange;

    public float RetreatRadius => retreatRadius;

    public BehaviourTree.Tree Tree => this;

    protected override Node SetUpTree()
    {
        Node root = new Selector(new List<Node>
        {
            new Selector(new List<Node>{
                 new Sequence(new List<Node>
                 {
                     new CheckForPlayerInRange(this, playerMask),
                     new CheckIfBeingShotAt(this, playerMask),
                     
                    new GoToTarget(this),
                    new BeginAttack(this),
                 }),
                    new MoveToSaferArea(this),
             
            }),

            new Patrol(this,targetLocations)
        });

        return root;
    }

}