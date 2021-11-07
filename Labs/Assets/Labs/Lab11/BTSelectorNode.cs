using System.Collections;
using System.Collections.Generic;

public class BTSelectorNode : BTComposite
{
    private int currentNode = 0;

    public BTSelectorNode(BehaviourTree t, BTNode[] children) : base(t, children)
    {

    }

    public override Result Execute()
    {
        if (currentNode < Children.Count)
        {
            // capture the result of the child
            Result result = Children[currentNode].Execute();

            // let the child run until it returns success

            if (result == Result.Running)
            {
                // if current child is running, return running
                return result;
            }
            else if (result == Result.Failure)
            {
                // if it returns failure, return running and move on to next child
                currentNode++;
                return Result.Running;
            }
            else // Success
            {
                return Result.Success;
            }
        }

        // if no child returns Success
        return Result.Failure;
    }
}
