using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Made in part by following tutorial: https://www.youtube.com/watch?v=iKtxC4mzpaI&t=0s
public class Enemy : MonoBehaviour
{
    [SerializeField] protected float enemyHealth;
    [SerializeField] protected float startHealth; // TODO: Will be used for healthbar

    [SerializeField] protected float movementSpeed;

    [SerializeField] protected Sprite mobSkin;
    [SerializeField] protected Animator animator; //TODO: attach animations to mobs

    protected PlayerController playerController;
    public int killReward; // The amount of money the played gets when this enemy is killed

    protected bool hasReachedEnd = false;
    public int damage; // The amount of damage the enemy does when it reached the end

    public GameObject targetTile;

    void Awake()
    {
        enemyHealth = startHealth;
        Enemies.enemies.Add(gameObject);
    }

    public virtual void Start()
    {
        initEnemy();
        playerController = FindObjectOfType<PlayerController>();
    }

    public virtual void Update()
    {
        checkPosition();
        moveEnemy();
        takeDamage(0);
    }

    protected virtual void initEnemy()
    {
        targetTile = MapGenerator.startTile;
        if(mobSkin != null)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = mobSkin;
        }
    }

    // listen for getting hit by a bullet
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            Debug.Log("Collision with Projectile detected! " + collision.gameObject.name);
            Bullet b = collision.gameObject.GetComponent<Bullet>();
            takeDamage(b.damage);

            // Enemy destroys projectile on contact to prevent
            //  raceCondition with projectile destroying itself
            //Destroy(b);
            //Debug.Log("Bullet Destroyed");
        }
    }

    public virtual void takeDamage(float amount)
    {
        enemyHealth -= amount;

        // TODO: animate take damage
        //if(animator != null)
        //{
        //    animator.SetBool("IsRunning", true);
        //    animator.SetBool("IsTakingDamage", true);
        //}

        if(enemyHealth <= 0)
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
        Destroy(transform.gameObject);
    }

    protected virtual void moveEnemy()
    {
        if(targetTile != null && gameObject != null)
        {
            transform.position = Vector3.MoveTowards(
            transform.position,
            targetTile.transform.position,
            movementSpeed * Time.deltaTime);
        }
        
    }

    protected virtual void checkPosition()
    {
        // Ensure target tile exists
        if(targetTile != null && targetTile != MapGenerator.endTile)
        {
            // Calculate distance between enemy's position and targetTile's position
            float distance = (transform.position - targetTile.transform.position).magnitude;

            if(distance < 0.001f)
            {
                int currentIndex = MapGenerator.pathTiles.IndexOf(targetTile);

                targetTile = MapGenerator.pathTiles[currentIndex + 1];
            }
        }

        // Detect when mob reaches target tile + decement numLives
        if(targetTile == MapGenerator.endTile)
        {
            // Calculate distance between enemy's position and targetTile's position
            float distance = (transform.position - targetTile.transform.position).magnitude;

            if (distance < 0.001f && !hasReachedEnd)
            {
                playerController.loseLives(damage);
                hasReachedEnd = true;
                // destroy the mob
                Enemies.enemies.Remove(gameObject);
                Destroy(transform.gameObject);
            }
        }
    }
}
