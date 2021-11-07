using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTFailNode : BTNode
{
    public BTFailNode(BehaviourTree t) : base(t)
    {

    }

    public override Result Execute()
    {
        Debug.Log("BTFailNode!");
        return Result.Failure;
    }
}
