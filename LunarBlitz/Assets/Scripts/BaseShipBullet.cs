using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseShipBullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * 0.25f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Collision detected!");
        Destroy(gameObject);
    }
}
