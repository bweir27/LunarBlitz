using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private int _livesRemaining;
    private int _gold;
    private int _roundNum;

    //public UI
    [SerializeField] Text LivesRemainingText;
    [SerializeField] Text GoldRemainingText;
    [SerializeField] Text RoundNumberText;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Dynamically generate the UI elements for each tower with name, price & button
    public void initTowerBtns()
    {

    }

    public void InitNumLivesUI(int lives)
    {
        _livesRemaining = lives;
        LivesRemainingText.text = "Lives: " + _livesRemaining.ToString();
    }

    public void updateLivesRemainingText(int lives)
    {
        _livesRemaining = lives;
        LivesRemainingText.text = "Lives: " + _livesRemaining.ToString();
        //Debug.Log("Lives remaining: " + _livesRemaining);

        //TODO: if lives = 0, show "Game Over" Display
    }

    public void updateGoldRemainingText(int goldAmt)
    {
        _gold = goldAmt;
        GoldRemainingText.text = _gold.ToString();
        //Debug.Log("GOLD remaining: " + _gold);
    }

    public void updateRoundNum(int currRound, int numRounds)
    {
        _roundNum = currRound;
        RoundNumberText.text = "Round: " + _roundNum.ToString() + "/" + numRounds.ToString();
        //Debug.Log("Wave updated");
    }
}
