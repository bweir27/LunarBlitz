using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobBTWalkNode : MobBTNode
{
    protected Vector3 NextDestination { get; set; }
    protected Enemy thisMob;

    public MobBTWalkNode(MobBehaviorTree t) : base(t)
    {
        NextDestination = MapGenerator.startTile.transform.position;
        TargetTile = MapGenerator.startTile;
        movementSpeed = Tree.gameObject.GetComponent<Enemy>().movementSpeed;
        thisMob = Tree.gameObject.GetComponent<Enemy>();
    }

    public bool FindNextDestination()
    {
        // an object to write to that TryGetValue will use
        object o;

        // start by assuming we didn't find it
        bool found = false;
        found = Tree.Blackboard.TryGetValue("PathTiles", out o);

        if (found)
        {
            List<GameObject> pathTiles = (List<GameObject>)o;
            
            int currentIndex = MapGenerator.pathTiles.IndexOf(TargetTile);
            TargetTile = MapGenerator.pathTiles[currentIndex + 1];

            NextDestination = new Vector3(
                TargetTile.transform.position.x,
                TargetTile.transform.position.y,
                NextDestination.z);
        }
        return found;
    }

    public override Result Execute()
    {
        Vector3 currPos = Tree.gameObject.transform.position;

        // have we made it to the next Tile? round corners
        if (DistanceToTarget() <= 0.3 && NextDestination != MapGenerator.endTile.transform.position)
        {
            // fail if we can't find a next destination
            if (!FindNextDestination())
                return Result.Failure;

            // NextDestination Found, still traveling along path
            return Result.Running;
        }
        else if(currPos == MapGenerator.endTile.transform.position)
        {
            // mob has reached the end, register success
            return Result.Success;
        }
        else
        {
            // otherwise, move towards our destination at the given speed
            thisMob.moveEnemyTowards(NextDestination);
       
            // we're not there yet, so return running
            return Result.Running;
        }
    }

    private float DistanceFromTarget(Vector3 currPos, Vector3 dest)
    {
        return (currPos - dest).magnitude;
    }
}
