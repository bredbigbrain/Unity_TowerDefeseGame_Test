using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour{

    private static Settings settings;
    private List<AudioSource> soursesEffects = new List<AudioSource>();

    void Awake()
    {
        if (settings != null)
            Destroy(settings);
        else
            settings = this;
    }

    void Start()
    {
        foreach (AudioSource aud in GameObject.FindObjectsOfType<AudioSource>())
        {
            if (aud.gameObject.tag != "Music")
                soursesEffects.Add(aud);
        }
        ApplySettings();
    }

    public void ApplySettings()
    {
        if (!PlayerPrefs.HasKey("GraphicsQuality"))
            PlayerPrefs.SetInt("GraphicsQuality", 1);
        if (!PlayerPrefs.HasKey("GeneralVolume"))
            PlayerPrefs.SetFloat("GeneralVolume", 1);
        if (!PlayerPrefs.HasKey("MusicVolume"))
            PlayerPrefs.SetFloat("MusicVolume", 1);
        if (!PlayerPrefs.HasKey("EffectsVolume"))
            PlayerPrefs.SetFloat("EffectsVolume", 1);

        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("GraphicsQuality"), true);
        AudioListener.volume = PlayerPrefs.GetFloat("GeneralVolume");
        if (GameObject.FindGameObjectWithTag("Music"))
            GameObject.FindGameObjectWithTag("Music").GetComponent<MusicPlayer>().SetVolume(PlayerPrefs.GetFloat("MusicVolume"));
        if (soursesEffects.Count > 0)
            foreach (AudioSource AS in soursesEffects)
                AS.volume = PlayerPrefs.GetFloat("EffectsVolume");
    }

    public void AddEffectAudioSourse(AudioSource AS)
    {
        AS.volume = PlayerPrefs.GetFloat("EffectsVolume");
        soursesEffects.Add(AS);
    }

    public void SetGraphicsQuality(int quality)
    {
        PlayerPrefs.SetInt("GraphicsQuality", quality);
        QualitySettings.SetQualityLevel(quality, true);
    }

    public int GetGraphicsQuality()
    {
        return PlayerPrefs.GetInt("GraphicsQuality");
    }

    public float GetGeneralVolume()
    {
        return PlayerPrefs.GetFloat("GeneralVolume");
    }

    public void SetGeneralVolume(float vol)
    {
        PlayerPrefs.SetFloat("GeneralVolume", vol);
        AudioListener.volume = PlayerPrefs.GetFloat("GeneralVolume");
    }

    public void SetMusicVolume(float vol)
    {
        PlayerPrefs.SetFloat("MusicVolume", vol);
        if (GameObject.FindGameObjectWithTag("Music"))
            GameObject.FindGameObjectWithTag("Music").GetComponent<MusicPlayer>().SetVolume(vol);
    }

    public float GetMusiclVolume()
    {
        return PlayerPrefs.GetFloat("MusicVolume");
    }

    public void SetEffectsVolume(float vol)
    {
        PlayerPrefs.SetFloat("EffectsVolume", vol);
        if (soursesEffects.Count > 0)
            foreach (AudioSource AS in soursesEffects)
                AS.volume = vol;
    }

    public float GetEffectsVolume()
    {
        return PlayerPrefs.GetFloat("EffectsVolume");
    }
}
