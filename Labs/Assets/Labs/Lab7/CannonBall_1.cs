using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall_1 : MonoBehaviour
{
    public float horizSpeed = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //transform.position += transform.right * 0.15f;
        //gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(8, 4));
    }
}
