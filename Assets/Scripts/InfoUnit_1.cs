using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InfoUnit_1 : MonoBehaviour {

    public Unit Value = new Unit();
    public bool gameOver = false;
    public bool pause = false;
    public float afterDethTime = 3f;

    public GameObject sphereRadius;

    private GameMode gameMode;
    private GameHUD gameHUD;

    void Start()
    {
        Value.Initialization("All");
        gameHUD = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameHUD>();
        gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        sphereRadius.SetActive(false);
    }

    public void Damage(float _damage)
    {
        Value.Damage(_damage);
        if(gameHUD.selectedObj == gameObject)
            gameHUD.UpdateDescription();
    }

    void Update()
    {
        Value.healthBar.transform.LookAt(gameMode.currCam.transform.position);

        if (Value.CurHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        gameMode.Diamonds += Value.Diamonds;
        gameMode.kills++;
        gameMode.GameUnits.Remove(transform.gameObject);

        if (transform.GetComponent<Exploder>())
            transform.GetComponent<Exploder>().Explode();

        if (gameHUD.selectedObj == gameObject)
            gameHUD.HideDescription();

        Destroy(transform.gameObject);
    }

    void OnMouseUp()
    {
        if (gameMode.mainCam.enabled & !EventSystem.current.IsPointerOverGameObject())
        {
            if (gameHUD.selectedObj != null)
            {
                switch (gameHUD.selectedObj.tag)
                {
                    case "Tower":
                        gameHUD.selectedObj.GetComponent<InfoTower>().sphereRadius.SetActive(false);
                        break;
                    case "Unit":
                        gameHUD.selectedObj.GetComponent<InfoUnit_1>().sphereRadius.SetActive(false);
                        break;
                }
            }

            sphereRadius.SetActive(true);
            gameHUD.selectedObj = gameObject;
            gameHUD.UpdateDescription();
        }
    }
}
