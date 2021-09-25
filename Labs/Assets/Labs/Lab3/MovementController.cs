using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public float minScaleSize = 1.5f;
    public Vector3 startPos = new Vector3(-6, 0, 0);
    public Vector3 endPos = new Vector3(6, 0, 0);
    public Vector3 scaleSize = new Vector3(1.0f, 1.0f, 1.0f);
    public bool facingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("GameManager");
        GameManager sm = obj.GetComponent<GameManager>();

        Vector3 destination;
        if (facingRight)
        {
            // move right toward endPos
            destination = endPos;
        }
        else
        {
            // move left toward startPos
            destination = startPos;

        }

        // perform scaling
        // idea to use Mathf.Sin(Time.time) from: https://answers.unity.com/questions/1369023/how-to-scale-an-object-up-and-down-over-time.html
        scaleSize = new Vector3(
            Mathf.Sin(Time.time) + minScaleSize + 1,
            Mathf.Sin(Time.time) + minScaleSize + 1,
            1);
        transform.localScale = scaleSize;

        //perform rotation
        gameObject.transform.Rotate(0, 0, sm.rotationSpeed);

        //move toward target
        transform.position = Vector3.MoveTowards(
            transform.position,
            destination,
            sm.movementSpeed * Time.deltaTime);


        //if reached one of the targets, change direction
        if (gameObject.transform.position.x <= startPos.x || gameObject.transform.position.x >= endPos.x)
        {
            facingRight = !facingRight;
            Debug.Log("Flipping");
        }
    }
}
