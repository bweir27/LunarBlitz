using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
	public float maxSpeed = 10.0f;
	public float speed;
	public float jumpHeight = 0.25f;
	public bool IsFacingRight = true;
	private Animator animator;

	void Start()
	{
        // assign the component to a name that's easy to use in this object

		// finds a Component of type Animator that is attached to
        //		the gameObject that owns this script
        animator = GetComponent<Animator>();

		if(animator == null)
        {
			Debug.LogError("No animator attached!");
        }
    }

    void Update()
	{
		// take the absolute value so I only have to compare for > 0 in the animator
		
		float horizontalAxis = Input.GetAxis("Horizontal");
		//check if the controller is pushed left or right
		if (Mathf.Abs(horizontalAxis) > 0.001f)
        {
			//check direction of movement vs. direction Tim is facing
			if((horizontalAxis > 0 && !IsFacingRight) || (horizontalAxis < 0 && IsFacingRight))
            {
				IsFacingRight = Flip();
            }
			
			//Debug.Log("horizontalAxis: " + horizontalAxis);
			speed += (maxSpeed * horizontalAxis) * Time.deltaTime;
			//Debug.Log("Time.deltaTime: " + Time.deltaTime);
			//Debug.Log("speed: " + speed);
			gameObject.transform.Translate(
				speed,
				transform.position.y,
				transform.position.z);
			//Debug.Log("Pos: " + transform.position.x);
			animator.SetBool("IsRunning", true);
        }
        else //not pushing controller left or right
        {
			animator.SetBool("IsRunning", false);
		}

		//check if spacebar is hit
		float jump = Input.GetAxis("Jump");

		if(Mathf.Abs(jump) > 0.0f)
        {
			speed = 0;
			animator.SetBool("IsJumping", true);
        }
		else
        {
			animator.SetBool("IsJumping", false);
		}
    }

	// Found at: https://answers.unity.com/questions/952558/how-to-flip-sprite-horizontally-in-unity-2d.html
	// Flip sprite / animation over the x-axis
	protected bool Flip()
    {
		IsFacingRight = !IsFacingRight;
        if (IsFacingRight)
        {
			//turn to face left
			transform.localRotation = Quaternion.Euler(0, 0, 0);
		}
		else
        {
			// turn to face Right
			transform.localRotation = Quaternion.Euler(0, 180, 0);
		}
		return IsFacingRight;
	}
}
