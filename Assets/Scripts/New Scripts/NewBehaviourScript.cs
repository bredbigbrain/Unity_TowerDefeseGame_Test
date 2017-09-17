using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    public Transform myTarget;  // drag the target here
    public GameObject cannonball;  // drag the cannonball prefab here
    public float shootAngle = 30f;  // elevation angle

    void Update()
    {
        if (Input.GetKeyDown("b"))
        {  // press b to shoot
            GameObject ball = Instantiate(cannonball, transform.position, Quaternion.identity);
            try
            {
                ball.GetComponent<Rigidbody>().velocity = BallisticVel(myTarget, shootAngle);
            }
            finally
            {
                Destroy(ball, 10);
            }
        }
    }

    Vector3 BallisticVel(Transform target, float angle)
    {
        var dir = target.position - transform.position;  // get target direction
        var h = dir.y;  // get height difference
        dir.y = 0;  // retain only the horizontal direction
        var dist = dir.magnitude;  // get horizontal distance
        var a = angle * Mathf.Deg2Rad;  // convert angle to radians
        dir.y = dist * Mathf.Tan(a);  // set dir to the elevation angle
        dist += h / Mathf.Tan(a);  // correct for small height differences
                                   // calculate the velocity magnitude
        var vel = Mathf.Sqrt(dist * Physics.gravity.magnitude / Mathf.Sin(2 * a));
        return vel * dir.normalized;
    }


}
