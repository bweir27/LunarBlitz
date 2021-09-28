using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluePlanetMovement : MonoBehaviour
{
    public float RotationSpeed = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, RotationSpeed, Space.World);
    }
}
