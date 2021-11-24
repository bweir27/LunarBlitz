using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobBehaviorTree : MonoBehaviour
{
    // the root of the tree
    public MobBTNode mRoot;

    public float distanceTraveled;
    // has the behaviour started
    private bool startedBehaviour;

    // access to the behaviour coroutine
    private Coroutine behaviour;

    // HashTable
    // access to the blackboard
    public Dictionary<string, object> Blackboard { get; set; }

    // public access to the root
    public MobBTNode Root { get { return mRoot; } }

    // Start is called before the first frame update
    void Start()
    {
        // create Blackboard
        Blackboard = new Dictionary<string, object>();

        // setup keys that every AI needs
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

        mRoot = new MobBTRepeaterNode(this,
                    new MobBTSequencer(this,
                        new MobBTNode[] {
                            new MobBTWalkNode(this)
                        }));

        distanceTraveled = 0f;
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
            yield return null;
            result = Root.Execute();
        }

        if(result == MobBTNode.Result.Failure)
        {
            Debug.LogError("Root Result -> FAILURE");
        }
        else if(result == MobBTNode.Result.Success)
        {
            //Mob has successfully reached the end, take lives
            PlayerController playerController = FindObjectOfType<PlayerController>();
            Enemy enemy = gameObject.GetComponent<Enemy>();
            playerController.loseLives(enemy.livesCost);

            // destroy the mob
            Enemies.enemies.Remove(gameObject);
            Destroy(transform.gameObject);
        }
    }
}
