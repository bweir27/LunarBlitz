using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Made by following tutorial: https://www.youtube.com/watch?v=iKtxC4mzpaI&t=0s
public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float enemyHealth;

    [SerializeField]
    private float movementSpeed;

    private int killReward; // The amount of money the played gets when this enemy is killed
    private int damage; // The amount of damage the enemy does when it reached the end

    private GameObject targetTile;

    private void Start()
    {
        initEnemy();
    }

    private void Update()
    {
        checkPosition();
        moveEnemy();

        takeDamage(0);
    }

    private void initEnemy()
    {
        targetTile = MapGenerator.startTile;

    }

    public void takeDamage(float amount)
    {
        enemyHealth -= amount;
        if(enemyHealth <= 0)
        {
            die();
        }
    }

    private void die()
    {
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
