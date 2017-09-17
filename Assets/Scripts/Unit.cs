using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Unit {

    public string Name;
    public float Health;
    public float CurHealth;

    public float Speed = 0.7f;
    public int Diamonds = 50;

    public Texture IconTexture;

    public GameObject healthBar;
    public GameObject[] healthParts;
    public Transform[] hParts;

    public void Initialization(string _state)
    {
        switch(_state)
        {
            case "Health":
                CurHealth = Health;
                break;
            default:
                CurHealth = Health;
                break;                    
        }
    }
    
    public void Damage(float _damage)
    {
        CurHealth -= _damage;

        float percHealth = CurHealth / Health;

        int partCount = Mathf.CeilToInt( healthParts.Length * percHealth);

        for (int i = 0; i < Mathf.Clamp( healthParts.Length - partCount - 1, 0, healthParts.Length - 1); i++)
        {
            healthParts[i].SetActive(false);
        }
    }
}
