using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerGround : MonoBehaviour
{
    public bool isTowered = false;
    
    public GameObject basement;


    public void UpdateVisibility(bool _shopButtonCliked)
    {
        if (isTowered)
        {
            basement.SetActive(true);
            GetComponent<CapsuleCollider>().enabled = true;
        }
        else
        {
            if (_shopButtonCliked)
            {
                basement.SetActive(true);
                GetComponent<CapsuleCollider>().enabled = true;
            }
            else
            {
                basement.SetActive(false);
                GetComponent<CapsuleCollider>().enabled = false;
            }
        }
    }
}
