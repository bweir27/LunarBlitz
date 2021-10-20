using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public PlayerController playerController;
    public GameObject basicTowerPrefab;

    public int basicTowerCost;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public int GetTowerCost(GameObject towerPrefab)
    {
        int cost = towerPrefab.GetComponent<Tower>().cost;

        //if(towerPrefab == basicTowerPrefab)
        //{
        //    cost = basicTowerCost;
        //}

        return towerPrefab.GetComponent<Tower>().cost;
    }

    public bool CanBuyTower(GameObject towerPrefab)
    {
        int cost = GetTowerCost(towerPrefab);
        return playerController.GetCurrentGold() >= cost;
    }

    public void BuyTower(GameObject towerPrefab)
    {
        Debug.Log(towerPrefab.name + " bought!");
        playerController.RemoveMoney(GetTowerCost(towerPrefab));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
