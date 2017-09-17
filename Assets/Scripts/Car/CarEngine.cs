using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEngine : MonoBehaviour {

    public InfoUnit_1 Value;
    public List<GameObject> path;
    public float maxSteerAngle = 45f;
    public float motorTorque = 20f;
    private float speedK;
    private float speedK_2 = 1;
    public float speedK_1 = 0.5f;

    public float currSpeed;

    public WheelCollider whellHL;
    public WheelCollider whellHR;

    public Vector3 centerOfMass;
    private Vector3 lastPos;
    private Quaternion lastRot;

    private int currNode = 0;

	void Start () {
        GetComponent<Rigidbody>().centerOfMass = centerOfMass;

        path = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>().Areas;
        Value = GetComponent<InfoUnit_1>();

    }
	
	void FixedUpdate () {
        if (!(Value.gameOver | Value.pause))
        {
            ApplySteer();
            Drive();

            lastPos = transform.position;
            lastRot = transform.rotation;
        }
        else
        {
            transform.position = lastPos;
            transform.rotation = lastRot;
        }
	}

    private void ApplySteer()
    {
        if ((Vector3.Distance(transform.position, path[currNode].transform.position) <= 0.5) & (currNode < path.Count - 1))
        {
            currNode++;
        }

        Vector3 relativeVector = transform.InverseTransformPoint(path[currNode].transform.position);
        float newSteer = maxSteerAngle * (relativeVector.x / relativeVector.magnitude);

        speedK =  1 - Mathf.Abs(relativeVector.x / relativeVector.magnitude);

        whellHL.steerAngle = newSteer;
        whellHR.steerAngle = newSteer;
    }

    void Drive()
    {
        if(Vector3.Distance(transform.position, path[currNode].transform.position) <= 3)
        {
            speedK_2 = speedK_1;
        }
        else
        {
            speedK_2 = 1f;
        }

        currSpeed = 2 * Mathf.PI * whellHL.radius * whellHL.rpm * 60 / 1000;

        if(currSpeed <= Value.Value.Speed)
        {
            whellHR.motorTorque = motorTorque * speedK * speedK_2;
            whellHL.motorTorque = motorTorque * speedK * speedK_2;
        }
        else
        {
            whellHL.motorTorque = 0;
            whellHR.motorTorque = 0;
        }
    }
}
