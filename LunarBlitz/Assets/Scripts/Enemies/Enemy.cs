using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Made in part by following tutorial: https://www.youtube.com/watch?v=iKtxC4mzpaI&t=0s
public class Enemy : MonoBehaviour
{
    [SerializeField] public float remainingHealth;
    [SerializeField] public float startHealth; // TODO: Will be used for healthbar

    public GameObject healthBarUI;
    public Slider healthBarSlider;

    [SerializeField] public float movementSpeed;
    public float DistanceCovered { get; set; }

    [SerializeField] protected Sprite mobSkin;
    [SerializeField] protected Animator animator; //TODO: attach animations to mobs

    protected PlayerController playerController;
    public int killReward; // The amount of money the played gets when this enemy is killed

    protected bool hasReachedEnd = false;
    public int damage; // The amount of damage the enemy does when it reached the end

    //public GameObject targetTile;

    void Awake()
    {
        remainingHealth = startHealth;
        Enemies.enemies.Add(gameObject);
        healthBarUI.GetComponent<Canvas>().worldCamera = Camera.main;
    }

    public virtual void Start()
    {
        initEnemy();
        playerController = FindObjectOfType<PlayerController>();
    }

    public virtual void Update()
    {
        checkPosition();
        //moveEnemy();
        takeDamage(0);
        healthBarSlider.value = CalculateHealth();

        // only show healthbar once damage has been taken
        if(remainingHealth < startHealth)
        {
            healthBarUI.SetActive(true);
            healthBarUI.GetComponent<Canvas>().enabled = true;
            //Debug.Log("HealthBarUI Active");
        }

        if(remainingHealth > startHealth)
        {
            remainingHealth = startHealth;
        }
    }

    protected virtual void initEnemy()
    {
        //targetTile = MapGenerator.startTile;
        if(mobSkin != null)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = mobSkin;
        }
        healthBarSlider.value = CalculateHealth();
        DistanceCovered = 0f;
    }

    // returns percentage of health remaining
    protected float CalculateHealth()
    {
        return remainingHealth / (float)startHealth;
    }

    // listen for getting hit by a bullet
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            //Debug.Log("Collision with Projectile detected! " + collision.gameObject.name);
            Bullet b = collision.gameObject.GetComponent<Bullet>();

            if(b != null && b.target != null)
            {
                // prevent "splash" damage when sprites may overlap
                if (b.target.name.Equals(gameObject.name))
                {
                    takeDamage(b.damage);
                }
            }
        }
    }

    public virtual void takeDamage(float amount)
    {
        remainingHealth -= amount;

        // TODO: animate take damage
        //if(animator != null)
        //{
        //    animator.SetBool("IsRunning", true);
        //    animator.SetBool("IsTakingDamage", true);
        //}

        if(remainingHealth <= 0)
        {
            die();
        }
    }

    

    protected virtual void die()
    {
        // reward gold 
        playerController.AddMoney(killReward);
        //TODO: animate death
        Enemies.enemies.Remove(gameObject);
        Destroy(gameObject);
    }

    public virtual void moveEnemyTowards(Vector3 targetDest)
    {
        if (targetDest != null && gameObject != null)
        {
            float thisMoveDist = movementSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetDest,
                thisMoveDist);

            // Update distance covered to be used in Tower targeting system
            DistanceCovered = DistanceCovered + thisMoveDist;
        }

    }

    protected virtual void checkPosition()
    {
        // Ensure target tile exists
        //if(targetTile != null && targetTile != MapGenerator.endTile)
        //{
        //    // Calculate distance between enemy's position and targetTile's position
        //    float distance = (transform.position - targetTile.transform.position).magnitude;

        //    if(distance < 0.001f)
        //    {
        //        int currentIndex = MapGenerator.pathTiles.IndexOf(targetTile);

        //        targetTile = MapGenerator.pathTiles[currentIndex + 1];
        //    }
        //}

        //// Detect when mob reaches target tile + decement numLives
        //if(targetTile == MapGenerator.endTile)
        //{
        //    // Calculate distance between enemy's position and targetTile's position
        //    float distance = (transform.position - targetTile.transform.position).magnitude;

        //    if (distance < 0.001f && !hasReachedEnd)
        //    {
        //        playerController.loseLives(damage);
        //        hasReachedEnd = true;
        //        // destroy the mob
        //        Enemies.enemies.Remove(gameObject);
        //        Destroy(transform.gameObject);
        //    }
        //}
    }
}
