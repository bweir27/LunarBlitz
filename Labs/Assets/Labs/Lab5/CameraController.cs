using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        float horizontalAxis = Input.GetAxis("Horizontal");
        float verticalAxis = Input.GetAxis("Vertical");

        //check if the controller is pushed left, right, up, or down
        if (Mathf.Abs(horizontalAxis) > 0.001f || Mathf.Abs(verticalAxis) > 0.001f)
        {
            gameObject.transform.Translate(
                horizontalAxis * Time.deltaTime,
                verticalAxis * Time.deltaTime,
                0
                );
        }
    }
}
