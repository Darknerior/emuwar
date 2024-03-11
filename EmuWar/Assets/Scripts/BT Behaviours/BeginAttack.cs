using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BeginAttack : BehaviourTree.Node
{
    Weapon weapon;
    public BeginAttack(IBehaviourTreeDependancies tree)
    {
       if(tree.Transform.TryGetComponent(out Weapon weapon))
        {
            this.weapon = weapon;
        }
        else
        {
            this.weapon = tree.Transform.AddComponent<Weapon>();
        }
    }
    public override NodeState Evaluate()
    {
        weapon.Shoot();
        return NodeState.RUNNING;
    }
}
