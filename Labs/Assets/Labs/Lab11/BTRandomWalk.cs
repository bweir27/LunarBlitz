using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTRandomWalk : BTNode
{
    protected Vector3 NextDestination { get; set; }
    public float speed = 2.0f;

    public BTRandomWalk(BehaviourTree t) : base(t)
    {
        NextDestination = Vector3.zero;
        FindNextDestination();
    }

    public bool FindNextDestination()
    {
        // an object to write to that TryGetValue will use
        object o;

        // start by assuming we didn't find it
        bool found = false;
        found = Tree.Blackboard.TryGetValue("WorldBounds", out o);

        if (found)
        {
            Rect bounds = (Rect)o;

            // pick a new x and y to walk to
            float x = UnityEngine.Random.value * bounds.width;
            float y = UnityEngine.Random.value * bounds.height;

            NextDestination = new Vector3(x, y, NextDestination.z);
        }
        return found;
    }

    public override Result Execute()
    {
        // have we made it to the destination?
        if(Tree.gameObject.transform.position == NextDestination)
        {
            Debug.Log("DESTINATION REACHED!");
            // fail if we can't find a next destination
            if (!FindNextDestination())
                return Result.Failure;
      
            // Not sure if I like this
            return Result.Success;
        }
        else
        {
            // otherwise, move towards our next destination as the given speed
            Tree.gameObject.transform.position =
                Vector3.MoveTowards(
                    Tree.gameObject.transform.position,
                    NextDestination,
                    speed * Time.deltaTime);

            // we're not there yet, so return running
            return Result.Running;
        }
    }
}
