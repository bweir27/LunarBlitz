using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Made by following tutorial: https://www.youtube.com/watch?v=I6T1En5cPq4
public class MapGenerator : MonoBehaviour
{
    public GameObject MapTile;
    public Color PathColor;

    [SerializeField] private int mapWidth;
    [SerializeField] private int mapHeight;

    private List<GameObject> mapTiles = new List<GameObject>();
    private List<GameObject> pathTiles = new List<GameObject>();

    private bool reachedX = false;
    private bool reachedY = false;

    private GameObject currentTile;
    private int currentIndex;
    private int nextIndex;

    // Start is called before the first frame update
    void Start()
    {
        generateMap();
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


    private void moveDown()
    {
        pathTiles.Add(currentTile);
        currentIndex = mapTiles.IndexOf(currentTile);
        nextIndex = currentIndex - mapWidth;
        currentTile = mapTiles[nextIndex];
    }

    private void moveLeft()
    {
        pathTiles.Add(currentTile);
        currentIndex = mapTiles.IndexOf(currentTile);
        nextIndex = currentIndex - 1;
        currentTile = mapTiles[nextIndex];
    }

    private void moveRight()
    {
        pathTiles.Add(currentTile);
        currentIndex = mapTiles.IndexOf(currentTile);
        nextIndex = currentIndex + 1;
        currentTile = mapTiles[nextIndex];

    }

    private void generateMap()
    {
        for(int y=0; y < mapHeight; y++)
        {
            for(int x = 0; x < mapWidth; x++)
            {
                GameObject newTile = Instantiate(MapTile);

                newTile.transform.position = new Vector2(x, y);
                mapTiles.Add(newTile);
            }
        }
        generatePath();
    }

    private void generatePath()
    {
        List<GameObject> topEdgeTiles = getTopEdgeTiles();
        List<GameObject> bottomEdgeTiles = getBottomEdgeTiles();

        int startTilePos = Random.Range(0, mapWidth);
        int endTilePos = Random.Range(0, mapWidth);

        GameObject startTile = topEdgeTiles[startTilePos];
        GameObject endTile = bottomEdgeTiles[endTilePos];

        currentTile = startTile;

        moveDown();

        int loopCount = 0;
        
        while (!reachedX)
        {
            loopCount++;
            if(loopCount > 100)
            {
                Debug.Log("ReachedX Loop ran too lon! Broke out of it!");
                break;
            }

            if(currentTile.transform.position.x > endTile.transform.position.x)
            {
                moveLeft();
            }
            else if (currentTile.transform.position.x < endTile.transform.position.x)
            {
                moveRight();
            }
            else
            {
                reachedX = true;
            }
        }

        loopCount = 0;

        while (!reachedY)
        {
            loopCount++;
            if (loopCount > 100)
            {
                Debug.Log("ReachedY Loop ran too lon! Broke out of it!");
                break;
            }

            if (currentTile.transform.position.y > endTile.transform.position.y)
            {
                moveDown();
            }
        
            else
            {
                reachedY = true;
            }
        }
        pathTiles.Add(endTile);

        Debug.Log(pathTiles.Count);
        Debug.Log(PathColor);

        foreach(GameObject obj in pathTiles)
        {
            //Destroy(obj);
            Debug.Log(obj);
            obj.GetComponent<SpriteRenderer>().color = PathColor;
        }


    }

}
