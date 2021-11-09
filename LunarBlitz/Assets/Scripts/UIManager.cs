using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Canvas canvas;
    public PlacementManager placementManager;
    public GameObject towerShopContainer;
    public GameObject towerShopUIPrefab;
    public Tower[] towerOptions;
    [SerializeField] private float spaceBetweenMenuOptions;

    private int _livesRemaining;
    private int _gold;
    private int _roundNum;


    //public UI
    [SerializeField] Text LivesRemainingText;
    [SerializeField] Text GoldRemainingText;
    [SerializeField] Text RoundNumberText;
    [SerializeField] Text BuyTowerText;
    [SerializeField] Text CancelBuyTowerText;

    // End Game Displays
    [SerializeField] GameObject GameWinDisplay;
    [SerializeField] Text GameWinLivesRemainingText;
    [SerializeField] GameObject GameLoseDisplay;



    // Start is called before the first frame update
    void Start()
    {
        if(spaceBetweenMenuOptions <= 0.5f)
        {
            spaceBetweenMenuOptions = 0.5f;
        }
        initTowerMenu();
    }

    private void initTowerMenu()
    {
        RectTransform containerRect = towerShopContainer.GetComponent<RectTransform>();
        RectTransform towerPrefabRect = towerShopUIPrefab.GetComponent<RectTransform>();

        
        float prefabWidth = towerPrefabRect.rect.width;
        float xPos = containerRect.rect.xMin + (prefabWidth / 2.0f) - prefabWidth; //containerRect.position.x;
        float yPos = 0;
        foreach (Tower t in towerOptions) {
            //create option from prefab
            GameObject towerOptionBtn = Instantiate(towerShopUIPrefab);
            //towerOptionBtn.transform.parent = towerShopContainer.transform;
            towerOptionBtn.transform.SetParent(towerShopContainer.transform, false);

            //position it within the parent
            Vector3 currPos = towerOptionBtn.transform.position;
            xPos += spaceBetweenMenuOptions + prefabWidth;

            Vector3 setPos = new Vector3(xPos, yPos, currPos.z);
            //Debug.Log("Positioning " + t.name + " at: " + setPos);
            towerOptionBtn.transform.localPosition = setPos;

            //get the children elements
            GameObject towerPreview = towerOptionBtn.transform.GetChild(0).gameObject;
            GameObject towerName = towerOptionBtn.transform.GetChild(1).gameObject;
            GameObject towerCost = towerOptionBtn.transform.GetChild(2).gameObject;
            GameObject towerBuyBtn = towerOptionBtn.transform.GetChild(3).gameObject;

            //set the values of the children elements depending on the tower
            Image towerPreviewImg = towerPreview.GetComponent<Image>();
            if(towerPreviewImg == null)
            {
                Debug.LogError("No Tower preview Image!");
            }
            towerPreviewImg.sprite = t.towerPreviewSprite;

            Text towerNameText = towerName.GetComponent<Text>();
            towerNameText.text = t.name;

            Text towerCostText = towerCost.GetComponent<Text>();
            towerCostText.text = "$" + t.cost;

            Button buyBtn = towerBuyBtn.GetComponent<Button>();
            buyBtn.onClick.RemoveAllListeners();
            buyBtn.onClick.AddListener(() => toggleBuyTower(t.gameObject));
            buyBtn.onClick.AddListener(() => toggleBtnText(buyBtn));
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    // Dynamically generate the UI elements for each tower with name, price & button
    public void initTowerBtns()
    {

    }

    public void toggleBuyTower(GameObject tower)
    {
        placementManager.toggleBuilding(tower);
    }

    public void toggleBtnText(Button btn)
    {
        Text btnText = btn.GetComponentInChildren<Text>();
        //Debug.Log(btn.name + " says: " + btnText.text);

        // TODO: allow cancel purchase
        //if (btnText.text.Equals("Buy"))
        //{
        //    btnText.text = "Cancel";
        //}
        //else
        //{
        //    btnText.text = "Buy";
        //}
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
    }

    public void updateGoldRemainingText(int goldAmt)
    {
        _gold = goldAmt;
        GoldRemainingText.text = _gold.ToString();
    }

    public void updateRoundNum(int currRound, int numRounds)
    {
        _roundNum = currRound;
        RoundNumberText.text = "Round: " + _roundNum.ToString() + "/" + numRounds.ToString();
    }

    public void displayGameWin()
    {
        // disable clicking on game content by making the GameEndDisplay a raycast target
        GameObject gameEndDisplay = GameObject.FindGameObjectWithTag("GameEndDisplay");
        if(gameEndDisplay != null)
        {
            Image gameEndImg = gameEndDisplay.GetComponent<Image>();
            if(gameEndImg != null)
            {
                gameEndImg.raycastTarget = true;
            }
        }


        //RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        RectTransform gameEndDisplayRect = gameEndDisplay.GetComponent<RectTransform>();
        RectTransform displayPanelRect = GameWinDisplay.GetComponent<RectTransform>();

        float prefabWidth = displayPanelRect.rect.width;
        float prefabHeight = displayPanelRect.rect.height;
        
        Vector3 gameEndDisplayCenter = gameEndDisplayRect.rect.center;
        float xPos = gameEndDisplayRect.rect.center.x - (prefabWidth / 2.0f); //containerRect.position.x;
        float yPos = gameEndDisplayRect.rect.center.y + (prefabHeight / 2.0f);

        GameObject gameWinDisplay = Instantiate(GameWinDisplay);
        GameObject endLivesReaminingObj = FindGameObjectInChildWithTag(gameWinDisplay, "EndLivesDisplay");

        // update the text for the number of lives left remaining
        if(endLivesReaminingObj != null)
        {
            GameWinLivesRemainingText = endLivesReaminingObj.GetComponent<Text>();
            GameWinLivesRemainingText.text = "Lives Remaining: " + _livesRemaining.ToString();
            gameWinDisplay.transform.SetParent(gameEndDisplayRect, false);

            Vector3 currPos = gameWinDisplay.transform.position;

            Vector3 setPos = new Vector3(xPos, yPos, currPos.z);
            gameWinDisplay.transform.localPosition = gameEndDisplayCenter;

        } else
        {
            Debug.LogError("Text not found!");
        }

        // make the buttons work
        SceneLoader sceneLoader = GameObject.FindObjectOfType<SceneLoader>();
        if(sceneLoader == null)
        {
            Debug.LogError("No SceneLoader found!");
        }

        // Next level Button
        GameObject nextLevelBtnObj = FindGameObjectInChildWithTag(GameWinDisplay, "NextLevelBtn");
        
        if(nextLevelBtnObj != null)
        {
            Transform nextLvlBtnPos = nextLevelBtnObj.transform;
            Button nextLevelBtn = nextLevelBtnObj.GetComponent<Button>();

            // if no next level, remove the next Level Btn
            Scene currScene = SceneManager.GetActiveScene();
            int nextLevelBuildIndex = currScene.buildIndex + 1;
            
            if (nextLevelBtn != null)
            {
                if(nextLevelBuildIndex < SceneManager.sceneCountInBuildSettings)
                {
                    nextLevelBtn.onClick.AddListener(clickNextLevelBtn);
                    //nextLevelBtn.onClick.AddListener(sceneLoader.startLoadNextLevelTransition);
                    // show next level button
                    CanvasGroup canvasGroup = nextLevelBtnObj.GetComponent<CanvasGroup>();
                    canvasGroup.alpha = 1f; // make visible
                    canvasGroup.blocksRaycasts = true; // allow to receive input
                }
                else
                {
                    // hide next level button
                    CanvasGroup canvasGroup = nextLevelBtnObj.GetComponent<CanvasGroup>();
                    canvasGroup.alpha = 0f; // make transparent
                    canvasGroup.blocksRaycasts = false; // prevent from receiving input
                }
            }
            else
            {
                //Debug.Log("No Next level!");
            }
        }
        else
        {
            Debug.LogError("NextLevelBtn not found!");
        }

        // Main Menu Btn
        GameObject mainMenuBtnObj = FindGameObjectInChildWithTag(GameWinDisplay, "MainMenuBtn");
        if (mainMenuBtnObj != null)
        {
            Button mainMenuBtn = mainMenuBtnObj.GetComponent<Button>();
            if (mainMenuBtn != null)
            {
                mainMenuBtn.onClick.AddListener(clickMainMenuBtn);
            }
            else
            {
                Debug.LogError("MainMenuBtn not found!");
            }
        }
        else
        {
            Debug.LogError("MainMenu not found!");
        }

        
    }

    public void displayGameLose()
    {
        // disable clicking on game content by making the GameEndDisplay a raycast target
        GameObject gameEndDisplay = GameObject.FindGameObjectWithTag("GameEndDisplay");
        if (gameEndDisplay != null)
        {
            Image gameEndImg = gameEndDisplay.GetComponent<Image>();
            if (gameEndImg != null)
            {
                gameEndImg.raycastTarget = true;
            }
        } else
        {
            Debug.LogError("GameEndDisplay not found!");
        }

        RectTransform gameEndDisplayRect = gameEndDisplay.GetComponent<RectTransform>();
        RectTransform displayPanelRect = GameLoseDisplay.GetComponent<RectTransform>();

        float prefabWidth = displayPanelRect.rect.width;
        float prefabHeight = displayPanelRect.rect.height;

        Vector3 gameEndDisplayCenter = gameEndDisplayRect.rect.center;
        float xPos = gameEndDisplayRect.rect.center.x - (prefabWidth / 2.0f); //containerRect.position.x;
        float yPos = gameEndDisplayRect.rect.center.y + (prefabHeight / 2.0f);

        GameObject gameLoseDisplay = Instantiate(GameLoseDisplay);
        gameLoseDisplay.transform.SetParent(gameEndDisplayRect, false);
        Vector3 currPos = gameLoseDisplay.transform.position;

        
        Image gameLoseImg = gameLoseDisplay.GetComponent<Image>();
        if (gameLoseImg != null)
        {
            gameLoseImg.raycastTarget = true;
            Debug.Log(gameLoseImg.color);
            //gameLoseImg.color.a = 1f;
        }

        Vector3 setPos = new Vector3(xPos, yPos, currPos.z);
        gameLoseDisplay.transform.localPosition = gameEndDisplayCenter;

        // make the buttons work
        SceneLoader sceneLoader = GameObject.FindObjectOfType<SceneLoader>();
        if (sceneLoader == null)
        {
            Debug.LogError("No SceneLoader found!");
        }

        // Next level Button
        GameObject replayLevelBtnObj = FindGameObjectInChildWithTag(GameLoseDisplay, "EndPlayAgainBtn");

        if (replayLevelBtnObj != null)
        {
            Transform replayLvlBtnPos = replayLevelBtnObj.transform;
            Button replayLevelBtn = replayLevelBtnObj.GetComponent<Button>();
            
            if (replayLevelBtn != null)
            {
                replayLevelBtn.onClick.AddListener(clickReplayLevelBtn);
                // show replay level button
                CanvasGroup canvasGroup = replayLevelBtnObj.GetComponent<CanvasGroup>();
                canvasGroup.alpha = 1f; // make visible
                canvasGroup.blocksRaycasts = true; // allow to receive input
            }
            else
            {
                Debug.LogError("ReplayLevelBtn not found!");
            }
        }
        else
        {
            Debug.LogError("ReplayLevelBtn not found!");
        }

        // Main Menu Btn
        GameObject mainMenuBtnObj = FindGameObjectInChildWithTag(GameLoseDisplay, "MainMenuBtn");
        if (mainMenuBtnObj != null)
        {
            Button mainMenuBtn = mainMenuBtnObj.GetComponent<Button>();
            if (mainMenuBtn != null)
            {
                mainMenuBtn.onClick.AddListener(clickMainMenuBtn);
            }
            else
            {
                Debug.LogError("MainMenuBtn not found!");
            }
        }
        else
        {
            Debug.LogError("MainMenu not found!");
        }
    }

    public void clickMainMenuBtn()
    {
        SceneLoader sceneLoader = GameObject.FindObjectOfType<SceneLoader>();
        if (sceneLoader == null)
        {
            Debug.LogError("No SceneLoader found!");
        }
        sceneLoader.startMainMenuTransition();
    }

    public void clickNextLevelBtn()
    {
        SceneLoader sceneLoader = GameObject.FindObjectOfType<SceneLoader>();
        if (sceneLoader == null)
        {
            Debug.LogError("No SceneLoader found!");
        }
        sceneLoader.startLoadNextLevelTransition();
    }

    public void clickReplayLevelBtn()
    {
        SceneLoader sceneLoader = GameObject.FindObjectOfType<SceneLoader>();
        if (sceneLoader == null)
        {
            Debug.LogError("No SceneLoader found!");
        }
        sceneLoader.startLoadSameLevelTransition();
    }

    // https://answers.unity.com/questions/893966/how-to-find-child-with-tag.html
    public GameObject FindGameObjectInChildWithTag(GameObject parent, string tag)
    {
        Transform t = parent.transform;
        for (int i = 0; i < t.childCount; i++)
        {
            if (t.GetChild(i).gameObject.tag == tag)
            {
                return t.GetChild(i).gameObject;
            }
        }
        return null;
    }


}
