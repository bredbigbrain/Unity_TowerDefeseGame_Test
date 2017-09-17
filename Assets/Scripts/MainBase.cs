using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainBase : MonoBehaviour {

    public float health = 500;
    private float currHealth;

    private GameMode gameMode;

    public GameObject healthBar;
    public Image currHealthImage;

    void Start()
    {
        gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();

        currHealth = health;
    }

    void Update()
    {
        if (gameMode.mainCam.enabled)
        {
            healthBar.SetActive(true);
            healthBar.transform.LookAt(gameMode.currCam.transform.position);
        }
        else
        {
            healthBar.SetActive(false);
        }
        if (currHealth <=0)
        {
            gameMode.GameOver();
        }
    }

    public void Damage(float _damage)
    {
        currHealth -= _damage;
        currHealthImage.fillAmount = currHealth / health;
    }
}
