using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameHUD : MonoBehaviour {

    private GameMode gameMode;
    private MusicPlayer soundCntrl;
    public Settings settings;

    public Canvas UI;

    [Header("Royal gun")]
    public Image reloadProgressImage;
    public RoyalGun royalGun;
    public Text pressRText;
    public Text rG_isreadyText;

    [Header("Bottom panel")]
    public Text DiamondText;
    public Text GameTimeText;
    public Text TextWave;

    [Header("Description panel")]
    public GameObject selectedObj;
    public GameObject descriptionPanel;
    private bool upPreview = false;

    public RawImage imageIcon;
    public Text textName;
    public Text textType;
    public Text textRange;
    public Text textReloadTime;
    public Text textDamage;

    public Text textName_;
    public Text textType_;
    public Text textRange_;
    public Text textReloadTime_;
    public Text textDamage_;

    public GameObject upgradeButt;
    public Text upgraeCostText;
    public GameObject sellButt;
    public GameObject OkButton;
    public GameObject costPanel;


    [Header("Main menu panel")]
    public GameObject mainMenuPanel;
    public Text menuText;
    public Text menuTextBack;
    public GameObject nextLvlBtn;
    public GameObject cencelBtn;
    public Scrollbar soundScrollBar;
    public Text trackNametext;
    public float textSpeed = 1.5f;
    private int i = 0;
    private float textTime;
    private string lasttrackName;

    [Header("Options panel")]
    public GameObject optionsPanel;
    public Text textOption;
    public Scrollbar scrollGenVol;
    public Text genVolumeText;
    public Scrollbar scrollMusVol;
    public Text musicVolumeText;
    public Scrollbar scrollEffVol;
    public Text effVolumeText;
    public Dropdown dropD_quality;
    public Text dropD_qualityLable;
    private bool optionsPanelActive = false;

    [Header("Shop panel")]
    public GameObject shopPanel;
    public Animator animShop;

    [Header("Load panel")]
    public GameObject loadPanel;
    public Image loadProgress;


    void Start()
    {
        settings = transform.gameObject.GetComponent<Settings>();
        soundCntrl = GameObject.FindObjectOfType<MusicPlayer>();
        gameMode = transform.GetComponent<GameMode>();

        UI.gameObject.SetActive(true);

        loadPanel.SetActive(false);
        mainMenuPanel.gameObject.SetActive(false);
        optionsPanel.SetActive(false);

        rG_isreadyText.gameObject.SetActive(false);
        pressRText.gameObject.SetActive(royalGun.loaded);

        descriptionPanel.gameObject.SetActive(false);
        costPanel.SetActive(false);
        OkButton.SetActive(false);

        textTime = textSpeed;

        string[] names = QualitySettings.names;
        dropD_qualityLable.text = names[settings.GetGraphicsQuality()];
        dropD_quality.value = settings.GetGraphicsQuality();
        scrollGenVol.value = settings.GetGeneralVolume();
        genVolumeText.text = Mathf.Round(settings.GetGeneralVolume() * 100).ToString();
        scrollMusVol.value = settings.GetMusiclVolume();
        musicVolumeText.text = Mathf.Round(settings.GetMusiclVolume() * 100).ToString();
        scrollEffVol.value = settings.GetEffectsVolume();
        effVolumeText.text = Mathf.Round(settings.GetEffectsVolume() * 100).ToString();

        soundScrollBar.value = settings.GetMusiclVolume();
    }

    void Update()
    {
        ListenKeyboard();
    }

    void LateUpdate()
    {
        DiamondText.text = gameMode.Diamonds.ToString();
        TextWave.text = (gameMode.CurrWave + 1).ToString();
        GameTimeText.text = gameMode.timeM.ToString() + ":" + gameMode.timeS.ToString();

        soundCntrl.PlayBG();
        RoyalGunLoading();

        if(gameMode.pause)
            TickerTrackName();

        if ((selectedObj != null) & descriptionPanel.activeSelf)
        {
            if (Input.GetMouseButton(1))
            {
                HideDescription();
            }
        }
    }

    public void UpdateDescription()
    {
        if (animShop.GetBool("Show"))
        {
            animShop.SetBool("Show", false);
        }
        if (selectedObj.tag == "Tower")
        {
            textName.text = selectedObj.GetComponent<InfoTower>().Value.Name;
            textType_.text = "Type:";
            textType.text = selectedObj.GetComponent<InfoTower>().Value.Type;
            textDamage_.text = "Damage:";
            textDamage.text = selectedObj.GetComponent<InfoTower>().Value.Damage.ToString();
            textRange_.text = "Range:";
            textRange.text = selectedObj.GetComponent<InfoTower>().Value.Range.ToString();
            textReloadTime.text = selectedObj.GetComponent<InfoTower>().Value.reloading.ToString();
            textReloadTime_.gameObject.SetActive(true);
            textReloadTime.gameObject.SetActive(true);
            imageIcon.texture = selectedObj.GetComponent<InfoTower>().Value.IconTexture;


            upgradeButt.SetActive(true);
            sellButt.SetActive(true);
            OkButton.SetActive(false);
            costPanel.SetActive(false);
            cencelBtn.SetActive(true);
            descriptionPanel.SetActive(true);

            upPreview = false;
        }
        else
        {
            textName.text = selectedObj.GetComponent<InfoUnit_1>().Value.Name;
            textType_.text = "Health:";
            textType.text = selectedObj.GetComponent<InfoUnit_1>().Value.CurHealth + "/" + selectedObj.GetComponent<InfoUnit_1>().Value.Health;
            textDamage_.text = "Speed:";
            textDamage.text = selectedObj.GetComponent<InfoUnit_1>().Value.Speed.ToString();
            textRange_.text = "Cost:";
            textRange.text = selectedObj.GetComponent<InfoUnit_1>().Value.Diamonds.ToString();
            textReloadTime_.gameObject.SetActive(false);
            textReloadTime.gameObject.SetActive(false);
            imageIcon.texture = selectedObj.GetComponent<InfoUnit_1>().Value.IconTexture;


            upgradeButt.SetActive(false);
            sellButt.SetActive(false);
            OkButton.SetActive(false);
            costPanel.SetActive(false);
            cencelBtn.SetActive(true);
            descriptionPanel.SetActive(true);
        }
    }

    public void HideDescription()
    {
        if (!upPreview)
        {
            descriptionPanel.SetActive(false);
            if(selectedObj != null)
                switch (selectedObj.tag)
                {
                    case "Tower":
                        selectedObj.GetComponent<InfoTower>().sphereRadius.SetActive(false);
                        break;
                    case "Unit":
                        selectedObj.GetComponent<InfoUnit_1>().sphereRadius.SetActive(false);
                        break;
                }
            selectedObj = null;
        }
        else
        {
            sellButt.SetActive(true);
            OkButton.SetActive(false);
            costPanel.SetActive(false);

            upPreview = false;

            UpdateDescription();
            selectedObj.GetComponent<InfoTower>().SetVisibleRange(selectedObj.GetComponent<InfoTower>().Value.Range);
        }
    }

    public void UpgradePreview()
    {
        upPreview = true;

        sellButt.SetActive(false);
        OkButton.SetActive(true);
        costPanel.SetActive(true);
        upgraeCostText.text = selectedObj.GetComponent<InfoTower>().Value.UpgradeCost.ToString();

        float[] stats = selectedObj.GetComponent<InfoTower>().NextLvlStats();
        textDamage.text = stats[0].ToString();
        textRange.text = stats[1].ToString();
        selectedObj.GetComponent<InfoTower>().SetVisibleRange(stats[1]);
    }

    public void Upgrade()
    {
        selectedObj.GetComponent<InfoTower>().Upgrade();

        sellButt.SetActive(true);
        OkButton.SetActive(false);
        costPanel.SetActive(false);

        upPreview = false;

        UpdateDescription();
    }

    public void Sell()
    {
        selectedObj.GetComponent<InfoTower>().Sell();
        selectedObj = null;
        HideDescription();
    }

    public void Pause(bool _value)
    {
        menuText.text = "Menu";
        menuTextBack.text = "Menu";
        mainMenuPanel.SetActive(_value);
        nextLvlBtn.SetActive(false);
        cencelBtn.SetActive(true);

        trackNametext.text = soundCntrl.GetTrackName();
    }

    public void EndGame()
    {
        if (!gameMode.mainCam.enabled)
        {
            gameMode.ChangeCam();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        menuText.text = "Game Over";
        menuTextBack.text = "Game Over";
        mainMenuPanel.SetActive(true);
        nextLvlBtn.SetActive(false);
        cencelBtn.SetActive(false);
        selectedObj = null;
    }

    public void WinGame()
    {
        if (!gameMode.mainCam.enabled)
        {
            gameMode.ChangeCam();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        menuText.text = "Win!";
        menuTextBack.text = "Win!";
        mainMenuPanel.SetActive(true);
        nextLvlBtn.SetActive(true);
        cencelBtn.SetActive(false);
        selectedObj = null;
    }

    public void ExitToMenu()
    {
        StartCoroutine("LoadScene", 0);
    }

    public void Restart()
    {
        StartCoroutine("LoadScene", 1);
    }

    public void NextLvl(int _lvl)
    {
        StartCoroutine("LoadScene", _lvl);
    }

    private IEnumerator LoadScene(int _lvl)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(_lvl);

        while (async.isDone == false)
        {
            loadPanel.SetActive(true);
            loadProgress.fillAmount = async.progress;

            yield return true;
        }
    }

    public void OnShopButton(int _idTower)
    {
        gameMode.shopButCliked = true;
        gameMode.testObj = null;
        gameMode.idShopTower = _idTower;
    }

    public void OnShopPanelButton()
    {
        animShop.SetBool("Show", !animShop.GetBool("Show"));
        HideDescription();
    }

    public void OnMuteButton()
    {
        soundCntrl.Pause();
    }

    public void OnChangeMusicVol()
    {
        settings.SetMusicVolume(soundScrollBar.value);
        scrollMusVol.value = soundScrollBar.value;
    }

    public void OnChangeTrack(int _val)
    {
        soundCntrl.ChangeTrack(_val);
    }

    public void OnOptionsButton()
    {
        optionsPanel.SetActive(true);
        optionsPanelActive = true;
        textOption.text = "Graphics";
        dropD_quality.gameObject.SetActive(true);
        scrollGenVol.gameObject.SetActive(false);
        scrollMusVol.gameObject.SetActive(false);
        scrollEffVol.gameObject.SetActive(false);
    }

    public void OnOptionsPageButton(int val)
    {
        if (val < 0)
        {
            textOption.text = "Graphics";
            dropD_quality.gameObject.SetActive(true);

            scrollGenVol.gameObject.SetActive(false);
            scrollMusVol.gameObject.SetActive(false);
            scrollEffVol.gameObject.SetActive(false);
        }
        else
        {
            textOption.text = "Audio";
            dropD_quality.gameObject.SetActive(false);

            scrollGenVol.gameObject.SetActive(true);
            scrollMusVol.gameObject.SetActive(true);
            scrollEffVol.gameObject.SetActive(true);
        }
    }

    public void OnSetGraphicsQuality()
    {
        settings.SetGraphicsQuality(dropD_quality.value);

        string[] names = QualitySettings.names;
        dropD_qualityLable.text = names[dropD_quality.value];
    }

    public void OnSetGeneralVol()
    {
        settings.SetGeneralVolume(scrollGenVol.value);
        genVolumeText.text = Mathf.Round(scrollGenVol.value * 100).ToString();
    }

    public void OnSetMusicVol()
    {
        settings.SetMusicVolume(scrollMusVol.value);
        soundScrollBar.value = scrollMusVol.value;
        musicVolumeText.text = Mathf.Round(scrollMusVol.value * 100).ToString();
    }

    public void OnSetEffectsVol()
    {
        settings.SetEffectsVolume(scrollEffVol.value);
        effVolumeText.text = Mathf.Round(scrollEffVol.value * 100).ToString();
    }

    public void RoyalGunLoading()
    {
        reloadProgressImage.fillAmount = (Mathf.Clamp(gameMode.kills, 0.001f, 3f) / 3);
        rG_isreadyText.gameObject.SetActive(royalGun.loaded);
        if (gameMode.mainCam.enabled)
            pressRText.gameObject.SetActive(royalGun.loaded);
    }

    public void TickerTrackName()
    {
        if (lasttrackName != soundCntrl.GetTrackName())
        {
            lasttrackName = soundCntrl.GetTrackName();
            i = 0;
        }

        if (soundCntrl.GetTrackName().Length >= 21)
        {
            trackNametext.text = soundCntrl.GetTrackName().Substring(i, 20);
            if (textTime <= 0)
            {
                i++;
                if (i > soundCntrl.GetTrackName().Length - 20)
                {
                    i = 0;
                    textTime = textSpeed * 7;
                }
                else if (i == soundCntrl.GetTrackName().Length - 20)
                    textTime = textSpeed * 7;
                else
                    textTime = textSpeed;
            }
            else
            {
                textTime -= Time.unscaledDeltaTime;
            }
        }
        else
        {
            trackNametext.text = soundCntrl.GetTrackName();
        }
    }

    private void ListenKeyboard()
    {
        //pause
        if (Input.GetButtonUp("Cancel") & optionsPanelActive)
        {
            optionsPanelActive = false;
            optionsPanel.SetActive(false);
        }
        else if (Input.GetButtonUp("Cancel") & (gameMode.mainCam.enabled))
        {
            gameMode.Pause(!gameMode.pause);
        }
        
            // switch cam Esc/R
            if ((Input.GetButtonUp("Cancel")) & (!gameMode.mainCam.enabled))
        {
            pressRText.text = "PRESS 'R'";

            gameMode.ChangeCam();

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if ((Input.GetKey(KeyCode.R)) & (gameMode.mainCam.enabled == true) & royalGun.loaded)
        {
            if (animShop.GetBool("Show"))
            {
                animShop.SetBool("Show", false);
            }
            pressRText.text = "PRESS 'Esc'";

            selectedObj = null;
            descriptionPanel.SetActive(false);

            gameMode.ChangeCam();
            gameMode.shopButCliked = false;
            foreach (GameObject obj in gameMode.GroundTower)
            {
                obj.GetComponent<TowerGround>().UpdateVisibility(false);
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
