using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobBehaviorTree : MonoBehaviour
{
    // the root of the tree
    private MobBTNode mRoot;

    // has the behaviour started
    private bool startedBehaviour;

    // access to the behaviour coroutine
    private Coroutine behaviour;

    // HashTable
    // access to the blackboard
    public Dictionary<string, object> Blackboard { get; set; }
    //public Dictionary<string, object> PathTileList

    // public access to the root
    public MobBTNode Root { get { return mRoot; } }

    public Rect bounds;

    // Start is called before the first frame update
    void Start()
    {
        // create Blackboard
        Blackboard = new Dictionary<string, object>();

        if (bounds == null)
        {
            bounds = new Rect(0, 0, 4, 4);
        }
        // setup keys that every AI needs
        Blackboard.Add("WorldBounds", bounds);
        List<GameObject> pathTiles = MapGenerator.pathTiles;
        if(pathTiles.Count > 0)
        {
            Blackboard.Add("PathTiles", pathTiles);
        }
        else
        {
            Debug.LogError("BT Error retrieving PathTiles");
        }

        startedBehaviour = false;

        mRoot = new MobBTRepeatUntilFinishOrDead(this,
                    new MobBTSequencer(this,
                        new MobBTNode[] { new MobBTWalk(this)
                        }));
    }

    // Update is called once per frame
    void Update()
    {
        if (!startedBehaviour)
        {
            behaviour = StartCoroutine(RunBehaviour());
            startedBehaviour = true;
        }
    }

    private IEnumerator RunBehaviour()
    {
        MobBTNode.Result result = Root.Execute();

        while (result == MobBTNode.Result.Running)
        {
            //Debug.Log("Root result: " + result);
            yield return null;
            result = Root.Execute();
        }

        Debug.Log("Behaviour has finished with: " + result);
    }
}
