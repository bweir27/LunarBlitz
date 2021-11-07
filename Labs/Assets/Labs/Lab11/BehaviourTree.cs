using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTree : MonoBehaviour
{
    // the root of the tree
    private BTNode mRoot;

    // has the behaviour started
    private bool startedBehaviour;

    // access to the behaviour coroutine
    private Coroutine behaviour;

    // HashTable
    // access to the blackboard
    public Dictionary<string, object> Blackboard { get; set; }

    // public access to the root
    public BTNode Root { get { return mRoot; } }

    public bool isRepeatUntilFailNode = true;
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

        startedBehaviour = false;

        // the repeater made in class
        //mRoot = new BTRepeater(this,
        //        new BTSequencer(this,
        //            new BTNode[] { new BTRandomWalk(this),
        //                            new BTRandomWalk(this),
        //                            new BTFailNode(this)
        //                             }));

        if (isRepeatUntilFailNode)
        {
            mRoot = new BTRepeatUntilFailureNode(this,
                    new BTSequencer(this,
                        new BTNode[] { new BTRandomWalk(this),
                                        new BTRandomWalk(this),
                                        new BTFailNode(this)
                                         }));
        }
        else
        {
            mRoot = new BTRepeatUntilFailureNode(this,
                    new BTSelectorNode(this,
                        new BTNode[] { new BTRandomWalk(this),
                                        new BTFailNode(this),
                                        new BTRandomWalk(this),
                                        new BTRandomWalk(this),
                                        new BTFailNode(this),
                                         }));
        }
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
        BTNode.Result result = Root.Execute();

        while(result == BTNode.Result.Running)
        {
            //Debug.Log("Root result: " + result);
            yield return null;
            result = Root.Execute();
        }

        Debug.Log("Behaviour has finished with: " + result);
    }
}
