using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobBTSequencer : MobBTComposite
{
    private int currentNode = 0;

    public MobBTSequencer(MobBehaviorTree t, MobBTNode[] children) : base(t, children)
    {

    }

    public override Result Execute()
    {
        if (currentNode < Children.Count)
        {
            // capture the result of the child
            Result result = Children[currentNode].Execute();

            // let the child run until it's finished
            if (result == Result.Running)
            {
                return result;
            }
            else if (result == Result.Failure)
            {
                currentNode = 0;
                return result;
            }
            else // Success
            {
                currentNode++;

                // I don't know how the current computation will complete
                // so just return running
                if (currentNode < Children.Count)
                {
                    return Result.Running;
                }
                else
                {
                    currentNode = 0;
                    return Result.Success;
                }
            }
        }
        return Result.Success;
    }
}
