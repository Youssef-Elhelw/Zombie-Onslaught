using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // Singleton for easy access

    [Header("---- Audio Sources ----")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource; // Dedicated SFX AudioSource
    public List<AudioSource> activeSfxSources = new List<AudioSource>(); // For multiple sounds

    [Header("---- Audio Clips ----")]
    public AudioClip shootingA;
    public AudioClip shootingB;
    public AudioClip gainMoney;
    public AudioClip purchaseTurret;
    public AudioClip death;
    public AudioClip lose;
    public AudioClip buildingSound;
    public AudioClip zombieSound;
    public AudioClip BossDies;
    public AudioClip Victory;
    public AudioClip applaus;
    public AudioClip UpgradeSound;
    public List<AudioClip> nextRoundSounds = new List<AudioClip>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;

        if (clip == shootingB)
        {
            foreach (var source in activeSfxSources)
            {
                if (source != null && source.clip == shootingB && source.isPlaying)
                    return;
            }
        }

        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        newSource.clip = clip;
        newSource.volume = volume;
        newSource.Play();

        activeSfxSources.Add(newSource);
        Destroy(newSource, clip.length);
    }


    public void StopAllSFX()
    {
        foreach (var source in activeSfxSources)
        {
            Destroy(source);
        }
        activeSfxSources.Clear();
    }

    public AudioSource GetSFXSource()
    {
        return sfxSource;
    }
}
