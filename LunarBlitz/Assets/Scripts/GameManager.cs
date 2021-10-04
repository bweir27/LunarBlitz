using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int roundNum = 0;
    public int waveNum = 0;
    public int[] numMobsPerRound = { 5, 10, 15, 20 };
    public int mobsSentThisRound = 0;

    public int[] startGold = { 300, 500, 1200, 1800 };

    public GameObject basicEnemy;
    [SerializeField] private GameObject defaultTower;
    [SerializeField] private Camera MainCamera;
    public float timeBetweenWaves;
    public float timeBeforeRoundStarts;
    public float timeBetweenMobSpawns;
    private float timeUntilNextMobSpawn;
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
            // Temporary until GUI is set up
            float startTrigger = Input.GetAxis("Jump");
            if(Mathf.Abs(startTrigger) > 0.002f)
            {
                Debug.Log("startTrigger: " + startTrigger);
                isStartOfRound = false;
                isRoundGoing = true;
            }
            
            //PlayerController.gold = startGold[roundNum];
        }
        else if (isIntermission)
        {

        }
        else if (isRoundGoing)
        {
            Vector3 startPos = new Vector3(
                MapGenerator.startTile.transform.position.x,
                MapGenerator.startTile.transform.position.y,
                MapGenerator.startTile.transform.position.z);
            if (mobsSentThisRound < numMobsPerRound[roundNum])
            {
                GameObject mobToSpawn = basicEnemy;

                if(mobToSpawn == null)
                {

                }

                if (Time.time >= timeUntilNextMobSpawn)
                {
                    GameObject mob = Instantiate(mobToSpawn);
                    mob.transform.position = startPos;
                    mobsSentThisRound += 1;
                    Debug.Log("Mob Created!");
                    timeUntilNextMobSpawn = Time.time + timeBetweenMobSpawns;
                   
                }
            }
            
            // TODO: Temporary until GUI for tower placement/purchase/upgrade
            //float placeTower = Input.GetAxis("Fire1");
            if (Input.GetButtonDown("Fire1"))
            {
                if(MainCamera == null)
                {
                    Debug.LogError("No camera detected!");
                }
                else
                {
                    Vector3 mousePos = Input.mousePosition;
                    Debug.Log(mousePos.x);
                    Debug.Log(mousePos.y);
                    Vector2 worldPoint = MainCamera.ScreenToWorldPoint(new Vector2(mousePos.x, mousePos.y));
                    Debug.Log("Click Detected!");

                    // TODO: have towers cost money
                    if (defaultTower != null)
                    {
                        GameObject tower = Instantiate(defaultTower);
                        tower.transform.position = worldPoint;
                        Debug.Log("Tower Created!");
                    }
                }


                
                
            }
        }
    }
}
