using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// Made in part by following tutorial: https://www.youtube.com/watch?v=7sxF8JVR74c
public class Tower : MonoBehaviour
{
    public Sprite towerPreviewSprite;
    [SerializeField] private float range;
    [SerializeField] public float damage;
    [SerializeField] private float timeBetweenShots; // Time in seconds between shots
    protected AudioSource fireSound;
    public int cost;

    //TODO: choose between multiple kinds of towers
    [SerializeField] private GameObject towerType;

    // for showing the tower range on :hover
    [SerializeField] protected Camera camera;
    protected Vector2 mousePosInWorldCoords;
    [SerializeField] protected GameObject rangeDisplayPrefab;
    protected GameObject rangeDisplay;
    [SerializeField] protected Color rangeDisplayColor;

    protected SpriteRenderer rangeRenderer;

    public bool isFocused = false;
    protected bool isActive = true;
    private float nextTimeToShoot;

    public GameObject currentTarget;
    protected Coroutine targeting;

    private void Awake()
    {
        Towers.towers.Add(gameObject);
        isActive = true;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        nextTimeToShoot = Time.time;
        if(camera == null)
        {
            camera = Camera.main;
        }

        // make Tower range display invisible onInit
        rangeDisplay = Instantiate(rangeDisplayPrefab, gameObject.transform.position, Quaternion.identity);
        Transform towerTransform = gameObject.GetComponent<Transform>();

        rangeRenderer = rangeDisplay.GetComponent<SpriteRenderer>();

        rangeDisplay.transform.localScale = new Vector2(range * 2, range * 2);

        Color c = rangeRenderer.color;
        rangeDisplayColor = c;
        rangeDisplayColor.a = 0.45f;
        c.a = 0;
        rangeRenderer.color = c;

        rangeDisplay.transform.SetParent(gameObject.transform);

        fireSound = gameObject.GetComponent<AudioSource>();
        if(fireSound == null)
        {
            Debug.LogError("This tower does not have an AudioSource!");
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (isActive)
        {
            updateLeadEnemy();
            if (Time.time >= nextTimeToShoot)
            {
                if (currentTarget != null)
                {
                    shoot();
                    nextTimeToShoot = Time.time + timeBetweenShots;
                }
            }

            rangeDisplay = this.transform.GetChild(1).gameObject;
            rangeRenderer = rangeDisplay.GetComponent<SpriteRenderer>();

            // Listen for mouseHover to focus
            // get the mouse coordinates (which are in screen coords)
            // and convert them to world coordinates
            mousePosInWorldCoords = camera.ScreenToWorldPoint(Input.mousePosition);

            // get a ray from the mouse coordinates
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            //do a raycast into the scene
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit && hit.collider != null)
            {
                if (hit.collider.gameObject.name == gameObject.name)
                {
                    showRangeDisplay();
                }
                else
                {
                    hideRangeDisplay();
                }
            }
        }
    }


    protected virtual void updateLeadEnemy()
    {
        currentTarget = GetLeadEnemy();
    }

    protected virtual void shoot()
    {
        if(fireSound != null)
        {
            fireSound.Play();
        }
    }

    protected virtual IEnumerator waitForRotation()
    {
        yield return new WaitForSeconds(0.0005f);
    }

    protected virtual void showRangeDisplay()
    {
        if(rangeDisplay == null)
        {
            rangeDisplay = Instantiate(rangeDisplayPrefab, gameObject.transform.position, Quaternion.identity);
            rangeDisplay.transform.localScale = new Vector2(range * 2, range * 2);
            rangeDisplay.transform.SetParent(gameObject.transform);
        }
        Color c = rangeDisplayColor;
        c.a = 0.45f;
        rangeRenderer.color = c;
    }

    protected virtual void hideRangeDisplay()
    {
        Color c = rangeDisplayColor;
        c.a = 0;
        rangeRenderer.color = c;
    }



    // === Helpers ===

    // Calculates the distance between a GameObject (enemy) and this tower
    private float DistanceFromTower(GameObject enemy)
    {
        return (transform.position - enemy.transform.position).magnitude;
    }

    // returns the tile position of 
    private int GetEnemyTilePos(GameObject enemy)
    {
        //return MapGenerator.pathTiles.IndexOf(enemy.GetComponent<MobBehaviorTree>().targetTile);
        return MapGenerator.pathTiles.IndexOf(
            enemy.GetComponent<MobBehaviorTree>().Root.TargetTile);
    }

    // returns the distance traveled by an enemy
    //  The enemy with the highest distance traveled leads the pack 
    private float GetEnemyDistanceCovered(GameObject enemy)
    {
        //return MapGenerator.pathTiles.IndexOf(enemy.GetComponent<MobBehaviorTree>().targetTile);
        return enemy.GetComponent<Enemy>().DistanceCovered;
    }

    // Filters the provided list of Enemies and returns those that are within
    //     range of this tower
    protected virtual List<GameObject> GetEnemiesWithinRange(List<GameObject> enemyList)
    {
        List<GameObject> enemiesInRange = enemyList.Where(c => DistanceFromTower(c) <= range).ToList();
        
        return enemiesInRange;
    }

    protected virtual List<GameObject> SortEnemiesInRangeByTilePos(List<GameObject> enemyList)
    {
        // of the enemies that are within range of the tower,
        //  sort them by the index of the pathTile they are on
        return GetEnemiesWithinRange(enemyList)
                .OrderBy(o => GetEnemyTilePos(o))
                .ToList();
    }

    protected virtual List<GameObject> SortEnemiesInRangeByDistanceCovered(List<GameObject> enemyList)
    {
        // of the enemies that are within range of the tower,
        //  sort them by the distance they've covered (i.e., order in the pack)
        return GetEnemiesWithinRange(enemyList)
                .OrderByDescending(o => GetEnemyDistanceCovered(o))
                .ToList();
    }

    protected virtual GameObject GetLeadEnemy()
    {
        //List<GameObject> sortedEnemiesInRange = SortEnemiesInRangeByTilePos(Enemies.enemies);
        List<GameObject> sortedEnemiesInRange = SortEnemiesInRangeByDistanceCovered(Enemies.enemies);
        if (sortedEnemiesInRange.Count > 0)
        {
            //Debug.Log("Enemy in Lead: " + sortedEnemiesInRange[0].name + " - DistanceCovered: " + GetEnemyDistanceCovered(sortedEnemiesInRange[0]));
            return sortedEnemiesInRange[0];
        }
        else
        {
            return null;
        }
    }

    // stop the aiming & firing during end game
    public virtual void shutdownTower()
    {
        isActive = false;
    }
}
