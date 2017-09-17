using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

    public GameObject blowEff;
    public GameObject trackEff;

    private GameMode gameMode;
    private bool hitted = false;
    private bool missed = false;
    private float afterLifeTime;
    private float hittedTime;
    public float blowEffTime;
    public float afterMissTime;

    public float range;
    public float damage;

    public bool pause = false;

	void Start () {
        gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        afterLifeTime = blowEffTime;
        hittedTime = afterMissTime;
        blowEff.transform.localScale = new Vector3(range * 3, range * 3, range * 3);
        blowEff.SetActive(false);
	}
	
	
	void Update () {
        if (!pause)
        {
            if (missed)
            {
                if (hittedTime <= 0)
                {
                    blowEff.SetActive(true);
                    transform.gameObject.GetComponent<MeshRenderer>().enabled = false;
                    trackEff.SetActive(false);
                    foreach (GameObject obj in gameMode.GameUnits)
                    {
                        if ((Vector3.Distance(transform.position, obj.transform.position) <= range) & !hitted)
                            obj.GetComponent<InfoUnit_1>().Damage(damage);
                    }
                    hitted = true;
                }
                else
                {
                    hittedTime -= Time.deltaTime;
                }
            }

            if (hitted)
            {
                if (afterLifeTime <= 0)
                {
                    Destroy(transform.gameObject);
                }
                else
                {
                    afterLifeTime -= Time.deltaTime;
                }
            }
        }
	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Unit")
        {
            blowEff.SetActive(true);
            transform.gameObject.GetComponent<MeshRenderer>().enabled = false;
            trackEff.SetActive(false);
            foreach (GameObject obj in gameMode.GameUnits)
            {
                if ((Vector3.Distance(transform.position, obj.transform.position) <= range) & !hitted)
                    obj.GetComponent<InfoUnit_1>().Damage(damage);
            }
            hitted = true;
        }

        if ((collision.gameObject.tag == "Untagged") | (collision.gameObject.tag == "Terrain"))
        {
            missed = true;
        }
    }
}
