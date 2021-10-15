using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HingeManager : MonoBehaviour
{
    // FIXME: This script necessary because my Unity WheelJoint2D
    //      Suspension and Motor dropdown were not appearing properly
    private HingeJoint2D hingeJoint;

    public float motorSpeed = -100;
    public float motorMaxForce = 10000f;

    public float LowerAngleLimit = 0.0f;
    public float UpperAngleLimit = 90.0f;

    // Start is called before the first frame update
    void Start()
    {
        hingeJoint = gameObject.GetComponent<HingeJoint2D>();
        if (hingeJoint == null)
        {
            Debug.LogError("No WheelJoint!");
        }
        Debug.Log("Joint: " + hingeJoint);

        var hingeLimits = hingeJoint.limits;
        var hingeMotor = hingeJoint.motor;


        hingeLimits.max = UpperAngleLimit;
        hingeLimits.min = LowerAngleLimit;

        hingeMotor.motorSpeed = motorSpeed;
        hingeMotor.motorSpeed = motorMaxForce;

        hingeJoint.limits = hingeLimits;
        hingeJoint.motor = hingeMotor;
    }

    // Update is called once per frame
    void Update()
    {
        var hingeLimits = hingeJoint.limits;
        var hingeMotor = hingeJoint.motor;

        hingeLimits.max = UpperAngleLimit;
        hingeLimits.min = LowerAngleLimit;

        hingeMotor.motorSpeed = motorSpeed;
        hingeMotor.motorSpeed = motorMaxForce;

        hingeJoint.limits = hingeLimits;
        hingeJoint.motor = hingeMotor;
    }
}
