using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class mainMenu : MonoBehaviour {

    private Settings settings;
    private MusicPlayer soundCntr;

    //track panel
    public Scrollbar scrollBar;
    public Text trackNametext;
    public float textSpeed = 1.5f;
    private string lasttrackName;
    private float textTime;
    private int i = 0;

    //main buttons
    public GameObject butt_1;
    public GameObject butt_2;
    public GameObject butt_3;
    public GameObject butt_4;

    //levels panel
    public GameObject lvlsPanel;

    //options Panel
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

    //load panel
    public GameObject loadPanel;
    public Image loadProgress;

    void Start()
    {
        settings = transform.gameObject.GetComponent<Settings>();
        soundCntr = GameObject.FindObjectOfType<MusicPlayer>();

        textTime = textSpeed;

        butt_1.SetActive(true);
        butt_2.SetActive(true);
        butt_3.SetActive(true);
        butt_4.SetActive(true);

        loadPanel.SetActive(false);
        lvlsPanel.SetActive(false);
        optionsPanel.SetActive(false);

        string[] names = QualitySettings.names;
        dropD_qualityLable.text = names[settings.GetGraphicsQuality()];
        dropD_quality.value = settings.GetGraphicsQuality();
        scrollGenVol.value = settings.GetGeneralVolume();
        genVolumeText.text = Mathf.Round(settings.GetGeneralVolume() * 100).ToString();
        scrollMusVol.value = settings.GetMusiclVolume();
        musicVolumeText.text = Mathf.Round(settings.GetMusiclVolume() * 100).ToString();
        scrollEffVol.value = settings.GetEffectsVolume();
        effVolumeText.text = Mathf.Round(settings.GetEffectsVolume() * 100).ToString();

        scrollBar.value = settings.GetMusiclVolume();
    }

    void LateUpdate()
    {
        soundCntr.PlayBG();
        TickerTrackName();

        if(Input.GetButtonDown("Cancel"))
        {
            optionsPanel.SetActive(false);
            lvlsPanel.SetActive(false);

            butt_1.SetActive(true);
            butt_2.SetActive(true);
            butt_3.SetActive(true);
            butt_4.SetActive(true);
        }
    }

    public void OnLevelsButton()
    {
        butt_1.SetActive(false);
        butt_2.SetActive(false);
        butt_3.SetActive(false);
        butt_4.SetActive(false);

        lvlsPanel.SetActive(true);
    }

    public void OnCloseLevelsButton()
    {
        butt_1.SetActive(true);
        butt_2.SetActive(true);
        butt_3.SetActive(true);

        lvlsPanel.SetActive(false);
    }

    public void OnOptionsButton()
    {
        optionsPanel.SetActive(true);
        textOption.text = "Graphics";
        dropD_quality.gameObject.SetActive(true);
        scrollGenVol.gameObject.SetActive(false);
        scrollMusVol.gameObject.SetActive(false);
        scrollEffVol.gameObject.SetActive(false);

        butt_1.SetActive(false);
        butt_2.SetActive(false);
        butt_3.SetActive(false);
        butt_4.SetActive(false);
    }

    public void OnOptionsPageButton(int val)
    {
        if(val < 0)
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
        scrollBar.value = scrollMusVol.value;
        musicVolumeText.text = Mathf.Round(scrollMusVol.value * 100).ToString();
    }

    public void OnSetEffectsVol()
    {
        settings.SetEffectsVolume(scrollEffVol.value);
        effVolumeText.text = Mathf.Round(scrollEffVol.value * 100).ToString();
    }

    public void LoadLevel(int _val)
    {
        StartCoroutine("LoadScene", _val);
    }

    private IEnumerator LoadScene(int _val)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(_val);

        while (async.isDone == false)
        {
            loadPanel.SetActive(true);
            loadProgress.fillAmount = async.progress;

            butt_1.SetActive(false);
            butt_2.SetActive(false);
            butt_3.SetActive(false);

            yield return true;
        }
    }

    public void OnMuteButton()
    {
        soundCntr.Pause();
    }

    public void OnChangeVol()
    {
        settings.SetMusicVolume(scrollBar.value);

        scrollMusVol.value = scrollBar.value;
        musicVolumeText.text = Mathf.Round(scrollMusVol.value * 100).ToString();
    }

    public void OnChangeTrack(int _val)
    {
        soundCntr.ChangeTrack(_val);
    }

    public void TickerTrackName()
    {
        if (lasttrackName != soundCntr.GetTrackName())
        {
            lasttrackName = soundCntr.GetTrackName();
            i = 0;
        }

        if (soundCntr.GetTrackName().Length >= 21)
        {
            trackNametext.text = soundCntr.GetTrackName().Substring(i, 20);
            if (textTime <= 0)
            {
                i++;
                if (i > soundCntr.GetTrackName().Length - 20)
                {
                    i = 0;
                    textTime = textSpeed * 7;
                }
                else if (i == soundCntr.GetTrackName().Length - 20)
                    textTime = textSpeed * 7;
                else
                    textTime = textSpeed;
            }
            else
            {
                textTime -= Time.deltaTime;
            }
        }
        else
        {
            trackNametext.text = soundCntr.GetTrackName();
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
}
