using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UiManager : MonoBehaviour
{
    //public Canvas canvas;
    private Text scoreDisplay;

    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        scoreDisplay = GameObject.FindGameObjectWithTag("scoreDisplay").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        scoreDisplay.text = "Score: " + player.score.ToString();
    }
}
