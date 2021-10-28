using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjController : MonoBehaviour
{
    public float speed;
    public List<Vector3> waypoints;
	public Color startColor;
	public Color endColor;

	private Coroutine path;
	private Coroutine colorShift;

	// Start is called before the first frame update
	void Start()
    {
		path = StartCoroutine(PatrolWaypoints());
		colorShift = StartCoroutine(InterpolateColors());
		
	}

    // Update is called once per frame
    void Update()
    {
		bool spacebarPress = Input.GetButtonDown("Jump");
        if (spacebarPress)
        {
			Debug.Log("SPACEBAR PRESSED");
			Debug.Log("Stopping...");
			StopPatrolling();
        }
    }

	public virtual void StopPatrolling()
	{
		StopCoroutine(path);
	}

	public IEnumerator PatrolWaypoints()
	{
		// path forever, unless StopPatrolling is called
		while (true)
		{
			// iterate through all points
			foreach (Vector3 point in waypoints)
			{
				Debug.Log("pathing to: " + point);
				// now with each point, I want to interpolate to it, so I'll use MoveTowards
				// which will at most move me at the given speed
				while (transform.position != point)
				{
					transform.position = Vector3.MoveTowards(transform.position, point, speed);
					yield return null;
				}
			}
			yield return null;
		}
	}

	public IEnumerator InterpolateColors()
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		
        while (true)
        {
			spriteRenderer.color = LerpColor(startColor, endColor, Time.time);
			yield return null;
		}
    }

    public Color LerpColor(Color start, Color end, float val)
    {
        return Color.Lerp(start, end, Mathf.PingPong(val, 1));
    }
}
