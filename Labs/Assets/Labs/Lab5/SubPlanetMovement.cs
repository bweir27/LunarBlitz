using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubPlanetMovement : MonoBehaviour
{
    public float RotationSpeed = 0.5f;
    public float OrbitDegrees = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 parentPosition = transform.parent.transform.position;

        transform.Rotate(0, 0, RotationSpeed, Space.World);
        transform.RotateAround(parentPosition, Vector3.forward, OrbitDegrees);
    }
}
