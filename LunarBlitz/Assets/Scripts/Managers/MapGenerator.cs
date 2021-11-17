using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Made in part by following tutorial: https://www.youtube.com/watch?v=I6T1En5cPq4
public class MapGenerator : MonoBehaviour
{
    public Tilemap Map;
    public GameObject MapTile;
    public Sprite MapTileSprite;
    public Sprite PathTileSprite;

    public Color PathColor;
    public Color StartTileColor;
    public Color EndTileColor;

    [SerializeField] private int mapWidth;
    [SerializeField] private int mapHeight;

    [SerializeField] private float originX = -6.5f;
    [SerializeField] private float originY = -4.5f;

    public static List<GameObject> mapTiles = new List<GameObject>();
    public static List<GameObject> pathTiles = new List<GameObject>();

    public static GameObject startTile;
    public static GameObject endTile;

    private bool reachedX = false;
    private bool reachedY = false;

    private GameObject currentTile;
    private int currentIndex;
    private int nextIndex;

    // Start is called before the first frame update
    void Start()
    {
        generatePlannedMap();
    }

    private List<GameObject> getTopEdgeTiles()
    {
        List<GameObject> edgeTiles = new List<GameObject>();
        for(int i = mapWidth * (mapHeight-1); i < mapWidth * mapHeight; i++)
        {
            edgeTiles.Add(mapTiles[i]);
        }

        return edgeTiles;
    }

    private List<GameObject> getBottomEdgeTiles()
    {
        List<GameObject> edgeTiles = new List<GameObject>();
        for(int i = 0; i < mapWidth; i++)
        {
            edgeTiles.Add(mapTiles[i]);
        }

        return edgeTiles;
    }

    // For Planned Map
    private GameObject PlannedStartTile()
    {
        return mapTiles.FindLast(
            delegate (GameObject obj)
            {
                SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
                if (renderer = null)
                {
                    return false;
                }
                return renderer.sprite == PathTileSprite;
            });
    }
    private List<GameObject> getLeftEdgeTiles()
    {
        List<GameObject> edgeTiles = new List<GameObject>();
        for (int i = mapWidth; i < mapWidth * mapHeight; i++)
        {
            edgeTiles.Add(mapTiles[i]);
        }

        return edgeTiles;
    }

    // Helper function for path generation
    private void moveDown()
    {
        pathTiles.Add(currentTile);
        currentIndex = mapTiles.IndexOf(currentTile);
        nextIndex = currentIndex - mapWidth;
        currentTile = mapTiles[nextIndex];
    }

    // Helper function for path generation
    private void moveLeft()
    {
        pathTiles.Add(currentTile);
        currentIndex = mapTiles.IndexOf(currentTile);
        nextIndex = currentIndex - 1;
        currentTile = mapTiles[nextIndex];
    }

    // Helper function for path generation
    private void moveRight()
    {
        pathTiles.Add(currentTile);
        currentIndex = mapTiles.IndexOf(currentTile);
        nextIndex = currentIndex + 1;
        currentTile = mapTiles[nextIndex];
    }
    // Helper function for path generation
    private void moveUp()
    {
        pathTiles.Add(currentTile);
        currentIndex = mapTiles.IndexOf(currentTile);
        nextIndex = currentIndex - mapWidth;
        currentTile = mapTiles[nextIndex];
    }

    // FIXME: this is currently not working
    private void generatePlannedMap()
    {
        BoundsInt bounds = Map.cellBounds;
        Debug.Log(bounds.size);
        mapWidth = bounds.size.x;
        mapHeight = bounds.size.y;
        TileBase[] allTiles = Map.GetTilesBlock(bounds);
        
        int orX = bounds.x;
        int orY = bounds.y;
        List<GameObject> tempPathTiles = new List<GameObject>();
        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null)
                {
                    GameObject newTile = Instantiate(MapTile);
                    newTile.transform.position = new Vector2(orX + x, orY + y);
                    if (tile.name.EndsWith(PathTileSprite.name.Substring(PathTileSprite.name.Length - 4)))
                    {
                        newTile.GetComponent<SpriteRenderer>().color = PathColor;
                        newTile.GetComponent<SpriteRenderer>().sprite = PathTileSprite;
                        tempPathTiles.Add(newTile);
                    }
                    else
                    {
                        newTile.GetComponent<SpriteRenderer>().color = Color.white;
                        newTile.GetComponent<SpriteRenderer>().sprite = MapTileSprite;
                    }
                    mapTiles.Add(newTile);
                }
            }
        }

        //we want the mobs to move from right -> left
        tempPathTiles.Reverse();
        tempPathTiles = pathSort(tempPathTiles, tempPathTiles[0]);

        for(int i = 0; i < tempPathTiles.Count; i++)
        {
            tempPathTiles[i].name = "Path " + i;
        }
        startTile = tempPathTiles[0];
        startTile.GetComponent<SpriteRenderer>().sprite = MapTile.GetComponent<SpriteRenderer>().sprite;
        //startTile.GetComponent<SpriteRenderer>().color = StartTileColor;
        startTile.name = "StartTile";

        endTile = tempPathTiles[tempPathTiles.Count - 1];
        endTile.GetComponent<SpriteRenderer>().sprite = MapTile.GetComponent<SpriteRenderer>().sprite;
        //endTile.GetComponent<SpriteRenderer>().color = EndTileColor;
        endTile.name = "EndTile";

        pathTiles = tempPathTiles;
    }

    private List<GameObject> pathSort(List<GameObject> pathList, GameObject startTile)
    {
        List<GameObject> sortedPath = new List<GameObject>();
        GameObject curr = startTile;
        GameObject previous = null;
        sortedPath.Add(curr);

        int loopNum = 0;
        while(curr != null)
        {
            //Debug.Log("LoopNum: " + loopNum);
            GameObject next = findNextStepInPath(pathList, curr, previous);
            if(next != null)
            {
                //Debug.Log("Next Tile Found: " + next.transform.position);
                sortedPath.Add(next);
            }
            else
            {
                //Debug.Log("\'next\' is NULL!");
            }
            
            //update vars
            GameObject temp1 = curr;
            GameObject temp2 = previous;
            previous = curr;
            curr = next;
            loopNum++;
        }

        return sortedPath;
    }

    private GameObject findNextStepInPath(List<GameObject> pathList, GameObject current, GameObject previous)
    {
        List<GameObject> path = pathList;
        GameObject curr = current;
        GameObject prev = previous;
        GameObject res = null;
        
        if(curr != null)
        {
            // there should only be two
            List<GameObject> neighbors = path.FindAll(
                delegate (GameObject obj)
                {
                    Vector2 currPos = curr.transform.position;
                    Vector2 objPos = obj.transform.position;

                    float dist = Vector2.Distance(currPos, objPos);

                    return dist < 1.25f && obj != curr;
                });
            int pos = path.IndexOf(curr);

            if(neighbors.Count > 0)
            {

                foreach (GameObject obj in neighbors)
                {
                    if (prev == null)
                    {
                        res = obj;
                    }
                    else if (prev != null && prev != obj)
                    {
                        res = obj;
                    }
                }
            }
        }
        
        return res;
    }

    
    private void generatePlannedPath()
    {
        List<GameObject> topEdgeTiles = getTopEdgeTiles();
        List<GameObject> bottomEdgeTiles = getBottomEdgeTiles();


        foreach (GameObject obj in pathTiles)
        {
            obj.GetComponent<SpriteRenderer>().color = PathColor;
            obj.GetComponent<SpriteRenderer>().sprite = PathTileSprite;
        }

        startTile.GetComponent<SpriteRenderer>().color = StartTileColor;
        endTile.GetComponent<SpriteRenderer>().color = EndTileColor;
    }

}
