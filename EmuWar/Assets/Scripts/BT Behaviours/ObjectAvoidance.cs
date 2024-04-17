using System;
using System.Collections.Generic;
using System.Linq;
using BehaviourTree;
using Tools;
using UnityEngine;

public class ObjectAvoidance : Node
{
   private BehaviourTree.Tree Tree;
   private Transform agent;
   private float range;
   private LayerMask immovable;
   
   public ObjectAvoidance(IBehaviourTreeDependancies tree)
   {
      Tree = tree.Tree;
      agent = tree.Transform;
      range = tree.FOVRange;
      immovable = LayerMask.NameToLayer("Immovable");
   }

   //This class kind of works. Its not very reliable but i dont know what is and isnt working to be able to fix it. 
   public override NodeState Evaluate()
   {
      //Find everything in range which is in the "Immovable" Layer
      var cols = Physics.OverlapSphere(agent.position, range).Where(x =>
         x.gameObject.layer == immovable && Vector3.Dot(agent.forward,(x.transform.position - agent.position).normalized) > 0.95f).ToList();
      if (cols.Count == 0) return NodeState.SUCCESS;
      //Sort them so the closest is at the front of the list
      cols.Sort((x,y) => (Vector3.Distance(agent.position, x.transform.position) * 100).ToInt());

      var direction = (cols[0].transform.position - agent.position).normalized;
      var dot = Vector3.Dot(agent.forward, direction);
     // Debug.Log($"{dot:n2}  {cols[0].gameObject.name}");
      if (dot >= .95f)
      {
        // Debug.Log($"Looking at {cols[0].gameObject.name}");
         var col = cols[0].bounds;
         var bounds1 = (agent.position - col.min).normalized;
         var bounds2 = ( agent.position - col.max).normalized;

         float dotForBounds1 = Vector3.Dot(agent.forward, bounds1);
         float dotForBounds2 = Vector3.Dot(agent.forward, bounds2);

         var target = dotForBounds1 < dotForBounds2 ? -col.extents : col.extents;

        // Debug.Log(dotForBounds1 < dotForBounds2 ? $"Rotating to {col.min}" : $"Rotating to {col.max}");
         // agent.Rotate(Vector3.RotateTowards(agent.rotation.eulerAngles, target, 360f * Mathf.Deg2Rad, 0f));
         Tree.SetData("ObjectNav", col.center + col.extents + new Vector3(1f,0f,1f).Multiply(target.Sign()));
      }
      
      return NodeState.SUCCESS;
   }
   
}
