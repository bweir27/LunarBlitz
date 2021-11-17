using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletExplosion : MonoBehaviour
{
    private ParticleSystem explosion;

    // Start is called before the first frame update
    void Start()
    {
        explosion = gameObject.GetComponent<ParticleSystem>();
        Destroy(gameObject, explosion.main.duration + 0.5f);
    }
}
