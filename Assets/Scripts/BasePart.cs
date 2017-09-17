using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePart : MonoBehaviour {

    private MainBase mBase;
    
    void Start()
    {
        mBase = GameObject.FindGameObjectWithTag("Base").GetComponent<MainBase>();
    }
    
    public void OnHitByRG(float _damage)
    {
        mBase.Damage(_damage);
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.collider.gameObject.tag == "Unit")
        {
            mBase.Damage(col.collider.gameObject.GetComponent<InfoUnit_1>().Value.CurHealth);
            col.collider.gameObject.GetComponent<InfoUnit_1>().Value.CurHealth = 0; ;
        }
    }	
}
