using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorcycleEngine : MonoBehaviour {

    public WheelCollider headWheelCol;
    public WheelCollider tailWheelCol;

    public Transform[] path;

    public float maxSteerAngle;
    public float maxSpeed;
    private float speedK;
    public float speedK_1 = 0.5f;

    public float motorTorque;
    public float currSpeed;

    public float centerOfMassY = -2.9f;
    public float zRotSpeed = 0.5f;

    public int currNode = 0;

    void Start () {
        GetComponent<Rigidbody>().centerOfMass = new Vector3(0, centerOfMassY, 0);
	}
	
	void Update () {
        ApplySteer();
        Drive();
    }

    private void ApplySteer()
    {
        if ((Vector3.Distance(transform.position, path[currNode].transform.position) <= 3) & (currNode < path.Length - 1))
        {
            currNode++;
        }

        Vector3 relativeVector = transform.InverseTransformPoint(path[currNode].transform.position);
        float newSteer = maxSteerAngle * (relativeVector.x / relativeVector.magnitude);

        speedK = 1 - Mathf.Abs(relativeVector.x / relativeVector.magnitude);

        headWheelCol.steerAngle = newSteer;
    }

    void Drive()
    {
        currSpeed = 2 * Mathf.PI * tailWheelCol.radius * tailWheelCol.rpm * 60 / 1000;

        if (currSpeed <= maxSpeed)
        {
            tailWheelCol.motorTorque = motorTorque * speedK;
        }
        else
        {
            tailWheelCol.motorTorque = 0;
        }
    }
}
