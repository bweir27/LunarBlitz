using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
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
        Debug.Log("xMin: " + xPos);
        foreach (Tower t in towerOptions) {
            //create option from prefab
            GameObject towerOptionBtn = Instantiate(towerShopUIPrefab);
            towerOptionBtn.transform.parent = towerShopContainer.transform;

            //position it within the parent
            Vector3 currPos = towerOptionBtn.transform.position;
            xPos += spaceBetweenMenuOptions + prefabWidth;

            Vector3 setPos = new Vector3(xPos, yPos, currPos.z);
            Debug.Log("Positioning " + t.name + " at: " + setPos);
            towerOptionBtn.transform.localPosition = setPos;

            //get the children elements
            GameObject towerPreview = towerOptionBtn.transform.GetChild(0).gameObject;
            GameObject towerName = towerOptionBtn.transform.GetChild(1).gameObject;
            GameObject towerCost = towerOptionBtn.transform.GetChild(2).gameObject;
            GameObject towerBuyBtn = towerOptionBtn.transform.GetChild(3).gameObject;

            //set the alues of the children elements depending on the tower
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
        Debug.Log(btn.name + " says: " + btnText.text);


        if (btnText.text.Equals("Buy"))
        {
            btnText.text = "Cancel";
        }
        else
        {
            btnText.text = "Buy";
        }
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
