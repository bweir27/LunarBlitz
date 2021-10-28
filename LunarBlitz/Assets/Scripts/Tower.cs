using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Made in part by following tutorial: https://www.youtube.com/watch?v=7sxF8JVR74c
public class Tower : MonoBehaviour
{
    public Sprite towerPreviewSprite;
    [SerializeField] private float range;
    [SerializeField] public float damage;
    [SerializeField] private float timeBetweenShots; // Time in seconds between shots
    public int cost;

    

    //TODO: choose between multiple kinds of towers
    [SerializeField] private GameObject towerType;

    // for showing the tower range on :hover
    [SerializeField] protected Camera camera;
    protected Vector2 mousePosInWorldCoords;
    [SerializeField] protected GameObject rangeDisplay;
    [SerializeField] protected Color rangeDisplayColor;

    protected SpriteRenderer rangeRenderer;

    public bool isFocused = false;
    private float nextTimeToShoot;

    public GameObject currentTarget;

    // Start is called before the first frame update
    void Start()
    {
        nextTimeToShoot = Time.time;
        if(camera == null)
        {
            camera = Camera.main;
        }

        // make Tower range display invisible onInit
        this.rangeDisplay = GameObject.FindGameObjectWithTag("TowerRangeDisplay");
        this.rangeRenderer = rangeDisplay.GetComponent<SpriteRenderer>();
        this.rangeDisplay.transform.localScale = new Vector2(range, range);
        Color c = rangeRenderer.color;
        rangeDisplayColor = c;
        rangeDisplayColor.a = 0.45f;
        c.a = 0;
        rangeRenderer.color = c;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        updateLeadEnemy();
        if (Time.time >= nextTimeToShoot)
        {
            if(currentTarget != null)
            {
                shoot();
                nextTimeToShoot = Time.time + timeBetweenShots;
            }
        }

        //rangeDisplay = this.transform.GetChild(1).gameObject;
        //rangeRenderer = rangeDisplay.GetComponent<SpriteRenderer>();

        //// Listen for mouseHover to focus
        //// get the mouse coordinates (which are in screen coords)
        //// and convert them to world coordinates
        //mousePosInWorldCoords = camera.ScreenToWorldPoint(Input.mousePosition);

        //// get a ray from the mouse coordinates
        //Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        ////do a raycast into the scene
        //RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        //if (hit && hit.collider != null)
        //{
        //    //FIXME: hover over tower to show range
        //    Debug.Log("Hit! " + hit.collider.gameObject.name);
        //    if (hit.collider.gameObject.name == gameObject.name)
        //    {
        //        Debug.Log("Hit Tower!");
        //        rangeRenderer.color = rangeDisplayColor;
        //        Debug.Log("Display Color: (" + rangeRenderer.color.r + ", " + rangeRenderer.color.g + ", " + rangeRenderer.color.b + ", " + rangeRenderer.color.a + ")");
        //    }
        //    else
        //    {
        //        Color invis = rangeDisplayColor;
        //        invis.a = 0;
        //        rangeRenderer.color = invis;
        //    }
        //}
        //else
        //{
        //    //GetComponent<SpriteRenderer>().color = Color.white;
        //}
        ////rangeRenderer.color = Color.clear;
    }


    private void updateLeadEnemy()
    {
        //Debug.Log("Updating Lead Enemy");
        GameObject currentLeadEnemyInRange = null;

        float distance = -Mathf.Infinity;
        int _maxTilePos = -1;

        foreach (GameObject enemy in Enemies.enemies)
        {
            if (enemy != null)
            {
                float _distance = (transform.position - enemy.transform.position).magnitude;
                // ensure enemy is within range of tower
                if (_distance <= range)
                {
                    int tilePos = MapGenerator.pathTiles.IndexOf(enemy.GetComponent<Enemy>().targetTile);
                    if (tilePos > _maxTilePos)
                    {
                        distance = _distance;
                        currentLeadEnemyInRange = enemy;
                        _maxTilePos = tilePos;
                    }
                }

                //if(enemy != currentLeadEnemyInRange)
                //{
                //    enemy.GetComponent<SpriteRenderer>().color = Color.white;
                //}
            }
        }

        if (currentLeadEnemyInRange != null)
        {
            currentTarget = currentLeadEnemyInRange;
            //currentTarget.GetComponent<SpriteRenderer>().color = Color.blue;
        }
        else
        {
            currentTarget = null;
        }
    }

    protected virtual void shoot()
    {
        Enemy enemyScript = currentTarget.GetComponent<Enemy>();
        enemyScript.takeDamage(damage);
    }
}
