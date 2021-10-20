using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundController : MonoBehaviour
{
    public GameObject mushroomMob;
    public GameObject flyingEyeMob;

    public float timeBetweenWaves;
    public float timeBeforeRoundStarts;
    public float timeVariable;

    public bool isRoundGoing;
    public bool isIntermission;
    public bool isStartOfRound;

    // Start is called before the first frame update
    void Start()
    {
        isRoundGoing = false;
        isIntermission = false;
        isStartOfRound = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Basic state-machine
        if (isStartOfRound)
        {

        }
        else if (isIntermission)
        {

        }
        else if (isRoundGoing)
        {

        }
    }
}
