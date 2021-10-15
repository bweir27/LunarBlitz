using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundListener : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    // called when this object or another object collides with us
    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log(col.gameObject.name + " entered a collision with the Ground!");
    }
}
