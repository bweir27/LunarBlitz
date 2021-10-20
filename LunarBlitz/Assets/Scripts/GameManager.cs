using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int roundNum = 0;
    public int NumberOfRounds = 5;
    public int[] numMobsPerRound = {
        5,
        10,
        15,
        20,
        35,
        40
    };
    public int[] numMushroomsPerRound = {5, 10, 10, 12, 25, 20};
    public int[] numFlyingEyesPerRound = { 0, 0, 5, 8, 10, 20 };

    public int mobsSentThisRound = 0;

    public int[] startGold = { 300, 120, 1200, 1800 };

    public GameObject mushroomMob;
    public GameObject flyingEyeMob;

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
        StartCoroutine("ISpawnMushrooms");
        StartCoroutine("ISpawnFlyingEyes");
    }

    IEnumerator ISpawnMushrooms()
    {
        for (int i = 0; i < numMushroomsPerRound[roundNum]; i++)
        {
            //spawn enemy
            GameObject mob = Instantiate(mushroomMob, MapGenerator.startTile.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(timeBetweenMobSpawns/roundNum);
        }
    }

    IEnumerator ISpawnFlyingEyes()
    {
        for (int i = 0; i < numFlyingEyesPerRound[roundNum]; i++)
        {
            //spawn enemy
            GameObject mob = Instantiate(flyingEyeMob, MapGenerator.startTile.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(timeBetweenMobSpawns/(2*roundNum));
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
            // Temporary until GUI is set up
            //float startTrigger = Input.GetAxis("Jump");
            //if(Mathf.Abs(startTrigger) > 0.002f)
            //{
            //    Debug.Log("startTrigger: " + startTrigger);
            //    isStartOfRound = false;
            //    isRoundGoing = true;
            //}
            
            //PlayerController.startGold = startGold[roundNum];
        }
        else if (isIntermission)
        {
            if (Time.time >= timeVariable)
            {
                isIntermission = false;
                isRoundGoing = true;

                //start round
                SpawnEnemies();

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
                isIntermission = true;
                isRoundGoing = false;

                // reset time
                timeVariable = Time.time + timeBetweenWaves;
                roundNum++;
                uiManager.updateRoundNum(roundNum, NumberOfRounds);
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



            // TODO: Temporary until GUI for tower placement/purchase/upgrade
            //float placeTower = Input.GetAxis("Fire1");
            //if (Input.GetButtonDown("Fire1"))
            //{
            //    if(MainCamera == null)
            //    {
            //        Debug.LogError("No camera detected!");
            //    }
            //    else
            //    {
            //        Vector3 mousePos = Input.mousePosition;
            //        Debug.Log(mousePos.x);
            //        Debug.Log(mousePos.y);
            //        Vector2 worldPoint = MainCamera.ScreenToWorldPoint(new Vector2(mousePos.x, mousePos.y));
            //        Debug.Log("Click Detected!");

            //        // TODO: have towers cost money
            //        if (defaultTower != null)
            //        {
            //            GameObject tower = Instantiate(defaultTower);
            //            tower.transform.position = worldPoint;
            //            Debug.Log("Tower Created!");
            //        }
            //    }

            //}
        }

     
    }
}
