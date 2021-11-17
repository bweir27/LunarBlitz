using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    public int GetTowerCost(GameObject towerPrefab)
    {
        int cost = towerPrefab.GetComponent<Tower>().cost;

        return towerPrefab.GetComponent<Tower>().cost;
    }

    public bool CanBuyTower(GameObject towerPrefab)
    {
        int cost = GetTowerCost(towerPrefab);
        return PlayerCanAfford(cost);
    }

    public bool PlayerCanAfford(int cost)
    {
        return playerController.CanAfford(cost);
    }

    public void BuyTower(GameObject towerPrefab)
    {
        //Debug.Log(towerPrefab.name + " bought!");
        playerController.RemoveMoney(GetTowerCost(towerPrefab));
    }

    // Update is called once per frame
    void Update()
    {
        if(playerController == null)
        {
            playerController = FindObjectOfType<PlayerController>();
        }
    }
}
