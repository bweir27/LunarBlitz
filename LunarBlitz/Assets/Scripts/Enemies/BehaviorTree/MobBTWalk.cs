using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobBTWalk : MobBTNode
{
    protected Vector3 NextDestination { get; set; }
    protected GameObject TargetTile { get; set; }
    protected float speed;

    public MobBTWalk(MobBehaviorTree t) : base(t)
    {
        NextDestination = MapGenerator.startTile.transform.position;
        TargetTile = MapGenerator.startTile;
        //speed = Tree.gameObject.GetComponent<Enemy>().
        //FindNextDestination();
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
        // have we made it to the destination?
        if (Tree.gameObject.transform.position == NextDestination)
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
