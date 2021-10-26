using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private UIManager uiManager;
    private int _currGold;
    public int startGold;

    public int numLives;
    private int _livesRemaining;

    // Start is called before the first frame update
    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();

        _currGold = startGold;
        _livesRemaining = numLives;
        uiManager.updateGoldRemainingText(_currGold);
        uiManager.updateLivesRemainingText(_livesRemaining);
    }

    public int GetCurrentGold()
    {
        return _currGold;
    }

    public void AddMoney(int amt)
    {
        
        _currGold += amt;
        //Debug.Log(amt + " Gold added! Has: " + _currGold);
        UpdateGoldUI();
    }

    public void RemoveMoney(int amt)
    {
        _currGold -= amt;
        //Debug.Log("Removed " + amt + " Gold, has: " + GetCurrentGold());
        UpdateGoldUI();
    }

    public void UpdateGoldUI()
    {
        uiManager.updateGoldRemainingText(_currGold);
    }


    public void loseLives(int numLivesLost)
    {
        numLives = Mathf.Max(numLives - numLivesLost, 0);
        
        _livesRemaining = numLives;

        uiManager.updateLivesRemainingText(_livesRemaining);
    }

    public void UpdateLivesUI()
    {
        uiManager.updateLivesRemainingText(_livesRemaining);
    }

    // Update is called once per frame
    void Update()
    {
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameController");
        GameManager sm = gameManager.GetComponent<GameManager>();
    }
}
