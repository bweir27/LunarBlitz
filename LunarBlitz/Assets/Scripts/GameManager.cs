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
    public int[] numMushroomsPerRound;
    public int[] numFlyingEyesPerRound;
    public int[] numGoblinsPerRound;

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
    public bool EndScreenDisplayed = false;

    private float minTimeBetweenGoblinSpawns = 0.4f;
    private float minTimeBetweenMushroomSpawns = 0.65f;

    // Start is called before the first frame update
    void Start()
    {
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


        // Spawn Mushrooms
        if (numMushroomsPerRound.Length > _currRoundNum)
        {
            StartCoroutine(ISpawnMobs(mushroomMob, numMushroomsPerRound[_currRoundNum], _mushroomWaitTime, minTimeBetweenMushroomSpawns));
        }

        //Spawn Flying Eyes
        if (numFlyingEyesPerRound.Length > _currRoundNum)
        {
            StartCoroutine(ISpawnMobs(flyingEyeMob, numFlyingEyesPerRound[_currRoundNum], _flyingEyeWaitTime, minTimeBetweenGoblinSpawns));
        }

        // Spawn Goblins
        if (numGoblinsPerRound.Length > _currRoundNum)
        {
            StartCoroutine(ISpawnMobs(goblinMob, numGoblinsPerRound[_currRoundNum], _goblinWaitTime, minTimeBetweenGoblinSpawns));
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
                if(_currRoundNum - 1 <= NumberOfRounds)
                {
                    isIntermission = false;
                    isRoundGoing = true;

                    //start round
                    SpawnEnemies();
                }
                else
                {
                    //Debug.Log("End of Game!");
                    isGameOver = true;
                }
            }
        }
        else if (isRoundGoing)
        {
            // if there are still enemies remaining
            if (Enemies.enemies.Count > 0)
            {
                checkGameOver();
            }
            else // end of round
            {
                if(_currRoundNum > NumberOfRounds)
                {
                    isGameOver = true;
                    isRoundGoing = false;
                }
                else
                {
                    isIntermission = true;
                    isRoundGoing = false;

                    // reset time
                    timeVariable = Time.time + timeBetweenWaves;

                    //check if gameover
                    if(_currRoundNum + 1 > NumberOfRounds)
                    {
                        isGameOver = true;
                        isIntermission = false;
                    }
                    else
                    {
                        //update round number
                        _currRoundNum++;
                        uiManager.updateRoundNum(_currRoundNum, NumberOfRounds);
                        return;
                    }
                }
                return;
            }

        }
        else if (isGameOver && !EndScreenDisplayed)
        {
            Debug.Log("GameOver!");
            //TODO: display game over UI
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                PlayerController player = playerObj.GetComponent<PlayerController>();
                EndScreenDisplayed = true;
                if (checkGameLost())
                {
                    Debug.Log("Sorry, you lose!");
                    uiManager.displayGameLose();
                }
                else
                {
                    Debug.Log("Game over, You Win!");
                    uiManager.displayGameWin();
                }
                
            }
            else
            {
                Debug.LogError("Player not found!");
            }
            
            
            return;
        }
    }

    public bool checkGameOver()
    {
        return checkGameLost() || checkGameWon();
    }

    public bool checkGameWon()
    {
        if(_currRoundNum + 1 > NumberOfRounds)
        {
            isGameOver = true;
            return true;
        }
        return false;
    }

    public bool checkGameLost()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            PlayerController player = playerObj.GetComponent<PlayerController>();
            int numLivesRemaining = player.GetNumLivesRemaining();
            if (numLivesRemaining <= 0)
            {
                Debug.Log("Game Lost!");
                isGameOver = true;
                isRoundGoing = false;
                isIntermission = false;
                return true;
            }
        }
        else
        {
            Debug.LogError("Player not found!");
        }
        return false;
    }
}
