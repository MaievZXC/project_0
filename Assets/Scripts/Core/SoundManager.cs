using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    private AudioSource soundSource;
    private AudioSource musicSource;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(instance != null && instance != this)
            Destroy(gameObject);
        soundSource = GetComponent<AudioSource>();
        musicSource = transform.GetChild(0).GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);

        //using methods in order to load volume instead of using default value
        ChangeSoundVolume(PlayerPrefs.GetFloat("soundVolume", 1));
        ChangeMusicVolume(PlayerPrefs.GetFloat("musicVolume", 1));
    }

    public void PlaySound(AudioClip _clip)
    {
        soundSource.PlayOneShot(_clip);
    }


    public void ChangeSourseVolume(float change, string sourseName, AudioSource source)
    {
        float currentVolume = change;    
        source.volume = currentVolume;
        PlayerPrefs.SetFloat(sourseName, currentVolume);
    }

    public void ChangeSoundVolume(float change)
    {
        ChangeSourseVolume(change, "soundVolume", soundSource);
    }

    public void ChangeMusicVolume(float change)
    {
        ChangeSourseVolume(change, "musicVolume", musicSource);
    }
}   
