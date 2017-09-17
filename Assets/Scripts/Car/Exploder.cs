using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exploder : MonoBehaviour
{
    public GameObject objParts;
    public GameObject trashParts;
    public GameObject explEffect;

    public float force = 100f;
    public float radius = 5f;

    public float objRadius = 1f;

    public float partLifeTime = 3f;
    public float partAfterLifeTime = 60f;

    public void Explode()
    {
        objParts.SetActive(false);

        if (transform.GetComponent<Rigidbody>())
        {
            Destroy(transform.GetComponent<Rigidbody>());
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, objRadius);

        foreach(Collider col in colliders)
        {
            if (col.gameObject.tag == "Unit")
                Destroy(col);
        }

        Instantiate(explEffect, transform.position, Quaternion.identity);

        GameObject trash = Instantiate(trashParts, transform.position, Quaternion.identity);

        if (!trash.GetComponent<SelfDestruct>())
        {
            trash.AddComponent<SelfDestruct>();
        }
        trash.GetComponent<SelfDestruct>().lifeTime = partLifeTime;
        trash.GetComponent<SelfDestruct>().afterLifeTime = partAfterLifeTime;

        foreach (Transform obj in trash.transform)
        {
            if (!obj.gameObject.GetComponent<BoxCollider>())
            {
                obj.gameObject.AddComponent<BoxCollider>();
            }
            if (!obj.gameObject.GetComponent<Rigidbody>())
            {
                obj.gameObject.AddComponent<Rigidbody>();
            }

            obj.gameObject.GetComponent<Rigidbody>().AddExplosionForce(force, transform.position, radius);
        }
    }
    /*
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, objRadius);
    }
    */
}

