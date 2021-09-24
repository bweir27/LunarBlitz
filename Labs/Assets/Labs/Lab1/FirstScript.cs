using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstScript : MonoBehaviour
{
    public float RotationSpeed = 1;
    public float MoveXSpeed = .5f;
    public float FrameCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("First script started");
    }

    // Update is called once per frame
    void Update()
    {
        var objectSize = GetComponent<Renderer>().bounds.size;
        Debug.Log("Time.deltaTime: " + Time.deltaTime);
        // only move if within frame (prevents errors)
        if (transform.position.x < Camera.main.pixelWidth + objectSize.x + 25)
        {
            gameObject.transform.Rotate(0, 0, RotationSpeed);

            // Space.World usage found at: https://docs.unity3d.com/ScriptReference/Transform.Translate.html?_ga=2.10803572.1534103338.1631833363-1381249790.1631557747
            gameObject.transform.Translate(
                MoveXSpeed * Time.deltaTime,
                transform.position.y,
                transform.position.z,
                Space.World // move relative to camera's axis, not GameObject's axis
                );
        }

        // Acheivement
        FrameCounter = FrameCounter + 1;
        Debug.Log("Frame Counter: " + FrameCounter);
    }
}
