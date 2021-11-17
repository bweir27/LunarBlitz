using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private int _currGold;
    [SerializeField] private int startGold;

    public int LivesAllowed;
    private int _livesRemaining;
    private LevelManager levelManager;

    // Start is called before the first frame update
    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();

        if (initLevelManager())
        {
            if(startGold != levelManager.startGold && levelManager.startGold > 0)
            {
                SetStartGold(levelManager.startGold);
            }
            if (LivesAllowed != levelManager.startLives && levelManager.startLives > 0)
            {
                SetLives(levelManager.startLives);
            }
        }

        if (uiManager)
        {
            uiManager.updateGoldRemainingText(_currGold);
            uiManager.updateLivesRemainingText(_livesRemaining);
        }
    }

    private void Update()
    {
        if (uiManager == null)
        {
            uiManager = FindObjectOfType<UIManager>();
        }
        if(levelManager == null)
        {
            initLevelManager();
        }
        else
        {
            UpdateUI();
        }
    }

    private bool initLevelManager()
    {
        levelManager = FindObjectOfType<LevelManager>();
        if(levelManager != null)
        {
            SetStartGold(levelManager.startGold);
            SetLives(levelManager.startLives);
        }
        
        return levelManager != null;
    }
    public void SetStartGold(int amt)
    {
        startGold = amt;
        _currGold = amt;
    }

    public int GetCurrentGold()
    {
        return _currGold;
    }

    public void AddMoney(int amt)
    {
        _currGold += amt;
        if (uiManager)
        {
            UpdateGoldUI();
        }
    }

    public void RemoveMoney(int amt)
    {
        _currGold -= amt;

        if (uiManager)
        {
            UpdateGoldUI();
        }
    }

    public void SetLives(int lives)
    {
        LivesAllowed = lives;
        _livesRemaining = lives;

        if (uiManager)
        {
            UpdateLivesUI();
        }
    }
    public void loseLives(int numLivesLost)
    {
        LivesAllowed = Mathf.Max(LivesAllowed - numLivesLost, 0);
        
        _livesRemaining = LivesAllowed;

        if (uiManager)
        {
            UpdateLivesUI();
        }
    }

    public int GetNumLivesRemaining()
    {
        return _livesRemaining;
    }

    public void UpdateUI()
    {
        if(uiManager != null)
        {
            UpdateGoldUI();
            UpdateLivesUI();
        }
    }

    public void UpdateGoldUI()
    {
        uiManager.updateGoldRemainingText(_currGold);
    }

    public void UpdateLivesUI()
    {
        uiManager.updateLivesRemainingText(_livesRemaining);
    }
}
