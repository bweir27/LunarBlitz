using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] protected float bulletSpeed;
    [SerializeField] public float damage;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Destroy(gameObject, 5f);
        if(bulletSpeed <= 0)
        {
            bulletSpeed = 0.2f;
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        transform.position += transform.right * bulletSpeed;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Collision detected!: " + collision.gameObject.name);
        //if(collision.gameObject.tag == "Enemy")
        //{
        //    Enemy hitEnemy = collision.gameObject.GetComponent<Enemy>();
        //    hitEnemy.takeDamage(gameObject.GetComponentInParent<Tower>().damage);
        //}
        
        Destroy(gameObject);
    }
}
