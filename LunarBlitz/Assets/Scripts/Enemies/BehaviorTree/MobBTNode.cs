using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobBTNode
{
    // Running -> Mob is still moving toward end Tile
    // Failure -> Mob has been killed
    // Success -> Mob has reached end Tile
    public enum Result { Running, Failure, Success };
    public GameObject TargetTile { get; set; }
    public GameObject EndTile { get; set; }
    protected float movementSpeed;
    

    public MobBehaviorTree Tree { get; set; }

    public MobBTNode(MobBehaviorTree t)
    {
        // assign tree to be the t we passed in
        Tree = t;
    }

    // called by the Behaviour Tree execution
    public virtual Result Execute()
    {
        Debug.Log("MobBTNode - Failure");
        return Result.Failure;
    }

    public virtual float DistanceToTarget()
    {
        return (Tree.gameObject.transform.position - TargetTile.transform.position).magnitude;
    }
}
