using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobBTRepeaterNode : Decorator
{
    public MobBTRepeaterNode(MobBehaviorTree t, MobBTNode c) : base(t, c)
    {

    }

    public override Result Execute()
    {
        // capture the result of the child
        Result result = Child.Execute();

        return result;
    }
}
