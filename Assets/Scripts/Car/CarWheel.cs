using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarWheel : MonoBehaviour {

    public WheelCollider wheelCollider;

    private Vector3 wheelPosition;
    private Quaternion wheelrotation;

    private GameMode gameMode;

    void Start()
    {
        gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
    }

	void Update () {
        if (!(gameMode.pause | gameMode.gameOver))
        {
            wheelCollider.GetWorldPose(out wheelPosition, out wheelrotation);

            //  transform.position = wheelPosition;
            transform.rotation = wheelrotation;
        }
	}
}
