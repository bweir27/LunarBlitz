using System.Collections;
using System.Collections.Generic;

public class BTRepeatUntilFailureNode : Decorator
{
    public BTRepeatUntilFailureNode(BehaviourTree t, BTNode c) : base(t, c)
    {

    }

    public override Result Execute()
    {
      
        // capture the result of the child
        Result result = Child.Execute();

        // if child is running, return running
        if (result == Result.Running || result == Result.Success)
        {
            return result;
        }
        else // Failure
        {
            // only when the child returns failure will it end
            // in that case, it returns with success
            return Result.Success;
        }
        
    }
}
