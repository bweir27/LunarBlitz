using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    public static GameObject mushroomMob;
    public static GameObject flyingEyeMob;
    public static GameObject goblinMob;
    public static GameObject banditMob;

    public GameObject mushroomPrefab;
    public GameObject flyingEyePrefab;
    public GameObject goblinPrefab;
    public GameObject banditPrefab;

    public static List<GameObject> enemies = new List<GameObject>();

    public void Awake()
    {
        mushroomMob = mushroomPrefab;
        flyingEyeMob = flyingEyePrefab;
        goblinMob = goblinPrefab;
        banditMob = banditPrefab;
    }

    public void Start()
    {
        mushroomMob = mushroomPrefab;
        flyingEyeMob = flyingEyePrefab;
        goblinMob = goblinPrefab;
        banditMob = banditPrefab;
    }
}
