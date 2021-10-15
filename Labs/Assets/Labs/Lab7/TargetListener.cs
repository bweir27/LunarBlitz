using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetListener : MonoBehaviour
{
    public bool movingRight = true;
    public float movementSpeed = 0.5f;
    private Rigidbody2D rigid;

    // Start is called before the first frame update
    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        if(rigid == null)
        {
            Debug.LogError("No RigidBody2D!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        float xPos = transform.position.x + (movementSpeed * Time.deltaTime);
        transform.position = new Vector3(xPos, transform.position.y, 0);
        
        // Change Direction
        if (transform.position.x > 5f || transform.position.x < 1f)
        {
            movementSpeed = movementSpeed * -1;
        }
    }

    // called when this object or another object collides with us
    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log(col.gameObject.name + " entered a collision with the Target!");
    }
}
