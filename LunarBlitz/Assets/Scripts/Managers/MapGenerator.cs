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

    public static List<GameObject> mapTiles = new List<GameObject>();
    public static List<GameObject> pathTiles = new List<GameObject>();

    public static GameObject startTile;
    public static GameObject endTile;

    private GameObject currentTile;
    private int currentIndex;
    private int nextIndex;

    // Start is called before the first frame update
    void Start()
    {
        generatePlannedMap();
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

    private void generatePlannedMap()
    {
        BoundsInt bounds = Map.cellBounds;
        mapWidth = bounds.size.x;
        mapHeight = bounds.size.y;

        TileBase[] allTiles = Map.GetTilesBlock(bounds);

        //get offset of tiles to align properly with tilemap
        float xOffset = Map.tileAnchor.x;
        float yOffset = Map.tileAnchor.y;
        
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
                    newTile.transform.position = new Vector2(orX + x + xOffset, orY + y + yOffset);
                    //newTile.transform.position = tile;
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
        startTile.name = "StartTile";

        endTile = tempPathTiles[tempPathTiles.Count - 1];
        endTile.GetComponent<SpriteRenderer>().sprite = MapTile.GetComponent<SpriteRenderer>().sprite;
        endTile.name = "EndTile";

        pathTiles = tempPathTiles;
    }

    // sorts the path tiles in the order they should be traversed
    private List<GameObject> pathSort(List<GameObject> pathList, GameObject startTile)
    {
        List<GameObject> sortedPath = new List<GameObject>();
        GameObject curr = startTile;
        GameObject previous = null;

        // this is the startTile
        sortedPath.Add(curr);

        while(curr != null)
        {
            GameObject next = findNextStepInPath(pathList, curr, previous);
            if(next != null)
            {
                sortedPath.Add(next);
            }
            
            //update vars
            GameObject temp1 = curr;
            GameObject temp2 = previous;

            previous = curr;
            curr = next;
        }

        return sortedPath;
    }

    // a helper method to find the next tile in the path
    private GameObject findNextStepInPath(List<GameObject> pathList, GameObject current, GameObject previous)
    {
        List<GameObject> path = pathList;
        GameObject curr = current;
        GameObject prev = previous;
        GameObject res = null;
        
        if(curr != null)
        {
            // for middle tiles, should only be two,
            //      one of which we have already seen (the previous tile).
            //  When we come across the case that there is only neighbor,
            //      we know that is an endPoint
            List<GameObject> neighbors = path.FindAll(
                delegate (GameObject obj)
                {
                    Vector2 currPos = curr.transform.position;
                    Vector2 objPos = obj.transform.position;

                    float dist = Vector2.Distance(currPos, objPos);

                    return dist < 1.25f && obj != curr;
                });

            // if we are looking at a middleTile
            if(neighbors.Count > 0)
            {
                // FIXME: This works under the assumption that there will only be two,
                //      but could/should be cleaned up for clarity
                //      and cases where there may be two paths diverging / joining
                foreach (GameObject obj in neighbors)
                {
                    if (prev == null || (prev != null && prev != obj))
                    {
                        res = obj;
                    }
                }
            }
        }
        
        return res;
    }
}
