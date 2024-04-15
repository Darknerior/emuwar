using System.Collections;
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

    public override NodeState Evaluate()
    {
        if (Vector3.Distance(Tree.Transform.position, target.position) < Tree.MinimumDistance) return NodeState.SUCCESS;
        neighbours = FindNeighbours();
        var move = (Cohesion() + Alignment() + Avoidance());
        //Tree.Transform.forward = Vector3.Slerp(Tree.Transform.forward,move,Time.deltaTime);
        Vector3 modify = (Vector3)Tree.Tree.GetData("Movement");
        modify -= Tree.Tree.transform.position;
        modify += move; 
        modify = modify.ChangeAxisValue(Vector3.up,0f);
        Debug.Log(modify);
            Tree.Transform.LookAt(Tree.Transform.position + Vector3.Lerp(Vector3.zero, modify, Tree.Speed*Time.deltaTime));
          Tree.Rigidbody.MovePosition(Vector3.MoveTowards(Tree.Transform.position, Tree.Transform.position+Tree.Transform.forward, Tree.Speed * Time.deltaTime));
        return NodeState.RUNNING;
    }

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

    private Vector3 Alignment()
    {
        if (neighbours.Length == 0) return Tree.Transform.forward;
        var following = neighbours.Where(x => x.transform != target).ToArray();
        List<Vector3> AvgHeading = new List<Vector3>();
        foreach (var follower in following) AvgHeading.Add(follower.transform.forward);

        return AvgHeading.Average();
    }

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
 
    private Collider[] FindNeighbours() => Physics
        .OverlapSphere(Tree.Transform.position, range, 1 << 6).Where(x => x.transform.gameObject.layer == Tree.Transform.gameObject.layer && x != myCol).ToArray();
    //.Where(x => x != myCol).ToArray();
}
