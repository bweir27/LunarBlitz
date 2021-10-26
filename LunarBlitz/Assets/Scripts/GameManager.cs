using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int roundNum = 0;
    public int NumberOfRounds = 6;
    public int[] numMobsPerRound = {
        5,
        10,
        15,
        20,
        35,
        40
    };
    //public int[] numMushroomsPerRound = {5, 10, 10, 12, 25, 20};
    public int[] numMushroomsPerRound = new int[7]; //{ 0, 0, 0, 1, 2, 4, 0 };
    //public int[] numFlyingEyesPerRound = { 0, 0, 5, 8, 10, 20 };
    public int[] numFlyingEyesPerRound = new int[7]; //{ 0, 0, 0, 0, 0, 0 };
    public int[] numGoblinsPerRound = new int[7]; //{ 3, 6, 9, 4, 0, 10, 16 };

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

    private float minTimeBetweenGoblinSpawns = 0.4f;
    private float minTimeBetweenMushroomSpawns = 0.65f;

    // Start is called before the first frame update
    void Start()
    {
        isRoundGoing = false;
        isIntermission = false;
        isStartOfRound = true;

        timeVariable = Time.time + timeBeforeRoundStarts;
        roundNum = 1;
        uiManager.updateRoundNum(roundNum, NumberOfRounds);
    }

    void SpawnEnemies()
    {
        Debug.Log("Spawning Enemies for Round: " + roundNum + "...");
        if (numMushroomsPerRound.Length > roundNum)
        {
            StartCoroutine("ISpawnMushrooms");
        }
        if (numFlyingEyesPerRound.Length > roundNum)
        {
            StartCoroutine("ISpawnFlyingEyes");
        }
        if (numGoblinsPerRound.Length > roundNum)
        {
            StartCoroutine("ISpawnGoblins");
        }
    }

    IEnumerator ISpawnMushrooms()
    {
        for (int i = 0; i < numMushroomsPerRound[roundNum]; i++)
        {
            //spawn enemy
            GameObject mob = Instantiate(mushroomMob, MapGenerator.startTile.transform.position, Quaternion.identity);
            float _mushroomWaitTime = timeBetweenMobSpawns / roundNum;
            float _decidedWaitTime = Mathf.Max(_mushroomWaitTime, minTimeBetweenMushroomSpawns);
            Debug.Log("mushroomWaitTime: " + _decidedWaitTime);
            yield return new WaitForSeconds(_decidedWaitTime);
        }
    }

    IEnumerator ISpawnFlyingEyes()
    {
        for (int i = 0; i < numFlyingEyesPerRound[roundNum]; i++)
        {
            //spawn enemy
            GameObject mob = Instantiate(flyingEyeMob, MapGenerator.startTile.transform.position, Quaternion.identity);
            float _flyingEyeWaitTime = timeBetweenMobSpawns / (2.0f * roundNum);
            yield return new WaitForSeconds(_flyingEyeWaitTime);
        }
    }

    IEnumerator ISpawnGoblins()
    {
        for (int i = 0; i < numGoblinsPerRound[roundNum]; i++)
        {
            //spawn enemy
            GameObject mob = Instantiate(goblinMob, MapGenerator.startTile.transform.position, Quaternion.identity);
            float _goblinWaitTime = timeBetweenMobSpawns / (1.2f * roundNum);
            float _decidedWaitTime = Mathf.Max(_goblinWaitTime, minTimeBetweenGoblinSpawns);
            Debug.Log("goblinWaitTime: " + _decidedWaitTime);
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
                Debug.Log("RoundNum: " + roundNum);
                if(roundNum - 1 <= NumberOfRounds)
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
                if(roundNum > NumberOfRounds)
                {
                    Debug.Log("HERE, end of game");
                    
                } else
                {
                    isIntermission = true;
                    isRoundGoing = false;

                    // reset time
                    timeVariable = Time.time + timeBetweenWaves;

                    //update round number
                    roundNum++;
                    uiManager.updateRoundNum(roundNum - 1, NumberOfRounds);
                    return;
                }
                return;
            }

            //Vector3 startPos = new Vector3(
            //    MapGenerator.startTile.transform.position.x,
            //    MapGenerator.startTile.transform.position.y,
            //    MapGenerator.startTile.transform.position.z);
            //if (mobsSentThisRound < numMobsPerRound[roundNum])
            //{
            //    GameObject mobToSpawn = basicEnemy;

            //    if(mobToSpawn == null)
            //    {

            //    }

            //    if (Time.time >= timeUntilNextMobSpawn)
            //    {
            //        GameObject mob = Instantiate(mobToSpawn);
            //        mob.transform.position = startPos;
            //        mobsSentThisRound += 1;
            //        Debug.Log("Mob Created!");
            //        timeUntilNextMobSpawn = Time.time + timeBetweenMobSpawns;
                   
            //    }
            //}
        }
    }
}
