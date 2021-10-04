using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int gold;
    public int livesLeft;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameController");
        GameManager sm = gameManager.GetComponent<GameManager>();
    }
}
