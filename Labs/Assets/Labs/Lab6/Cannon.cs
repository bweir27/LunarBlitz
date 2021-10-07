using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] protected GameObject CannonBall;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected virtual void shoot()
    {
        //create Bullet
        GameObject cannonBall = Instantiate(
            CannonBall,
            transform.position,
            transform.rotation);
    }
}
