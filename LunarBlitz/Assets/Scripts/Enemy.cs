using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Made in part by following tutorial: https://www.youtube.com/watch?v=iKtxC4mzpaI&t=0s
public class Enemy : MonoBehaviour
{
    [SerializeField] private float enemyHealth;
    [SerializeField] private float startHealth; // TODO: Will be used for healthbar

    [SerializeField] private float movementSpeed;

    [SerializeField] private Sprite mobSkin;
    [SerializeField] private Animator animator; //TODO: attach animations to mobs

    private int killReward; // The amount of money the played gets when this enemy is killed
    private int damage; // The amount of damage the enemy does when it reached the end

    private GameObject targetTile;

    void Awake()
    {
        startHealth = enemyHealth;
        Enemies.enemies.Add(gameObject);
    }

    void Start()
    {
        //Enemies.enemies.Add(gameObject);
        initEnemy();
    }

    void Update()
    {
        checkPosition();
        moveEnemy();
        takeDamage(0);
    }

    private void initEnemy()
    {
        targetTile = MapGenerator.startTile;
        if(mobSkin != null)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = mobSkin;
        }
    }

    public void takeDamage(float amount)
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

    private void die()
    {
        //TODO: animate death
        Enemies.enemies.Remove(gameObject);
        Destroy(transform.gameObject);
    }

    private void moveEnemy()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetTile.transform.position,
            movementSpeed * Time.deltaTime);
    }

    private void checkPosition()
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
    }
}
