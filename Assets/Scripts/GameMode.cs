using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameMode : MonoBehaviour {

    private GameHUD gameHUD;

    public bool gameOver, pause;

    public List<Wave> Waves = new List<Wave>();
    public List<GameObject> GameUnits = new List<GameObject>();
    public List<GameObject> Areas = new List<GameObject>();
    public Transform spawnPoint;

    public List<GameObject> GameTowers = new List<GameObject>();
    public List<GameObject> ShopTowers = new List<GameObject>();
    public List<GameObject> GroundTower = new List<GameObject>();

    public int timeM, timeS;
    private float timeMS;

    public float spawnRotation = -60;
    public int CoutWaves;
    public int CurrWave;

    private float timeWave = 5.0f;
    private float timeUnit = 0f;
    private bool wavesRemains = true;
    private int keyUnit = 0;

    public float kills = 0f;
    public int Diamonds = 0;

    public Camera mainCam;
    public Camera baseCam;
    public Camera currCam;
    public GameObject royalGun;

    public bool shopButCliked;
    public int idShopTower = 0;
    public GameObject testObj = null;

    void Start()
    {
        Time.timeScale = 1;

        gameHUD = transform.GetComponent<GameHUD>();
        currCam = mainCam;
        CoutWaves = Waves.Count;
        CurrWave = 0;

        gameOver = false; pause = false;

        mainCam.enabled = true;
        mainCam.GetComponent<AudioListener>().enabled = true;
        baseCam.enabled = false;
        baseCam.GetComponent<AudioListener>().enabled = false;

        shopButCliked = false;

        foreach (GameObject obj in GroundTower)
        {
            obj.GetComponent<TowerGround>().UpdateVisibility(shopButCliked);
        }
    }

    void Update()
    {
        if (!(gameOver | pause))
        {
            Timer();
            WaveSpawning();
        }
        if (!wavesRemains && GameUnits.Count == 0 && !gameOver)
        {
            Victory();
        }

        if (shopButCliked)
            BuildTower(ShopTowers[idShopTower]);
    }

    public void Pause(bool _value)
    {
        pause = _value;
        if (_value)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;

        gameHUD.Pause(_value);

        foreach (GameObject obj in GameTowers)
        {
            obj.GetComponent<InfoTower>().pause = _value;

            if (obj.GetComponent<InfoTower>().bullets.Count != 0)
                foreach (GameObject bul in obj.GetComponent<InfoTower>().bullets)
                {
                    if (bul != null)
                    {
                        if (bul.GetComponent<Bullet>())
                            bul.GetComponent<Bullet>().pause = _value;
                        if (bul.GetComponent<Rocket>())
                            bul.GetComponent<Rocket>().pause = _value;
                        if (bul.GetComponent<Bomb>())
                            bul.GetComponent<Bomb>().pause = _value;
                    }
                }
        }

        foreach (GameObject obj in GameUnits)
        {
            if (obj != null)
                obj.GetComponent<InfoUnit_1>().pause = _value;
        }
    }

    private void Victory()
    {
        gameHUD.WinGame();
        Time.timeScale = 0;
        pause = true;

        foreach (GameObject obj in GameTowers)
        {
            obj.GetComponent<InfoTower>().pause = true;

            if (obj.GetComponent<InfoTower>().bullets.Count != 0)
                foreach (GameObject bul in obj.GetComponent<InfoTower>().bullets)
                {
                    if (bul != null)
                    {
                        if (bul.GetComponent<Bullet>())
                            bul.GetComponent<Bullet>().pause = true;
                        if (bul.GetComponent<Rocket>())
                            bul.GetComponent<Rocket>().pause = true;
                        if (bul.GetComponent<Bomb>())
                            bul.GetComponent<Bomb>().pause = true;
                    }
                }
        }

        foreach (GameObject obj in GameUnits)
        {
            if (obj != null)
                obj.GetComponent<InfoUnit_1>().pause = true;
        }
    }

    public void GameOver()
    {
        gameOver = true;
        pause = true;
        
        gameHUD.EndGame();

        foreach (GameObject obj in GameTowers)
        {
            obj.GetComponent<InfoTower>().pause = true;

            if (obj.GetComponent<InfoTower>().bullets.Count != 0)
                foreach (GameObject bul in obj.GetComponent<InfoTower>().bullets)
                {
                    if (bul != null)
                    {
                        if (bul.GetComponent<Bullet>())
                            bul.GetComponent<Bullet>().pause = true;
                        if (bul.GetComponent<Rocket>())
                            bul.GetComponent<Rocket>().pause = true;
                        if (bul.GetComponent<Bomb>())
                            bul.GetComponent<Bomb>().pause = true;
                    }
                }
        }

        foreach (GameObject obj in GameUnits)
        {
            obj.GetComponent<InfoUnit_1>().gameOver = true;
        }

        Time.timeScale = 0;
    }

    private void WaveSpawning()
    {
        if (timeWave <= 0.0f && wavesRemains)
        {
            if (CurrWave <= CoutWaves - 1)
            {
                if (timeUnit <= 0.0f)
                {
                    if (keyUnit <= Waves[CurrWave].Units.Count - 1)
                    {
                        GameUnits.Add(Instantiate(Waves[CurrWave].Units[keyUnit], spawnPoint.position, Quaternion.Euler(0, spawnRotation, 0)));

                        keyUnit++;
                        timeUnit = 3.0f;
                    }
                    else
                    {
                        if (GameUnits.Count == 0)
                        {
                            CurrWave++;
                            keyUnit = 0;
                            timeUnit = 0f;
                            timeWave = 5.0f;
                        }
                    }
                }
                else
                {
                    timeUnit -= Time.deltaTime;
                }
            }
            else
            {
                wavesRemains = false;
            }
        }
        else
        {
            timeWave -= Time.deltaTime;
        }
    }

    private void Timer()
    {
        if (timeMS >= 1.0f)
        {
            timeS++;
            timeMS = 0;
            if (timeS >= 60)
            {
                timeS = 0;
                timeM++;
            }
        }
        else
        {
            timeMS += Time.deltaTime;
        }
    }
    
    private void BuildTower(GameObject _towerPrefab)
    {
        foreach (GameObject obj in GroundTower)
        {
            obj.GetComponent<TowerGround>().UpdateVisibility(shopButCliked);
        }


        if ((Input.GetMouseButton(0)) & (!EventSystem.current.IsPointerOverGameObject()))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                if ((hitInfo.collider.gameObject.tag == "Terrain") | ((hitInfo.collider.gameObject.tag == "TowerGround")))
                {
                    if (testObj == null)
                        testObj = Instantiate(_towerPrefab, hitInfo.point, Quaternion.identity);

                    testObj.GetComponent<InfoTower>().pause = true;
                    testObj.gameObject.transform.position = hitInfo.point;
                }
            }
        }
        else
        {
            if (testObj != null)
            {
                if (testObj.GetComponent<InfoTower>().onGround != true)
                {
                    Destroy(testObj.gameObject);
                    shopButCliked = false;
                }
                else if (Diamonds >= testObj.GetComponent<InfoTower>().Value.Cost)
                {
                    testObj.transform.position = testObj.GetComponent<InfoTower>().Value.TGround.transform.position;
                    testObj.GetComponent<InfoTower>().Value.TGround.GetComponent<TowerGround>().isTowered = true;
                    testObj.GetComponent<InfoTower>().pause = false;

                    Diamonds -= testObj.GetComponent<InfoTower>().Value.Cost;

                    GameTowers.Add(testObj);

                    testObj = null;
                    shopButCliked = false;
                }
                else
                {
                    Destroy(testObj.gameObject);
                    shopButCliked = false;
                }

                foreach (GameObject obj in GroundTower)
                {
                    obj.GetComponent<TowerGround>().UpdateVisibility(shopButCliked);
                }
            }
            
            if (Input.GetMouseButton(1) & shopButCliked)
            {
                shopButCliked = false;
                foreach (GameObject obj in GroundTower)
                {
                    obj.GetComponent<TowerGround>().UpdateVisibility(shopButCliked);
                }
            }
        }
    }

    public void ChangeCam()
    {
        mainCam.enabled = !mainCam.enabled;
        mainCam.GetComponent<AudioListener>().enabled = !mainCam.GetComponent<AudioListener>().enabled;

        baseCam.enabled = !baseCam.enabled;
        baseCam.GetComponent<AudioListener>().enabled = !baseCam.GetComponent<AudioListener>().enabled;

        if (mainCam.enabled)
        {
            currCam = mainCam;
            royalGun.GetComponent<RoyalGun>().active = false;
            royalGun.GetComponent<RoyalGun>().ResetRotation();
        }
        else
        {
            currCam = baseCam;
            royalGun.GetComponent<RoyalGun>().active = true;
        }
    }
}
