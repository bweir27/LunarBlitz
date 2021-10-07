using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCannon : Cannon
{

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("shoot", 0.5f, 0.75f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void shoot()
    {
        base.shoot();
    }
}
