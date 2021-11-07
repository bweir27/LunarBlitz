using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTNode
{
    public enum Result { Running, Failure, Success };

    public BehaviourTree Tree { get; set; }

    public BTNode(BehaviourTree t)
    {
        // assign tree to be the t we passed in
        Tree = t;
    }

    // called by the Behaviour Tree execution
    public virtual Result Execute()
    {
        Debug.Log("BTNode - Failure");
        return Result.Failure;
    }
}
