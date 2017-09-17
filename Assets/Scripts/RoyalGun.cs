using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoyalGun : MonoBehaviour {

    public Camera cam;
    public GameObject gun;
    public GameObject blowEffect;
    public Animator anim;
    public AudioSource boom;
    public GameMode gameMode;
    public bool active = false;
    public bool loaded = false;

    private float vertRot = 0.0f;
    public float vertRotRange = 30.0f;
    
    public float damage = 30f;
    public float explosionRange = 1;

	void Start () {
        gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        GameObject.FindObjectOfType<Settings>().AddEffectAudioSourse(boom);
    }
	
	
	void Update () {
        if (active)
        {
            Control();
            Shooting();
        }

        if (gameMode.kills >= 3)
        {
            loaded = true;
        }
    }

    public void ResetRotation()
    {
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        gun.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    void Control()
    {
        float horRot = Input.GetAxis("Mouse X");

        vertRot += -Input.GetAxis("Mouse Y");
        vertRot = Mathf.Clamp(vertRot, -vertRotRange, vertRotRange);

        transform.Rotate(0, horRot, 0);

        gun.transform.localRotation = Quaternion.Euler(vertRot, 0, 0);
    }

    void Shooting()
    {
        if (!loaded)
        {
            anim.SetBool("Shoot", false);
        }

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            if ((Input.GetMouseButtonDown(0)) & loaded)
            { 
                Instantiate(blowEffect, hitInfo.point, Quaternion.identity);
                boom.Play();

                anim.SetBool("Shoot", true);
                loaded = false;
                gameMode.kills = 0;

                foreach (GameObject obj in gameMode.GameUnits)
                {
                    if (Vector3.Distance(hitInfo.point, obj.transform.position) <= explosionRange)
                        obj.GetComponent<InfoUnit_1>().Damage(damage / 3);
                }

                if (hitInfo.collider.gameObject.tag == "Unit")
                {
                    hitInfo.collider.gameObject.GetComponent<InfoUnit_1>().Damage(damage);
                }

                if (hitInfo.collider.gameObject.tag == "Tower")
                {
                    hitInfo.collider.gameObject.GetComponent<InfoTower>().DestroyByRG();
                }

                if (hitInfo.collider.gameObject.tag == "BasePart")
                {
                    hitInfo.collider.gameObject.GetComponent<BasePart>().OnHitByRG(damage);
                }
            }
        }
    }
}
