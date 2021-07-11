using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public bool ismusicEnabled = true;
    public AudioSource musicSource;
    public AudioClip deathSound;
    public AudioClip coinSound;
    public AudioClip pageSound;
    public AudioClip birdWingSound;

    [Range(0, 1)]
    public float fxVolume = 1f;

    [Range(0, 1)]
    public float musicVolume = 1f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public void PlayBackgroundMusic(AudioClip musicClip)
    {
        if (!musicSource || !ismusicEnabled || !musicClip)
        {
            return;
        }
        musicSource.Stop();
        musicSource.clip = musicClip;
        musicSource.volume = 0.2f;
        musicSource.loop = true;
        musicSource.Play();
    }
    void CheckMusicUpdate()
    {
        if (musicSource.isPlaying != ismusicEnabled)
        {
            if (ismusicEnabled)
            {
                //PlayBackgroundMusic();
            }
            else
            {
                musicSource.Stop();
            }
        }
    }
    public void PlaySound(AudioClip clip, float volume)
    {
        if (clip)
        {
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, Mathf.Clamp(fxVolume * volume, 0.05f, 1f));
        }

    }
}
