using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Rect bounds = new Rect(0, 0, 4, 4);
    public float speed = 2.0f;

    public bool isMovingRight = true;
    private Vector3 targetDest;


    // Start is called before the first frame update
    public virtual void Start()
    {
        if (bounds == null)
        {
            bounds = new Rect(0, 0, 4, 4);
        }

        targetDest = new Vector3(
            bounds.xMax,
            gameObject.transform.position.y,
            gameObject.transform.position.z
        );


    }

    public virtual void switchDirections()
    {

        if (isMovingRight)
        {
            targetDest = new Vector3(
                bounds.xMax,
                gameObject.transform.position.y,
                gameObject.transform.position.z
            );
        }
        else
        {
            targetDest = new Vector3(
                bounds.xMin,
                gameObject.transform.position.y,
                gameObject.transform.position.z
            );
        }
        isMovingRight = !isMovingRight;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (gameObject.transform.position == targetDest)
        {
            switchDirections();
        }
        gameObject.transform.position = Vector3.MoveTowards(
                    gameObject.transform.position,
                    targetDest,
                    speed * Time.deltaTime);
    }
}
