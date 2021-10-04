using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Made in part by following tutorial: https://www.youtube.com/watch?v=I6T1En5cPq4
public class MapGenerator : MonoBehaviour
{
    public GameObject MapTile;
    public Color PathColor;
    public Color StartTileColor;
    public Color EndTileColor;

    [SerializeField] private int mapWidth;
    [SerializeField] private int mapHeight;

    //TODO: allow custom map design
    //[SerializeField]
    //private int[,] mapDesign = new int[,] {
    //    { 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
    //    { 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
    //    { 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0 },
    //    { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 },
    //    { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 },
    //    { 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0 },
    //    { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 },
    //    { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 },
    //    { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 },
    //    { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 },
    //};


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
        generateRandomMap();
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

    // FIXME: this is currently not working
    private void generatePlannedMap()
    {
        //bool startTileFound = false;
        //bool endTileFound = false;
        //mapHeight = mapDesign.Length;
        //mapWidth = mapDesign.GetLength(0);
        //for (int i = 0; i < mapDesign.Length; i++)
        //{
        //    GameObject newTile = Instantiate(MapTile);
        //    newTile.transform.position = new Vector2(originX + x, originY + y);
        //    if (!startTileFound && mapDesign[i] > 0)
        //    {
        //        startTileFound = true;
        //        startTile
        //    }

        //    newTile.transform.position = new Vector2(originX + x, originY + y);
        //    mapTiles.Add(newTile);
        //}

        //for (int y = 0; y < mapHeight; y++)
        //{
        //    for (int x = 0; x < mapWidth; x++)
        //    {
        //        GameObject newTile = Instantiate(MapTile);

        //        newTile.transform.position = new Vector2(originX + x, originY + y);
        //        if (mapDesign[x, y] > 0)
        //        {
        //            startTileFound = true;
        //            startTile = newTile;
        //            pathTiles.Add(newTile);
        //        }
        //        mapTiles.Add(newTile);

        //        ////assume endTile has to be in first row, and there is only one
        //        //if(y == 0 && mapDesign[x, y] > 0)
        //        //{
        //        //    endTileFound = true;
        //        //    endTile
        //        //}
        //    }
        //}


        //int startTilePos = topEdgeTiles.Find(;
    }

   
    private void generateRandomMap()
    {
        for(int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                GameObject newTile = Instantiate(MapTile);
                newTile.transform.position = new Vector2(originX + x, originY + y);
                mapTiles.Add(newTile);
            }
        }

        generatePath();
    }

    /* TODO: this can be deleted once generatePlannedMap() is working, 
     * was mostly made when following tutorial, but do not actually want random map generation
     */
    private void generatePath()
    {
        List<GameObject> topEdgeTiles = getTopEdgeTiles();
        List<GameObject> bottomEdgeTiles = getBottomEdgeTiles();

        int startTilePos = Random.Range(0, mapWidth);
        int endTilePos = Random.Range(0, mapWidth);

        startTile = topEdgeTiles[startTilePos];
        endTile = bottomEdgeTiles[endTilePos];

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

        //Debug.Log(pathTiles.Count);
        //Debug.Log(PathColor);

        foreach(GameObject obj in pathTiles)
        {
            obj.GetComponent<SpriteRenderer>().color = PathColor;
        }

        startTile.GetComponent<SpriteRenderer>().color = StartTileColor;
        endTile.GetComponent<SpriteRenderer>().color = EndTileColor;
    }

}
