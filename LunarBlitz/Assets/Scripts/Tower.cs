using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Made in part by following tutorial: https://www.youtube.com/watch?v=7sxF8JVR74c
public class Tower : MonoBehaviour
{
    [SerializeField] private float range;
    [SerializeField] private float damage;
    [SerializeField] private float timeBetweenShots; // Time in seconds between shots

    //TODO: choose between multiple kinds of towers
    [SerializeField] private GameObject towerType;

    private float nextTimeToShoot;

    public GameObject currentTarget;

    // Start is called before the first frame update
    void Start()
    {
        nextTimeToShoot = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Tower Update");
        updateNearestEnemy();
        if(Time.time >= nextTimeToShoot)
        {
            if(currentTarget != null)
            {
                shoot();
                nextTimeToShoot = Time.time + timeBetweenShots;
            }
        }
    }

    //TODO: target enemy closest to end -> not closest to tower
    private void updateNearestEnemy()
    {
        GameObject currentNearestEnemy = null;

        float distance = Mathf.Infinity;

        foreach(GameObject enemy in Enemies.enemies)
        {
            if (enemy != null)
            {
                float _distance = (transform.position - enemy.transform.position).magnitude;
                if (_distance < distance)
                {
                    distance = _distance;
                    currentNearestEnemy = enemy;
                }
            }
        }

        if(distance <= range)
        {
            currentTarget = currentNearestEnemy;
        }
        else
        {
            currentTarget = null;
        }
        //Debug.Log("CurrentTarget: " + currentTarget);
    }

    protected virtual void shoot()
    {
        Enemy enemyScript = currentTarget.GetComponent<Enemy>();
        enemyScript.takeDamage(damage);
        
    }
}
