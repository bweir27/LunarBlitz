using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Canvas canvas;
    public PlacementManager placementManager;
    private ShopManager shopManager;
    public GameObject towerShopContainer;
    public GameObject towerShopUIPrefab;
    public Tower[] towerOptions;
    private List<GameObject> towerOptionUIList;
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
    private GameObject gameEndDisplay;
    [SerializeField] GameObject GameWinDisplay;
    [SerializeField] Text GameWinLivesRemainingText;
    [SerializeField] GameObject GameLoseDisplay;
    private RectTransform gameEndDisplayRect;

    // Start is called before the first frame update
    void Start()
    {
        if(spaceBetweenMenuOptions <= 0.5f)
        {
            spaceBetweenMenuOptions = 0.5f;
        }
        towerOptionUIList = initTowerMenu();
        shopManager = FindObjectOfType<ShopManager>();
    }

    private List<GameObject> initTowerMenu()
    {
        List<GameObject> towerOptionList = new List<GameObject>();
        RectTransform containerRect = towerShopContainer.GetComponent<RectTransform>();
        RectTransform towerPrefabRect = towerShopUIPrefab.GetComponent<RectTransform>();

        float prefabWidth = towerPrefabRect.rect.width;
        float xPos = containerRect.rect.xMin + (prefabWidth / 2.0f) - prefabWidth; //containerRect.position.x;
        float yPos = 0;

        towerOptions = towerOptions.OrderBy(t => t.cost).ToArray();
        foreach (Tower t in towerOptions) {
            //create option from prefab
            GameObject towerOptionBtn = Instantiate(towerShopUIPrefab);

            towerOptionBtn.transform.SetParent(towerShopContainer.transform, false);

            //position it within the parent
            Vector3 currPos = towerOptionBtn.transform.position;
            xPos += spaceBetweenMenuOptions + prefabWidth;

            Vector3 setPos = new Vector3(xPos, yPos, currPos.z);
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

            towerOptionList.Add(towerOptionBtn);
        }
        return towerOptionList;
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
        //Debug.Log("Buy Btn Clicked");
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

    public void updateLivesRemainingText(int livesRemaining, int startLives)
    {
        _livesRemaining = livesRemaining;
        LivesRemainingText.text = "Lives: " + _livesRemaining.ToString() + "/" + startLives.ToString();
    }

    public void updateGoldRemainingText(int goldAmt)
    {
        //Debug.Log("updateGoldRemainingText: " + goldAmt);
        _gold = goldAmt;
        GoldRemainingText.text = _gold.ToString();
        if(towerOptionUIList != null)
        {
            updateTowerBuyBtns();
        }
        
    }

    public void updateRoundNum(int currRound, int numRounds)
    {
        _roundNum = currRound;
        RoundNumberText.text = "Round: " + _roundNum.ToString() + "/" + numRounds.ToString();
    }

    public void updateTowerBuyBtns()
    {
        // disallow purchasing of towers that are out of price-range
        if(towerOptionUIList != null && towerOptionUIList.Count > 0)
        {
            foreach (GameObject t in towerOptionUIList)
            {
                if (t != null)
                {
                    GameObject towerCost = t.transform.GetChild(2).gameObject;
                    GameObject towerBuyBtn = t.transform.GetChild(3).gameObject;

                    // remove the $ from the tower cost, then parse to an int
                    int parsedCost = Convert.ToInt32(towerCost.GetComponent<Text>().text.Substring(1));

                    // disable the buy button depending on whether or not player can afford it
                    Button buyBtn = towerBuyBtn.GetComponent<Button>();
                    bool canAffordTower = shopManager.PlayerCanAfford(parsedCost);

                    // since the towers are sorted by cost,
                    //      we can assume that if we weren't able to afford this tower last time,
                    //      and we still can't, then we can assume the same holds true for the rest of them
                    if(!canAffordTower && !buyBtn.interactable)
                    {
                        break;
                    }
                    buyBtn.interactable = canAffordTower;
                }
            }
        }
    }

    private void setupGameEndDisplay()
    {
        // disable clicking on game content by making the GameEndDisplay a raycast target
        gameEndDisplay = GameObject.FindGameObjectWithTag("GameEndDisplay");
        if (gameEndDisplay != null)
        {
            Image gameEndImg = gameEndDisplay.GetComponent<Image>();
            if (gameEndImg != null)
            {
                gameEndImg.raycastTarget = true;
            }
        }
    }

    public void displayGameWin()
    {
        setupGameEndDisplay();

        gameEndDisplayRect = gameEndDisplay.GetComponent<RectTransform>();
        RectTransform displayPanelRect = GameWinDisplay.GetComponent<RectTransform>();

        float prefabWidth = displayPanelRect.rect.width;
        float prefabHeight = displayPanelRect.rect.height;

        Vector3 gameEndDisplayCenter = gameEndDisplayRect.rect.center;
        float xPos = gameEndDisplayRect.rect.center.x - (prefabWidth / 2.0f);
        float yPos = gameEndDisplayRect.rect.center.y + (prefabHeight / 2.0f);

        GameObject gameWinDisplay = Instantiate(GameWinDisplay);
        GameObject endLivesReaminingObj = FindGameObjectInChildWithTag(gameWinDisplay, "EndLivesDisplay");

        // update the text for the number of lives left remaining
        if(endLivesReaminingObj != null)
        {
            GameWinLivesRemainingText = endLivesReaminingObj.GetComponent<Text>();
            GameWinLivesRemainingText.text = "Lives Remaining: " + _livesRemaining.ToString();
            gameWinDisplay.transform.SetParent(gameEndDisplay.transform, false);

            Vector3 currPos = gameWinDisplay.transform.position;

            Vector3 setPos = new Vector3(xPos, yPos, currPos.z);
            gameWinDisplay.transform.localPosition = gameEndDisplayCenter;
            RectTransform gameWinDisplayRect = gameWinDisplay.GetComponent<RectTransform>();

            gameWinDisplayRect.offsetMin = new Vector2(0, 0);
            gameWinDisplayRect.offsetMax = new Vector2(0, 0);

            gameWinDisplayRect.anchoredPosition = new Vector2(0, 0);
        }
        else
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
                nextLevelBtn.onClick.RemoveAllListeners();
                nextLevelBtn.onClick.AddListener(clickNextLevelBtn);

                // if there is a next level
                if (nextLevelBuildIndex < SceneManager.sceneCountInBuildSettings - 1)
                {
                    //nextLevelBtn.onClick.AddListener(clickNextLevelBtn);

                    // show next level button
                    Debug.Log("Showing Next level Btn");
                    Text nextLvlBtnText = nextLevelBtn.GetComponentInChildren<Text>();
                    nextLvlBtnText.text = "Next Level";
                    CanvasGroup canvasGroup = nextLevelBtnObj.GetComponent<CanvasGroup>();
                    canvasGroup.alpha = 1f; // make visible
                    canvasGroup.blocksRaycasts = true; // allow to receive input
                }
                else
                {
                    Debug.Log("No next level");

                    // hide next level button
                    Debug.Log("Hiding Next level Btn");
                    Text nextLvlBtnText = nextLevelBtn.GetComponentInChildren<Text>();
                    nextLvlBtnText.text = "Credits";
                    CanvasGroup canvasGroup = nextLevelBtnObj.GetComponent<CanvasGroup>();
                    canvasGroup.alpha = 1f; // make visible
                    canvasGroup.blocksRaycasts = true; // allow to receive input
                    //canvasGroup.alpha = 0f; // make transparent
                    //canvasGroup.blocksRaycasts = false; // prevent from receiving input
                }
            }
            else
            {
                Debug.Log("No Next level!");
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

        // Save btn
        GameObject saveGameBtnObj = FindGameObjectInChildWithTag(GameWinDisplay, "SaveGameBtn");
        if(saveGameBtnObj != null)
        {
            Button saveGameBtn = saveGameBtnObj.GetComponent<Button>();
            if (saveGameBtn != null)
            {
                saveGameBtn.onClick.AddListener(clickSaveGameBtn);
            }
            else
            {
                Debug.LogError("MainMenuBtn not found!");
            }
        }
    }

    public void displayGameLose()
    {
        setupGameEndDisplay();

        gameEndDisplayRect = gameEndDisplay.GetComponent<RectTransform>();
        RectTransform displayPanelRect = GameLoseDisplay.GetComponent<RectTransform>();

        float prefabWidth = displayPanelRect.rect.width;
        float prefabHeight = displayPanelRect.rect.height;

        Vector3 gameEndDisplayCenter = gameEndDisplayRect.rect.center;
        float xPos = gameEndDisplayRect.rect.center.x - (prefabWidth / 2.0f); //containerRect.position.x;
        float yPos = gameEndDisplayRect.rect.center.y + (prefabHeight / 2.0f);

        GameObject gameLoseDisplay = Instantiate(GameLoseDisplay);
        gameLoseDisplay.transform.SetParent(gameEndDisplayRect, false);
        Vector3 currPos = gameLoseDisplay.transform.position;

        RectTransform gameLoseDisplayRect = gameLoseDisplay.GetComponent<RectTransform>();

        gameLoseDisplayRect.offsetMin = new Vector2(0, 0);
        gameLoseDisplayRect.offsetMax = new Vector2(0, 0);

        gameLoseDisplayRect.anchoredPosition = new Vector2(0, 0);

        Image gameLoseImg = gameLoseDisplay.GetComponent<Image>();
        if (gameLoseImg != null)
        {
            gameLoseImg.raycastTarget = true;
            //Debug.Log("GameLoseImg.Color: " + gameLoseImg.color);
        }

        Vector3 setPos = new Vector3(xPos, yPos, currPos.z);
        gameLoseDisplay.transform.localPosition = gameEndDisplayCenter;
        RectTransform gameWinDisplayRect = gameLoseDisplay.GetComponent<RectTransform>();
        gameWinDisplayRect.anchoredPosition = new Vector2(0, 0);

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

    public void clickSaveGameBtn()
    {
        GameModel gameModel = FindObjectOfType<GameModel>();
        if(gameModel != null)
        {
            gameModel.OnSaveClick();
        }
        else
        {
            Debug.LogError("GameModel not found!");
        }
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
