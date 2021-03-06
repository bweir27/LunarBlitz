using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseShipBullet : Bullet
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        bulletSpeed = 0.2f;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if(target != null)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target.transform.position,
                bulletSpeed);
        }
        else
        {
            transform.position += transform.up * bulletSpeed;
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }
}
