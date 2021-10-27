using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int _currRoundNum = 0;
    public int NumberOfRounds = 7;
    public int[] numMobsPerRound = {
        5,
        10,
        15,
        20,
        35,
        40
    };
    //public int[] numMushroomsPerRound = {5, 10, 10, 12, 25, 20};
    public int[] numMushroomsPerRound;// = new int[8]; //{ 0, 0, 0, 1, 2, 4, 0 };
    //public int[] numFlyingEyesPerRound = { 0, 0, 5, 8, 10, 20 };
    public int[] numFlyingEyesPerRound;// = new int[8]; //{ 0, 0, 0, 0, 0, 0 };
    public int[] numGoblinsPerRound;// = new int[8]; //{ 3, 6, 9, 4, 0, 10, 16 };

    public int mobsSentThisRound = 0;


    public GameObject mushroomMob;
    public GameObject flyingEyeMob;
    public GameObject goblinMob;

    public Camera cam;
    [SerializeField] private UIManager uiManager;
  
    [SerializeField] private Camera MainCamera;
    public float timeBetweenWaves;
    public float timeBeforeRoundStarts;
    public float timeBetweenMobSpawns;

    public float timeVariable;

    public bool isRoundGoing;
    public bool isIntermission;
    public bool isStartOfRound;
    public bool isGameOver;

    private float minTimeBetweenGoblinSpawns = 0.4f;
    private float minTimeBetweenMushroomSpawns = 0.65f;

    // Start is called before the first frame update
    void Start()
    {
        //numMushroomsPerRound
        isRoundGoing = false;
        isIntermission = false;
        isStartOfRound = true;
        isGameOver = false;

        timeVariable = Time.time + timeBeforeRoundStarts;
        _currRoundNum = 0;
        uiManager.updateRoundNum(_currRoundNum, NumberOfRounds);
    }

    void SpawnEnemies()
    {
        // config time between spawns of mobs of same type
        float _mushroomWaitTime;
        float _flyingEyeWaitTime;
        float _goblinWaitTime;
        if(_currRoundNum > 0)
        {
            _mushroomWaitTime = timeBetweenMobSpawns / _currRoundNum;
            _flyingEyeWaitTime = timeBetweenMobSpawns / (2.0f * _currRoundNum);
            _goblinWaitTime = timeBetweenMobSpawns / (1.2f * _currRoundNum);
        }
        else // _currRoundNum == 0
        {
            _mushroomWaitTime = timeBetweenMobSpawns;
            _flyingEyeWaitTime = timeBetweenMobSpawns;
            _goblinWaitTime = timeBetweenMobSpawns;
        }

        Debug.Log("Spawning Enemies for Round: " + _currRoundNum + "...");

        // Spawn Mushrooms
        if (numMushroomsPerRound.Length > _currRoundNum)
        {
            Debug.Log("Num Mushrooms: " + numMushroomsPerRound[_currRoundNum]);
            
            //StartCoroutine("ISpawnMushrooms");
            StartCoroutine(ISpawnMobs(mushroomMob, numMushroomsPerRound[_currRoundNum], _mushroomWaitTime, minTimeBetweenMushroomSpawns));
        }

        //Spawn Flying Eyes
        if (numFlyingEyesPerRound.Length > _currRoundNum)
        {
            Debug.Log("Num FlyingEyes: " + numFlyingEyesPerRound[_currRoundNum]);
            //StartCoroutine("ISpawnFlyingEyes");

            StartCoroutine(ISpawnMobs(flyingEyeMob, numFlyingEyesPerRound[_currRoundNum], _flyingEyeWaitTime, minTimeBetweenGoblinSpawns));
        }

        // Spawn Goblins
        if (numGoblinsPerRound.Length > _currRoundNum)
        {
            Debug.Log("Num Goblins: " + numGoblinsPerRound[_currRoundNum]);
            StartCoroutine(ISpawnMobs(goblinMob, numGoblinsPerRound[_currRoundNum], _goblinWaitTime, minTimeBetweenGoblinSpawns));
        }
    }

    IEnumerator ISpawnMushrooms()
    {
        for (int i = 0; i < numMushroomsPerRound[_currRoundNum]; i++)
        {
            //spawn enemy
            GameObject mob = Instantiate(mushroomMob, MapGenerator.startTile.transform.position, Quaternion.identity);
            float _mushroomWaitTime = timeBetweenMobSpawns / _currRoundNum;
            float _decidedWaitTime = Mathf.Max(_mushroomWaitTime, minTimeBetweenMushroomSpawns);
            //Debug.Log("mushroomWaitTime: " + _decidedWaitTime);
            yield return new WaitForSeconds(_decidedWaitTime);
        }
    }

    IEnumerator ISpawnFlyingEyes()
    {
        for (int i = 0; i < numFlyingEyesPerRound[_currRoundNum]; i++)
        {
            //spawn enemy
            GameObject mob = Instantiate(flyingEyeMob, MapGenerator.startTile.transform.position, Quaternion.identity);
            float _flyingEyeWaitTime = timeBetweenMobSpawns / (2.0f * _currRoundNum);
            yield return new WaitForSeconds(_flyingEyeWaitTime);
        }
    }

    IEnumerator ISpawnGoblins()
    {
        for (int i = 0; i < numGoblinsPerRound[_currRoundNum]; i++)
        {
            //spawn enemy
            GameObject mob = Instantiate(goblinMob, MapGenerator.startTile.transform.position, Quaternion.identity);
            //float _goblinWaitTime = timeBetweenMobSpawns / (1.2f * _currRoundNum);
            //if (_currRoundNum == 0)
            //{

            //}
            float _goblinWaitTime = timeBetweenMobSpawns / (1.2f * _currRoundNum);
            float _decidedWaitTime = Mathf.Max(_goblinWaitTime, minTimeBetweenGoblinSpawns);
            //Debug.Log("goblinWaitTime: " + _decidedWaitTime);
            yield return new WaitForSeconds(_decidedWaitTime);
        }
    }

    IEnumerator ISpawnMobs(GameObject mobToSpawn, int numMobsToSpawn, float mobWaitTime, float minTimeBetweenSpawn)
    {
        for (int i = 0; i < numMobsToSpawn; i++)
        {
            //spawn enemy
            GameObject mob = Instantiate(mobToSpawn, MapGenerator.startTile.transform.position, Quaternion.identity);
            float _decidedWaitTime = Mathf.Max(mobWaitTime, minTimeBetweenSpawn);
            yield return new WaitForSeconds(_decidedWaitTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Basic state-machine
        if (isStartOfRound)
        {
            if(Time.time >= timeVariable)
            {
                isStartOfRound = false;
                isRoundGoing = true;

                SpawnEnemies();
                return;
            }
        }
        else if (isIntermission)
        {
            // wait during intermission
            if (Time.time >= timeVariable)
            {
                Debug.Log("RoundNum: " + _currRoundNum);
                if(_currRoundNum - 1 <= NumberOfRounds)
                {
                    isIntermission = false;
                    isRoundGoing = true;

                    //start round
                    SpawnEnemies();
                }
                else
                {
                    Debug.Log("End of Game!");
                }
            }
        }
        else if (isRoundGoing)
        {
            // if there are still enemies remaining
            if (Enemies.enemies.Count > 0)
            {

            }
            else // end of round
            {
                if(_currRoundNum > NumberOfRounds)
                {
                    Debug.Log("HERE, end of game");
                    isGameOver = true;
                    isRoundGoing = false;
                    
                } else
                {
                    isIntermission = true;
                    isRoundGoing = false;

                    // reset time
                    timeVariable = Time.time + timeBetweenWaves;

                    //update round number
                    _currRoundNum++;
                    uiManager.updateRoundNum(_currRoundNum, NumberOfRounds);
                    return;
                }
                return;
            }

        }
        else if (isGameOver)
        {
            return;
        }
    }
}
