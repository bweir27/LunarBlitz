using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobBTNode
{
    // Running -> Mob is still moving toward end Tile
    // Failure -> Mob has been killed
    // Success -> Mob has reached end Tile
    public enum Result { Running, Failure, Success };

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
}
