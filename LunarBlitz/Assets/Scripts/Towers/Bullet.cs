using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] protected float bulletSpeed;
    [SerializeField] public float damage;
    public GameObject ExplosionPrefab;
    protected ParticleSystem OnHitParticleSystem;
    public GameObject target;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Destroy(gameObject, 5f);
        if(bulletSpeed <= 0)
        {
            bulletSpeed = 0.2f;
        }
        if (ExplosionPrefab)
        {
            OnHitParticleSystem = ExplosionPrefab.GetComponent<ParticleSystem>();
            //OnHitParticleSystem.emission.rateOverTime = 0;
        }
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //transform.position += transform.right * bulletSpeed;
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target.transform.position,
                bulletSpeed);
        }
        else
        {
            transform.position += transform.right * bulletSpeed;
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag.Equals("Enemy") && collision.gameObject == target)
        {
            Explode();
            //Destroy(gameObject);
        }
    }

    protected virtual void Explode()
    {
        if (OnHitParticleSystem)
        {
            Instantiate(ExplosionPrefab, gameObject.transform.position, Quaternion.identity);
            //Destroy(ExplosionPrefab, OnHitParticleSystem.main.duration);
            //OnHitParticleSystem.Emit(10);
        }
        else
        {
            Debug.Log("No Particle System found!");
        }

        Destroy(gameObject);
    }
}
