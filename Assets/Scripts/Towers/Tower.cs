using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tower{

    public enum atkType
    {
        Target,
        RangeTarget,
        Splash
    }

    public string Name;
    public string Type;
    public int Cost;
    public int UpgradeCost;
    public int Level;
    public int Damage;
    public float Range;
    public float MinRange;
    public float bulletRange;
    public float reloading;
    public float bltSpeed;
    public Texture IconTextureLvl_1;
    public Texture IconTextureLvl_2;
    public Texture IconTexture;
    public GameObject BulletPref;
    public GameObject BulletSpawn;
    public GameObject TGround;

    public atkType attkType;
}
