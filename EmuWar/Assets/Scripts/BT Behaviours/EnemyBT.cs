using BehaviourTree;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyBT : BehaviourTree.Tree, IBehaviourTreeDependancies
{
    [SerializeField] private Transform[] targetLocations;
    [SerializeField] protected LayerMask playerMask;
    [SerializeField] private float fovRange;
    [SerializeField] private float minimumRange;
    [SerializeField] private float retreatRadius;
    public Patrol Patrol { get; private set; }
    private Base spawner;
    public Node ThisNode { get; private set; }
    private Vector3[] TargetPositions => targetLocations.Length != 0? targetLocations.Select(x => x.position).ToArray() : new Vector3[]{Vector3.zero};
    public Animator Animator => transform.Find("EMUanimated").GetComponent<Animator>();
    public Rigidbody Rigidbody { get; private set; }
    public float FOVRange => fovRange;
    public float Speed => speed;
    public Transform Transform => transform;
    public float MinimumDistance => minimumRange;
    public float RetreatRadius => retreatRadius;
    public BehaviourTree.Tree Tree => this;

    protected override Node SetUpTree()
    {
        Rigidbody = transform.GetComponent<Rigidbody>();
        
         ThisNode = new Selector(new List<Node>
        {
            new Selector(new List<Node>{
                 new Sequence(new List<Node>
                 {
                     new CheckForPlayerInRange(this, playerMask),
                     new CheckIfBeingShotAt(this),
                    new GoToTarget(this),
                   
                    new BeginAttack(this),
                 }),
                 new MoveToSaferArea(this),
            }),

             new Patrol(this,TargetPositions)
        });
        Patrol = ThisNode.NodeType<Patrol>();
        SetTargets(TargetPositions.ToList());
        return ThisNode;
    }
    public EnemyBT SetTargets(List<Vector3> positions)
    {
        Patrol.PatrolTargets(positions);
        return this;
    }

    public EnemyBT SetBase(Base caller)
    {
        spawner = caller;
        return this;
    }

    protected override void Die()
    {
        base.Die();
        GameManager.Instance.ObjectiveProgress();
        if (spawner == null) return;
        spawner.RemoveEntityFromList(this);
    }
    /*private void Awake()
    {
        health = enemyHealth;
        maxHealth = health;
    }*/
 
}