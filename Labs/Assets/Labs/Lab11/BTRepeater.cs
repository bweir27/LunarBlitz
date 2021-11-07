using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTRepeater : Decorator
{
    public BTRepeater(BehaviourTree t, BTNode c) : base(t, c)
    {

    }

    public override Result Execute()
    {
        // execute the child first (like a good parent)
        Result r = Child.Execute();
        Debug.Log("Repeater Child returned: " + r);
        
        // always return running
        return Result.Running;
    }
}
