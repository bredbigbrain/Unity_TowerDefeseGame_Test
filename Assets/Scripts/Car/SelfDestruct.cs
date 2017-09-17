using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour {

    public float lifeTime;
    public float afterLifeTime;

	void Update () {
		if(lifeTime <= 0)
        {
            foreach (Transform obj in transform)
            {
                Destroy(obj.transform.gameObject.GetComponent<Collider>());
                Destroy(obj.transform.gameObject.GetComponent<Rigidbody>());
            }
        }
        else
        {
            lifeTime -= Time.deltaTime;
        }

        if (afterLifeTime <=0)
        {
            Destroy(transform.gameObject);
        }
        else
        {
            afterLifeTime -= Time.deltaTime;
        }
	}
}
