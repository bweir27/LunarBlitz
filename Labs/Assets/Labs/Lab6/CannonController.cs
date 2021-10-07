using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : Cannon
{

    // Start is called before the first frame update
    void Start()
    {
        if(base.CannonBall == null)
        {
            Debug.LogError("Missing CannonBall!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool shooting = Input.GetButtonDown("Jump");
        if (shooting)
        {
            shoot();
        }
    }

    protected override void shoot()
    {
        // random rotation
        float randZRotation = Random.Range(-180, 180);
        transform.localRotation = Quaternion.Euler(0, 0, randZRotation);

        base.shoot();
    }
}
