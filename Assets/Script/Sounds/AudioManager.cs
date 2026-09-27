using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Sounds")]
    public Sound[] musicSounds;
    public Sound[] sfxSounds;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        PlayMusic("BGM");
    }

    // =========================================================
    // MUSIC
    // =========================================================

    public void PlayMusic(string name)
    {
        if (musicSource == null)
        {
            Debug.LogWarning("AudioManager: Music AudioSource is missing.");
            return;
        }

        Sound sound = Array.Find(
            musicSounds,
            x => x != null && x.name == name
        );

        if (sound == null)
        {
            Debug.LogWarning("Music not found: " + name);
            return;
        }

        if (sound.audioClip == null)
        {
            Debug.LogWarning("AudioClip missing for: " + name);
            return;
        }

        musicSource.clip = sound.audioClip;
        musicSource.loop = true;
        musicSource.Play();
    }

    // =========================================================
    // SFX
    // =========================================================

    public void PlaySFX(string name)
    {
        if (sfxSource == null)
        {
            Debug.LogWarning("AudioManager: SFX AudioSource is missing.");
            return;
        }

        Sound sound = Array.Find(
            sfxSounds,
            x => x != null && x.name == name
        );

        if (sound == null)
        {
            Debug.LogWarning("SFX not found: " + name);
            return;
        }

        if (sound.audioClip == null)
        {
            Debug.LogWarning("AudioClip missing for: " + name);
            return;
        }

        sfxSource.PlayOneShot(sound.audioClip);
    }

    // =========================================================
    // MUTE
    // =========================================================

    public void ToggleMusic()
    {
        if (musicSource == null)
            return;

        musicSource.mute = !musicSource.mute;
    }

    public void ToggleSFX()
    {
        if (sfxSource == null)
            return;

        sfxSource.mute = !sfxSource.mute;
    }

    // =========================================================
    // VOLUME
    // =========================================================

    public void MusicVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0.0001f, 1f);

        if (audioMixer != null)
        {
            audioMixer.SetFloat(
                "music",
                Mathf.Log10(volume) * 20f
            );
        }
    }

    public void SFXVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0.0001f, 1f);

        if (audioMixer != null)
        {
            audioMixer.SetFloat(
                "SFX",
                Mathf.Log10(volume) * 20f
            );
        }
    }
}