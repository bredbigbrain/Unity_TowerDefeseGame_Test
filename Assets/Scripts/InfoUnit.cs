using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InfoUnit : MonoBehaviour {
    /*
    public Unit Value = new Unit();
    public GameObject circleSelect;
    public bool gameOver = false;
    public bool pause = false;
    
    private int keyArea = 0;
    private bool move = true;
    
    private Vector3 screenPos;
    private Rect backRect;
    private Rect colorRect;

        public int currDest = 0;
    
    private GameMode gameMode;
    private GameHUD gameHUD;
    public NavMeshAgent navMeshAg;


    void Start()
    {
        Value.Initialization("All");
        gameHUD = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameHUD>();
        gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();

        navMeshAg = GetComponent<NavMeshAgent>();
        navMeshAg.speed = Value.Speed;
   //     navMeshAg.destination = gameMode.Areas[currDest].transform.position;

  //      gameHUD.armorPercText.text = (System.Math.Round(Value.PercentArmor, 1) * 100).ToString() + "%";
    }

    void Update()
    {
        if (!(gameOver | pause))
        {
            navMeshAg.Resume();
            Movement();
        }
        else
        {
            navMeshAg.Stop();
        }
        if (Value.CurHealth <= 0)
        {
            gameMode.Diamonds += Value.Diamonds;
            gameMode.GameUnits.Remove(transform.gameObject);
            Destroy(transform.gameObject);
        }
    }

    void Movement()
    {
        if (Vector3.Distance(transform.position, gameMode.Areas[currDest].transform.position) <= 0.15f)
        {
            currDest++;
            
            if (currDest < gameMode.Areas.Count)
                navMeshAg.destination = gameMode.Areas[currDest].transform.position;
            else
            {
     //           gameMode.LosesUnits++;
                gameMode.GameUnits.Remove(transform.gameObject);
                Destroy(transform.gameObject);
            }
        }
    }
    /*
    void Movement()
    {
        if (move)
        {
            if (keyArea <= gameMode.Areas.Count - 1)
            {
                if (Vector3.Distance(transform.position, gameMode.Areas[keyArea].transform.position) > 0)
                {
                    transform.position = Vector3.MoveTowards(transform.position, gameMode.Areas[keyArea].transform.position, Value.Speed * Time.deltaTime);
                }
                else
                {
                    keyArea++;
                }
            }
            else
            {
                move = false;
            }
        }
        else
        {
            gameMode.LosesUnits++;
            gameMode.GameUnits.Remove(transform.gameObject);
            Destroy(transform.gameObject);
        }
    }
    
    void OnMouseUp()
    {
        if (gameHUD.selectedObj != null)
        {
            switch (gameHUD.selectedObj.tag)
            {
                case "Tower":
       //             gameHUD.selectedObj.GetComponent<InfoTower>().circleSelect.SetActive(false);
                    break;
                case "Unit":
                    gameHUD.selectedObj.GetComponent<InfoUnit>().circleSelect.SetActive(false);
                    break;
            }
        }
        gameHUD.selectedObj = gameObject;
        circleSelect.SetActive(true);
        gameHUD.OnInfo(Value.Name, 0, 0, Value.Armor, Value.CurHealth, Value.Health, false);
        gameHUD.descriptionPanel.SetActive(true);
    }
    
    void OnGUI()
    {
        if (!(gameOver | pause))
        {
            screenPos = gameMode.currCam.WorldToScreenPoint(transform.position);
            backRect = new Rect(screenPos.x - 30, Screen.height - screenPos.y + 20, 60, 10);
            colorRect = new Rect(screenPos.x - 30, Screen.height - screenPos.y + 20, 60 * (Value.CurHealth / Value.Health), 10);
            GUI.DrawTexture(backRect, gameHUD.skin.GetStyle("HealthBar").normal.background);
            GUI.DrawTexture(colorRect, gameHUD.skin.GetStyle("HealthBar").active.background);
            GUI.DrawTexture(backRect, gameHUD.skin.GetStyle("HealthBar").hover.background);
        }
    }*/
}
