using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Towers : MonoBehaviour
{

    public static GameObject basicTower;
    public static GameObject shipTower;
    public static GameObject rangeDisplay;

    public GameObject basicTowerPrefab;
    public GameObject shipTowerPrefab;
    public GameObject rangeDisplayPrefab;

    public List<GameObject> towerPrefabOptions = new List<GameObject>();
    public static List<GameObject> towerOptions = new List<GameObject>();
    public static List<GameObject> activeTowers = new List<GameObject>();


    public void Awake()
    {
        basicTower = basicTowerPrefab;
        shipTower = shipTowerPrefab;

        towerOptions = towerPrefabOptions;
    }

    public void Start()
    {
        basicTower = basicTowerPrefab;
        shipTower = shipTowerPrefab;
    }

    public static void shutdownTowers()
    {
        //Debug.Log("Shutting down towers...");
        activeTowers.ForEach(t =>
        {
            if(t != null)
            {
                Tower tower = t.GetComponent<Tower>();
                if (tower != null)
                {
                    //Debug.Log("Shutting down " + tower.name + "...");
                    tower.shutdownTower();
                }
            }
        });
    }

    public static List<GameObject> GetTowerOptions()
    {
        return towerOptions;
    }
}
