using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Towers : MonoBehaviour
{
    public static List<GameObject> towers = new List<GameObject>();

    public static void shutdownTowers()
    {
        Debug.Log("Shutting down towers...");
        towers.ForEach(t =>
        {
            if(t != null)
            {
                Tower tower = t.GetComponent<Tower>();
                if (tower != null)
                {
                    Debug.Log("Shutting down " + tower.name + "...");
                    tower.shutdownTower();
                }
            }
        });
    }
}
