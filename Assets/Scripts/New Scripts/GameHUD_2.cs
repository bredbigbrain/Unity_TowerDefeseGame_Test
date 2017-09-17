using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameHUD_2 : MonoBehaviour {

    public GameObject shopPanel;

    public Animator animShop;

	void Start () {
		
	}

	void Update ()
    {
		
	}

    public void OnShopButton()
    {
        animShop.SetBool("Show", !animShop.GetBool("Show"));
    }

    public void LoadLevel(int _val)
    {
        SceneManager.LoadScene(_val);
    }
}
