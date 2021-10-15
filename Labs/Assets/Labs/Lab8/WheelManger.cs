using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelManger : MonoBehaviour
{
    // FIXME: This script necessary because my Unity WheelJoint2D
    //      Suspension and Motor dropdown were not appearing properly
    private WheelJoint2D wheelJoint;

    public float motorSpeed = -100f;
    public float motorMaxForce = 10000f;

    public float suspensionDamping = 0.7f;
    public float suspensionFreq = 4f;
    public float suspensionAngle = -30;

    // Start is called before the first frame update
    void Start()
    {
        wheelJoint = gameObject.GetComponent<WheelJoint2D>();
        if(wheelJoint == null)
        {
            Debug.LogError("No WheelJoint!");
        }
        Debug.Log("Joint: " + wheelJoint);

        var jointSuspension = wheelJoint.suspension;
        var wheelMotor = wheelJoint.motor;

        jointSuspension.dampingRatio = suspensionDamping;
        jointSuspension.frequency = suspensionFreq;
        jointSuspension.angle = suspensionAngle;

        wheelMotor.maxMotorTorque = motorMaxForce;
        wheelMotor.motorSpeed = motorSpeed;

        wheelJoint.suspension = jointSuspension;
        wheelJoint.motor = wheelMotor;
    }

    // Update is called once per frame
    void Update()
    {
        var jointSuspension = wheelJoint.suspension;
        var wheelMotor = wheelJoint.motor;

        jointSuspension.dampingRatio = suspensionDamping;
        jointSuspension.frequency = suspensionFreq;
        jointSuspension.angle = suspensionAngle;


        wheelMotor.maxMotorTorque = motorMaxForce;
        wheelMotor.motorSpeed = motorSpeed;

        wheelJoint.suspension = jointSuspension;
        wheelJoint.motor = wheelMotor;
    }
}
