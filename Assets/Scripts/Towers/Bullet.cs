using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public GameObject Target;
    public GameObject PSprefab;
    public Vector3 position;

    public int Damage;
    public float Speed;
    public float range;

    public bool pause = false;

    void Start()
    {

    }

    void Update()
    {
        if (!pause)
        {
            if (Target != null)
                transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, Speed * Time.deltaTime);
            else
                Destroy(transform.gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Unit")
        {
            collision.gameObject.GetComponent<InfoUnit_1>().Damage((float)Damage);
            Destroy(transform.gameObject);
        }

        if ((collision.gameObject.tag == "Untagged") | (collision.gameObject.tag == "Terrain"))
        {
            Destroy(transform.gameObject);
        }
    } 

}
