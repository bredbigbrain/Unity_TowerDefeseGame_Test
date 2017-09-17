using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InfoTower : MonoBehaviour {

    public Tower Value = new Tower();
    private GameMode gameMode;
    private GameHUD gameHUD;

    public bool gameOver;
    public bool pause;

    private GameObject target = null;
    private float reloads;
    private bool isAimed = false;

    private GameObject bullet;
    public List<GameObject> bullets = new List<GameObject>();

    public bool onGround = false;

    public GameObject gun;

    public List<GameObject> staffLevel_1;
    public List<GameObject> staffLevel_2;
    public List<GameObject> spawns;
    public List<ParticleSystem> fireEffects;

    public GameObject wheel_1;
    public GameObject wheel_2;

    public GameObject roratbleY;

    private float rotY = 0;
    private float rotX = 0;
    public float rotYspeed = 3;
    public float rotXspeed = 2;

    public GameObject sphereRadius;

    public AudioSource audioSource;

    public float rocketTime = 0.5f;
    private float rockTime, fireTime;
    private int i = 0, j = 0;

    public float aheadDeelay;
    private  Vector3 targetCorr;

    void Start()
    {
        gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        gameHUD = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameHUD>();
        pause = false;
        gameOver = false;
        reloads = 0;
        rockTime = rocketTime;
        fireTime = Value.reloading / 2;
        Value.Level = 1;

        sphereRadius.SetActive(false);

        Value.IconTexture = Value.IconTextureLvl_1;
        Value.BulletSpawn = spawns[0];
        gun = staffLevel_1[0];

        foreach (GameObject obj in staffLevel_2)
        {
            obj.SetActive(false);
        }
        foreach (GameObject obj in staffLevel_1)
        {
            obj.SetActive(true);
        }

        gameHUD.settings.AddEffectAudioSourse(audioSource);
    }

    void Update()
    {
        if (!(gameOver | pause))
        {
            if (target != null)
            {
                Aims();
                Attack();
            }
            else
            {
                foreach (GameObject obj in gameMode.GameUnits)
                {
                    if ((Vector3.Distance(transform.position, obj.transform.position) <= Value.Range) & (Vector3.Distance(transform.position, obj.transform.position) >= Value.MinRange))
                    {
                        target = obj;
                    }
                }
            }

            if (!gameMode.mainCam.enabled)
            {
                sphereRadius.SetActive(false);
            }
        }

        
    }

    void Aims()
    {



        switch (Value.attkType)
        {
            case Tower.atkType.Target:
                {
                    TargetAims();
                    break;
                }
            case Tower.atkType.RangeTarget:
                {
                    RangeTargetAims();
                    break;
                }
            case Tower.atkType.Splash:
                {
                    targetCorr = target.transform.forward * target.GetComponent<CarEngine>().currSpeed / aheadDeelay;
                    targetCorr = new Vector3(Mathf.Clamp(targetCorr.x, -3f, 3f), Mathf.Clamp(targetCorr.y, -0.5f, 0.5f), Mathf.Clamp(targetCorr.z, -0.5f, 0.5f));

                    SplashAims();
                    break;
                }
        }
        
    }

    void TargetAims()
    {
        Vector3 relativeVector = roratbleY.transform.InverseTransformPoint(target.transform.position);
        float sideX = relativeVector.x / relativeVector.magnitude;

        if (sideX > 0.22)
        {
            rotY += rotYspeed;
        }
        else if (sideX < -0.22)
        {
            rotY -= rotYspeed;
        }

        Vector3 relativeVector2 = gun.transform.InverseTransformPoint(target.transform.position);
        float sideY = relativeVector2.y / relativeVector2.magnitude;

        if (sideY > 0.05)
        {
            rotX -= rotXspeed;
        }
        else if (sideY < -0.05)
        {
            rotX += rotXspeed;
        }

        roratbleY.transform.localRotation = Quaternion.Euler(0, rotY, 0);
        gun.transform.localRotation = Quaternion.Euler(rotX, 0, 0);

        wheel_1.transform.localRotation = Quaternion.Euler(0, rotY * 3, 0);
        wheel_2.transform.localRotation = Quaternion.Euler(0, rotY * 3, 0);

        if ((Mathf.Abs(sideX) < 0.22) & (Mathf.Abs(sideY) < 0.05) & (relativeVector.z >= 0))
        {
            isAimed = true;
        }
        else
        {
            isAimed = false;
        }
    }

    void RangeTargetAims()
    {
        Vector3 relativeVector = roratbleY.transform.InverseTransformPoint(target.transform.position);
        float sideX = relativeVector.x / relativeVector.magnitude;

        if (sideX > 0.22)
        {
            rotY += rotYspeed;
        }
        else if (sideX < -0.22)
        {
            rotY -= rotYspeed;
        }


        float requiredAngle = -(Vector3.Distance(transform.position, target.transform.position) / Value.Range) * 90f;

        bool sideY = false;

        if (rotX > requiredAngle + rotXspeed + 3)
        {
            rotX -= rotXspeed;
        }
        else if (rotX < requiredAngle - rotXspeed - 3)
        {
            rotX += rotXspeed;
        }else
        {
            sideY = true;
        }

        roratbleY.transform.localRotation = Quaternion.Euler(0, rotY, 0);
        gun.transform.localRotation = Quaternion.Euler(rotX, 0, 0);

        wheel_1.transform.localRotation = Quaternion.Euler(0, rotY * 3, 0);
        wheel_2.transform.localRotation = Quaternion.Euler(0, rotY * 3, 0);

        if ((Mathf.Abs(sideX) < 0.22) & sideY & (relativeVector.z >= 0))
        {
            isAimed = true;
        }
        else
        {
            isAimed = false;
        }
    }

    void SplashAims()
    {
        Vector3 relativeVector = roratbleY.transform.InverseTransformPoint(target.transform.position + targetCorr);
        float sideX = relativeVector.x / relativeVector.magnitude;

        if (sideX > 0.22)
        {
            rotY += rotYspeed;
        }
        else if (sideX < -0.22)
        {
            rotY -= rotYspeed;
        }


        float requiredAngle = -40 -(1 - Vector3.Distance(transform.position, target.transform.position + targetCorr) / Value.Range) * 90;
        requiredAngle = Mathf.Clamp(requiredAngle, -75, -35);
        bool sideY = false;

        if (rotX > requiredAngle + rotXspeed + 3)
        {
            rotX -= rotXspeed;
        }
        else if (rotX < requiredAngle - rotXspeed - 3)
        {
            rotX += rotXspeed;
        }
        else
        {
            sideY = true;
        }

        roratbleY.transform.localRotation = Quaternion.Euler(0, rotY, 0);
        gun.transform.localRotation = Quaternion.Euler(rotX, 0, 0);

        wheel_1.transform.localRotation = Quaternion.Euler(0, rotY * 3, 0);
        wheel_2.transform.localRotation = Quaternion.Euler(0, rotY * 3, 0);

        if ((Mathf.Abs(sideX) < 0.22) & sideY & (relativeVector.z >= 0))
        {
            isAimed = true;
        }
        else
        {
            isAimed = false;
        }
    }

    void Attack()
    {
        if ((Vector3.Distance(transform.position, target.transform.position) <= Value.Range) & (Vector3.Distance(transform.position, target.transform.position) >= Value.MinRange))
        { 
            if (isAimed)
            {
                if (reloads <= 0.0f)
                {
                    switch (Value.attkType)
                    {
                        case Tower.atkType.Target:
                            {
                                TargetAttack();
                                break;
                            }
                        case Tower.atkType.RangeTarget:
                            {
                                RangeTargetAttack();
                                break;
                            }
                        case Tower.atkType.Splash:
                            {
                                SplashAttack();
                                break;
                            }
                    }
                }
                else
                {
                    reloads -= Time.deltaTime;
                }
            }
        }
        else
        {
            target = null;
        }
    }

    void TargetAttack()
    {
        Ray ray = new Ray(Value.BulletSpawn.transform.position, Value.BulletSpawn.transform.forward);
        RaycastHit hitInfo;
        Physics.Raycast(ray, out hitInfo);

        if ((hitInfo.collider.gameObject == target) & (target != null))
        {
            audioSource.Play();
            fireEffects[0].transform.position = Value.BulletSpawn.transform.position;
            fireEffects[0].transform.rotation = Value.BulletSpawn.transform.rotation;
            fireEffects[0].Play();

            bullet = (GameObject)Instantiate(Value.BulletPref, Value.BulletSpawn.transform.position, Quaternion.identity);
            bullet.GetComponent<Bullet>().Target = target;
            bullet.GetComponent<Bullet>().Damage = Value.Damage;
            bullet.GetComponent<Bullet>().Speed = Value.bltSpeed;

            bullets.Add(bullet);
            reloads = Value.reloading;
        }
        else
        {
            target = null;
        }
    }

    void RangeTargetAttack()
    {
        if (Value.Level == 1)
        {
            if (rockTime <= 0)
            {
                audioSource.Play();
                fireEffects[i].transform.position = spawns[i].transform.position;
                fireEffects[i].transform.rotation = spawns[i].transform.rotation;
                fireEffects[i].Play();
                bullet = (GameObject)Instantiate(Value.BulletPref, spawns[i].transform.position, spawns[i].transform.rotation);
                bullet.GetComponent<Rocket>().Launch(spawns[i].transform.forward, target, Value.bltSpeed, Value.Damage);

                bullets.Add(bullet);

                i++;
                rockTime = rocketTime;
                if (i == 4)
                {
                    i = 0;
                    reloads = Value.reloading;
                }
            }
            else
            {
                rockTime -= Time.deltaTime;
            }
        }
        else
        {
            if (rockTime <= 0)
            {
                if (i < 4)
                {
                    audioSource.Play();
                    fireEffects[i].transform.position = spawns[i].transform.position;
                    fireEffects[i].transform.rotation = spawns[i].transform.rotation;
                    fireEffects[i].Play();
                    bullet = (GameObject)Instantiate(Value.BulletPref, spawns[i].transform.position, Quaternion.identity);
                    bullet.GetComponent<Rocket>().Launch(spawns[i].transform.forward, target, Value.bltSpeed, Value.Damage);

                    bullets.Add(bullet);

                    i++;
                    rockTime = rocketTime;

                }
                else if (fireTime <= 0)
                {
                    audioSource.Play();
                    fireEffects[j].transform.position = spawns[j].transform.position;
                    fireEffects[j].transform.rotation = spawns[j].transform.rotation;
                    fireEffects[j].Play();
                    bullet = (GameObject)Instantiate(Value.BulletPref, spawns[j].transform.position, Quaternion.identity);
                    bullet.GetComponent<Rocket>().Launch(spawns[j].transform.forward, target, Value.bltSpeed, Value.Damage);

                    bullets.Add(bullet);

                    j++;
                    rockTime = rocketTime;
                    if (j == 8)
                    {
                        j = 4;
                        i = 0;
                        reloads = Value.reloading;
                        fireTime = Value.reloading / 2;
                    }
                }
                else
                {
                    fireTime -= Time.deltaTime;
                }
            }
            else
            {
                rockTime -= Time.deltaTime;
            }
        }
    }

    void SplashAttack()
    {
        audioSource.Play();
        
        bullet = (GameObject)Instantiate(Value.BulletPref, Value.BulletSpawn.transform.position, Value.BulletSpawn.transform.rotation);
        try
        {
            bullet.GetComponent<Rigidbody>().velocity = BallisticVel( target.transform.position + targetCorr, -rotX);
            bullet.GetComponent<Bomb>().damage = Value.Damage;
            bullet.GetComponent<Bomb>().range = Value.bulletRange;

            reloads = Value.reloading;

            bullets.Add(bullet);
        }
        catch { }
        finally { }
    }

    Vector3 BallisticVel(Vector3 target, float angle)
    {
        var dir = target - transform.position;  // get target direction
        var h = dir.y;  // get height difference
        dir.y = 0;  // retain only the horizontal direction
        var dist = dir.magnitude;  // get horizontal distance
        var a = angle * Mathf.Deg2Rad;  // convert angle to radians
        dir.y = dist * Mathf.Tan(a);  // set dir to the elevation angle
        dist += h / Mathf.Tan(a);  // correct for small height differences
                                   // calculate the velocity magnitude
        var vel = Mathf.Sqrt(dist * Physics.gravity.magnitude / Mathf.Sin(2 * a));
        return vel * dir.normalized;
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
            gameHUD.selectedObj = gameObject;
            gameHUD.UpdateDescription();

            sphereRadius.SetActive(true);
            sphereRadius.transform.localScale = new Vector3(Value.Range * 2, Value.Range * 2, Value.Range * 2);
        }
    }

    public void Upgrade()
    {
        if (gameMode.Diamonds >= Value.UpgradeCost)
        {
            gameMode.Diamonds -= Value.UpgradeCost;
            Value.UpgradeCost += Value.UpgradeCost / 2;
            Value.Level++;
            Value.Damage += Value.Damage / 2;
            Value.Range += Value.Range / 5;

            reloads = 0;
            rockTime = rocketTime;
            fireTime = Value.reloading / 2;

            gameHUD.UpdateDescription();
        }

        if (Value.Level == 2)
        {
            foreach (GameObject obj in staffLevel_1)
            {
                obj.SetActive(false);
            }

            Value.IconTexture = Value.IconTextureLvl_2;

            gun = staffLevel_2[0];

            foreach (GameObject obj in staffLevel_2)
            {
                obj.SetActive(true);
            }

            switch (Value.attkType)
            {
                case Tower.atkType.RangeTarget:
                    {
                        spawns[0] = spawns[8];
                        spawns[1] = spawns[9];
                        spawns[2] = spawns[10];
                        spawns[3] = spawns[11];
                        break;
                    }
                default:
                    {
                        Value.BulletSpawn = spawns[1];
                        break;
                    }
            }
        }
    }

    public float[] NextLvlStats()
    {
        float[] stats = new float[] { (Value.Damage * 1.5f), (Value.Range * 1.2f) };

        return stats;
    }

    public void SetVisibleRange(float _range)
    {
        sphereRadius.transform.localScale = new Vector3(_range * 2, _range * 2, _range * 2);
    }

    public void Sell()
    {
        gameMode.Diamonds += Value.UpgradeCost / 3;

        Value.TGround.GetComponent<TowerGround>().isTowered = false;
        Value.TGround.GetComponent<TowerGround>().UpdateVisibility(false);

        gameMode.GameTowers.Remove(gameObject);
        Destroy(gameObject);
    }

    public void DestroyByRG()
    {
        Value.TGround.GetComponent<TowerGround>().isTowered = false;
        Value.TGround.GetComponent<TowerGround>().UpdateVisibility(false);

        gameMode.GameTowers.Remove(gameObject);
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "TowerGround")
        {
            onGround = true;
            Value.TGround = col.gameObject;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "TowerGround")
        {
            onGround = false;
        }
    }
}
