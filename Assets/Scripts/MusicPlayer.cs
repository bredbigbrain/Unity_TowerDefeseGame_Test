using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour {

    public AudioClip[] backgroundMusic;
    private AudioSource sourse;

    private bool pause = false;
    private int currTrack;

    void Start () {
        sourse = transform.gameObject.GetComponent<AudioSource>();
        backgroundMusic = Resources.LoadAll<AudioClip>("Audio/Background");

        currTrack = backgroundMusic.Length;
    }
	
    public void PlayBG()
    {
        if (!sourse.isPlaying && !pause)
        {
            currTrack--;
            if (currTrack < 0)
                currTrack = backgroundMusic.Length - 1;

            sourse.clip = backgroundMusic[currTrack];
            sourse.Play();
        }
    }

    public void Pause()
    {
        pause = !pause;

        if (pause)
            sourse.Pause();
        else
        {
            sourse.UnPause();
        }
    }

    public void ChangeTrack(int _val)
    {
        if (_val >= 0)
        {
            currTrack++;
            if (currTrack == backgroundMusic.Length)
                currTrack = 0;
        }
        else
        {
            currTrack--;
            if (currTrack < 0)
                currTrack = backgroundMusic.Length - 1;
        }

        sourse.clip = backgroundMusic[currTrack];
        sourse.Play();
    }

    public void SetVolume(float _vol)
    {
        sourse.volume = _vol;
    }

    public string GetTrackName()
    {
        return backgroundMusic[currTrack].name;
    }
}
