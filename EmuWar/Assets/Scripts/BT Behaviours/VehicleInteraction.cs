using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BehaviourTree;
using Interfaces;
using UnityEngine;
using Tree = BehaviourTree.Tree;

public class VehicleInteraction : Node
{

    private GameEntity treeEntity;
    private Tree tree;
    private Transform agent;
    private PlayerController player;
    private Collider myCol;
    private float minDist;
    private bool inVehicle;
    public VehicleInteraction(IBehaviourTreeDependancies tree)
    {
        this.tree = tree.Tree;
        treeEntity = this.tree;
        agent = tree.Transform;
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        minDist = tree.MinimumDistance;
        myCol = tree.Transform.GetComponent<Collider>();
    }
    
    public override NodeState Evaluate()
    {
        if (player.inVehicle && !inVehicle)
        {
            if (Vector3.Distance(agent.position, player.transform.position) <= minDist)
            {
                var col = Physics.OverlapSphere(agent.position, minDist)
                    .FirstOrDefault(x => x.TryGetComponent(out INPCInteractible _));
                if (col == null) return NodeState.SUCCESS;

               var car = col.GetComponent<INPCInteractible>();
                inVehicle = car.Interact(treeEntity);
                myCol.enabled = !inVehicle;
                myCol.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
            }
        }
        else if (inVehicle && !player.inVehicle)
        {
            var car = Physics.OverlapSphere(agent.position, minDist)
                .First(x => x.TryGetComponent(out INPCInteractible _)).GetComponent<INPCInteractible>();
                inVehicle = car.ExitVehicle(treeEntity);
                myCol.enabled = !inVehicle;
                myCol.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
        
        tree.SetData("InVehicle", inVehicle);
        return NodeState.SUCCESS;
    }
}
