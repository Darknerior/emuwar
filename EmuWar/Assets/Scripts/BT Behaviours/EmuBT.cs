using UnityEngine;
using System.Collections.Generic;
using BehaviourTree;

public class EmuBT : BehaviourTree.Tree, IBehaviourTreeDependancies
{
    public Transform targetPosition;
    [SerializeField] private float fovRange = 6f;
    [SerializeField] private float minimumRange;
    [SerializeField] private float retreatRadius;
    [SerializeField] private float followDistance;
    [SerializeField] private LayerMask enemyMask;
    public Animator Animator => GetComponent<Animator>();
    public Rigidbody Rigidbody { get; private set; }
    public float FOVRange => fovRange;
    public float Speed => speed;
    public Transform Transform => transform;

    public float MinimumDistance => minimumRange;
    public BehaviourTree.Tree Tree => this;
    public float RetreatRadius => retreatRadius;
    public float FollowDistance => followDistance;

    protected override Node SetUpTree()
    {
        Rigidbody = GetComponent<Rigidbody>();
        targetPosition = GameObject.Find("Player").transform;
        Node root = new Selector(new List<Node>
        {
            new Selector( new List<Node>(){
                
                    new Sequence(new List<Node>
                    {
                         new CheckForPlayerInRange(this,enemyMask),
                         new CheckIfBeingShotAt(this),
                         new GoToTarget(this),
                         new BeginAttack(this),
                    }),
                  new MoveToSaferArea(this),
            }),
            
            new Sequence(new List<Node>(){
                new Patrol(this,targetPosition,followDistance),
                new ObjectAvoidance(this),
                 new Flock(this, targetPosition),
                 new VehicleInteraction(this)
            })
           
        }) ;

        return root;
    }
}
