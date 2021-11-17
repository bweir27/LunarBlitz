using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobBTRepeatUntilFinishOrDead : Decorator
{
    public MobBTRepeatUntilFinishOrDead(MobBehaviorTree t, MobBTNode c) : base(t, c)
    {

    }

    public override Result Execute()
    {
        // capture the result of the child
        Result result = Child.Execute();

        // if enemyHealth <= 0: die, award gold, return Result.Failure
        // if collistion w/ bullet -> takeDamage()
        // if at targetTile:
        //  - if currTile != endTile
        //       - FindNextDestination() -> Result.Running
        //  - if currTile == endTile
        //      - cost Player lives (Result.Success)

        Debug.Log("RepeatUntilDoneOrDead Result: " + result);
        return result;
        /*
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
        */
    }
}
