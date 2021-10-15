using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonControl : MonoBehaviour
{
    [SerializeField] protected GameObject CannonBall;
    public float launchVelocity = 600f;

    // Start is called before the first frame update
    void Start()
    {
        if (CannonBall == null)
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

    void shoot()
    {
        //create Bullet
        GameObject cannonBall = Instantiate(
            CannonBall,
            transform.position,
            transform.rotation);

        // found at: https://learn.unity.com/tutorial/using-c-to-launch-projectiles?signup=true#5fd7ab3bedbc2a7fb11f4e41
        cannonBall.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(launchVelocity, 0));
    }
}
