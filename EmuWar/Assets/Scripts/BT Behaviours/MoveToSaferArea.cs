using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToSaferArea : BehaviourTree.Node
{
    private float radius;
    private BehaviourTree.Tree tree;
    private Transform transform;
    private  float speed;
    private bool finished = true;
    Coroutine moving;
    public MoveToSaferArea(IBehaviourTreeDependancies tree)
    {
        radius = tree.RetreatRadius;
        this.tree = tree.Tree;
        transform = tree.Transform;
        this.speed = tree.Speed;
    }
    public override NodeState Evaluate()
    {
        //if not being shot at, dont need to find any cover.
          if(!StatusCheck())  return NodeState.FAILED;
        
      //Checks to see if the entity has finshed any previous set destination, and
      //sets a random position in a set radius from the entity's position and starts the coroutine for movement.
        if (finished)
        {   
            Vector3 randomPos = transform.position + new Vector3(Random.Range(-radius, radius), 0, Random.Range(-radius, radius));
            tree.SetData("Finding Cover", true);
            moving = tree.Begin(MoveTo(randomPos));
            return NodeState.RUNNING;
        }

        return NodeState.SUCCESS;
    }

    /// <summary>
    /// Moves to a set position. Does its own check each tick to see if the coroutine is still needed, and ends itself if not.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    private IEnumerator MoveTo(Vector3 pos)
    {
        finished = false;
        while (Vector3.Distance(transform.position, pos) > 2f)
        {
            StatusCheck();
            transform.position = Vector3.MoveTowards(transform.position, pos, speed * Time.deltaTime );

            yield return null;
        }
        finished = true;
        tree.SetData("Finding Cover", false);
    }

    /// <summary>
    /// Checks if entity is still being shot at.
    /// If not, will stop the coroutine if it is running.
    /// </summary>
    /// <returns></returns>
    private bool StatusCheck()
    {
       bool beingShot = (bool)tree.GetData("BeingShot");
        bool safe = (bool)tree.GetData("Safe Distance");
        if (!beingShot && safe)
        {
            if (moving != null)
            {
                tree.End(moving);
                finished = true;
                moving = null;
            }
            tree.SetData("Finding Cover", false);
        }

        return beingShot && !safe;
    }
}
