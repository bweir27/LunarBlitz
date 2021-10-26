using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseShipBullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * bulletSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Collision detected!");
        Destroy(gameObject);
    }
}
