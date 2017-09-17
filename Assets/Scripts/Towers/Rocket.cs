using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    public GameObject blowEff;
    public GameObject PS_fire;
    private GameObject target;
    private Vector3 startDirection;
    private float speed;
    private float damage;
    public float startForce;
    public float flyTime;
    public float blowEffTime;

    private bool launched = false;
    private bool hited = false;
    private Vector3 lastTargrtPosition;

    public bool pause = false;

    void Start()
    {
        blowEff.SetActive(false);
        PS_fire.SetActive(false);
    }

    void Update()
    {
        if (!pause)
        {
            if (target != null)
                lastTargrtPosition = target.transform.position;

            if (launched)
            {
                transform.gameObject.GetComponent<Rigidbody>().useGravity = true;
                transform.gameObject.GetComponent<Rigidbody>().AddForce(startDirection * startForce, ForceMode.Impulse);
                launched = false;
            }
            else if (flyTime <= 0)
            {
                transform.gameObject.GetComponent<Rigidbody>().useGravity = false;
                transform.position = Vector3.MoveTowards(transform.position, lastTargrtPosition, speed * Time.deltaTime);
                PS_fire.SetActive(true);
            }
            else
            {
                flyTime -= Time.deltaTime;
            }

            if (Vector3.Distance(transform.position, lastTargrtPosition) <= 0.05)
            {
                hited = true;
                blowEff.SetActive(true);
            }

            if (hited)
            {
                if (blowEffTime <= 0)
                {
                    Destroy(transform.gameObject);
                }
                else
                {
                    blowEffTime -= Time.deltaTime;
                }
            }
        }
        else
        {
            
        }
    }

    public void Launch(Vector3 _startDirection, GameObject _target, float _speed, float _damage)
    {
        startDirection = _startDirection;
        target = _target;
        speed = _speed;
        damage = _damage;
        launched = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Unit")
        {
            collision.gameObject.GetComponent<InfoUnit_1>().Damage(damage);
            hited = true;
            blowEff.SetActive(true);
        }

        if ((collision.gameObject.tag == "Untagged") | (collision.gameObject.tag == "Terrain"))
        {
            hited = true;
            blowEff.SetActive(true);
        }
    }
}
