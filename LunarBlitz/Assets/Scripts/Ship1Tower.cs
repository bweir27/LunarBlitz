using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship1Tower : Tower
{
    public Transform pivot;
    public Transform barrel;
    public GameObject bullet;

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        //if (currentTarget != null)
        //{
        //    Debug.Log(base.currentTarget);
        //    Vector2 relative = currentTarget.transform.position - pivot.position;

        //    float angle = Mathf.Atan2(relative.y, relative.x) * Mathf.Rad2Deg;
        //    Debug.Log("angle: " + angle);

        //    Vector3 newRotation = new Vector3(0, 0, angle + 90);

        //    pivot.rotation = Quaternion.Euler(newRotation);
        //}

    }

    protected override void shoot()
    {
        base.shoot();
        Debug.Log(bullet);
        Debug.Log("Fire!");
        //create Bullet
        GameObject newBullet = Instantiate(
            bullet,
            barrel.position,
            pivot.rotation);
    }
}
