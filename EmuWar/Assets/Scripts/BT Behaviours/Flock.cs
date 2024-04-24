using System.Collections.Generic;
using System.Linq;
using BehaviourTree;
using Tools;
using UnityEngine;

public class Flock : Node
{
    private IBehaviourTreeDependancies Tree;

    private Collider[] neighbours;
    private float range;
    private Collider myCol;
    private Transform target;
    public Flock(IBehaviourTreeDependancies tree, Transform target)
    {
        Tree = tree;
        myCol = tree.Transform.GetComponent<Collider>();
        range = Tree.FOVRange;
        this.target = target;
    }
    
    //Checks first to see if it is near it target, if so, return.
    //Finds the neighbours that will influence its position, then gets the "Movement" information from previous 
    //behaviours, and adds its own to that, and applies the movement.
    public override NodeState Evaluate()
    {
        if (Vector3.Distance(Tree.Transform.position, target.position) < Tree.MinimumDistance) return NodeState.SUCCESS;
        neighbours = FindNeighbours();
        var move = (Cohesion() + Alignment() + Avoidance());
        object info = Tree.Tree.GetData("Movement");
        Vector3 modify = Vector3.zero;
        if (info != null)  modify = (Vector3)info;
        modify -= Tree.Tree.transform.position;
        modify += move;
        
        modify = modify.ChangeAxisValue(Vector3.up,0f);
        Tree.Transform.LookAt(Tree.Transform.position + Vector3.Lerp(Vector3.zero, modify, Tree.Speed*Time.deltaTime));
        Tree.Rigidbody.MovePosition(Vector3.MoveTowards(Tree.Transform.position, Tree.Transform.position+Tree.Transform.forward, Tree.Speed * Time.deltaTime));
        return NodeState.RUNNING;
    }
    /// <summary>
    /// Returns a target position which is the centre point of all applicable neighbours
    /// </summary>
    /// <returns></returns>
    private Vector3 Cohesion()
    {
        //if there is nothing, you have no neighbours
        if (neighbours.Length == 0) return Vector3.zero;
        List<Vector3> neighbourPositions = new List<Vector3>();
        //for every neighbour, store their position in a list
        foreach (var neighbour in neighbours) neighbourPositions.Add(neighbour.transform.position);
        //Average the position of all of them to get a target position, then minus your position to get an offset 
        return (neighbourPositions.Average() - Tree.Transform.position);
    }
    /// <summary>
    /// Returns the Average direction of all applicable neighbours
    /// </summary>
    /// <returns></returns>
    private Vector3 Alignment()
    {
        if (neighbours.Length == 0) return Tree.Transform.forward;
        var following = neighbours.Where(x => x.transform != target).ToArray();
        List<Vector3> AvgHeading = new List<Vector3>();
        foreach (var follower in following) AvgHeading.Add(follower.transform.forward);

        return AvgHeading.Average();
    }
    /// <summary>
    /// Return a position which ensures entity is not overlapping another.
    /// </summary>
    /// <returns></returns>
    private Vector3 Avoidance()
    {
        if (neighbours.Length == 0) return Vector3.zero;
        var tooClose = neighbours.Where(x =>
            Vector3.Distance(x.transform.position, Tree.Transform.position) < Tree.MinimumDistance).ToArray();

        if (tooClose.Length == 0) return Vector3.zero;
        List<Vector3> pos = new List<Vector3>();
        Vector3 myPos = Tree.Transform.position;
        foreach (var collider in tooClose) pos.Add(myPos - collider.transform.position);

        return pos.Average();
    }
 
    private Collider[] FindNeighbours() => Physics.OverlapSphere(Tree.Transform.position, range, 1 << 6)
                                                  .Where(x => x.transform.gameObject.layer == Tree.Transform.gameObject.layer && x != myCol)
                                                  .ToArray();
    
}