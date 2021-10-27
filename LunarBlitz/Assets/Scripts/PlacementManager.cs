using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public ShopManager shopManager;

    public Camera cam;
    [SerializeField] private GameObject defaultTower;
    private GameObject towerToBePlaced;
    private int numTowersPlaced;

    private GameObject dummyPlacement;
    public GameObject currMapTile;
    private GameObject hoverTile;
    public LayerMask mask;
    public LayerMask towerMask;
    public bool isBuilding;

    // Start is called before the first frame update
    void Start()
    {
        numTowersPlaced = 0;
    }

    public void setTowerToBePlaced(GameObject tower)
    {
        towerToBePlaced = tower;
    }
    public GameObject getTowerToBePlaced()
    {
        return towerToBePlaced;
    }

    public Vector2 GetMousePosition()
    {
        return cam.ScreenToWorldPoint(Input.mousePosition);
    }

    public void GetCurrentHoverTile()
    {
        Vector2 mousePos = GetMousePosition();

        RaycastHit2D hit = Physics2D.Raycast(mousePos, new Vector2(0, 0), 0.1f, mask, -100, 100);

        if (hit.collider != null)
        {
            bool isMapTile = MapGenerator.mapTiles.Contains(hit.collider.gameObject);
            bool isPathTile = MapGenerator.pathTiles.Contains(hit.collider.gameObject);
            // check if mapTile is valid (exists and NOT a path tile)
            if (isMapTile)
            {
                if (!isPathTile)
                {
                    hoverTile = hit.collider.gameObject;
                }
            }
        }

    }

    public void toggleBuilding(GameObject towerToBuild)
    {
        // if currently building, and Buy btn is clicked again, cancel the build
        if (isBuilding)
        {
            Debug.Log("Canceling Build...");
            towerToBePlaced = null;
            Destroy(dummyPlacement);
            isBuilding = false;
        } else
        {
            StartBuilding(towerToBuild);
        }
    }

    public void StartBuilding(GameObject towerToBuild)
    {
        isBuilding = true;

        towerToBePlaced = towerToBuild;

        dummyPlacement = Instantiate(towerToBePlaced);

        // prevent dummy tower from actually being able to fire at mobs
        if (dummyPlacement.GetComponent<Tower>() != null)
        {
            Destroy(dummyPlacement.GetComponent<Tower>());
        }

        if (dummyPlacement.GetComponent<BarrelRotation>() != null)
        {
            Destroy(dummyPlacement.GetComponent<BarrelRotation>());
        }
    }

    // Disallow tower collision
    public bool CheckForTower()
    {
        bool towerOnSlot = false;

        Vector2 mousPos = GetMousePosition();
        RaycastHit2D hit = Physics2D.Raycast(mousPos, new Vector2(0, 0), 0.1f, towerMask, -100, 100);

        if (hit.collider != null)
        {
            towerOnSlot = true;
        }

        return towerOnSlot;
    }

    // Actually place the Tower
    public void PlaceTower()
    {
        if (hoverTile != null)
        {
            if (!CheckForTower())
            {
                // ensure player can afford the tower they are trying to place
                if (shopManager.CanBuyTower(towerToBePlaced))
                {
                    GameObject newTowerObj = Instantiate(towerToBePlaced);
                    newTowerObj.layer = LayerMask.NameToLayer("Tower");
                    newTowerObj.name = towerToBePlaced.name + "" + numTowersPlaced+ "";
                    numTowersPlaced++;
                    newTowerObj.transform.position = hoverTile.transform.position;

                    BoxCollider2D tileCollider = hoverTile.GetComponent<BoxCollider2D>();
                    Destroy(tileCollider);

                    EndBuilding();
                    shopManager.BuyTower(towerToBePlaced);
                }
                else
                {
                    Debug.Log("Not enough money!");
                }
            }
        }
    }

    public void EndBuilding()
    {
        isBuilding = false;

        if(dummyPlacement != null)
        {
            Destroy(dummyPlacement);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isBuilding)
        {
            if (dummyPlacement != null)
            {
                GetCurrentHoverTile();
                // snap to tile grid
                if (hoverTile != null)
                {
                    dummyPlacement.transform.position = hoverTile.transform.position;
                }
            }
            if (Input.GetButtonDown("Fire1"))
            {
                PlaceTower();
            }
        }
    }
}
