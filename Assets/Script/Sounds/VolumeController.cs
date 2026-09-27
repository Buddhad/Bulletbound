using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        LoadVolume();
    }

    // =========================================================
    // MUSIC
    // =========================================================

    public void SetMusicVolume()
    {
        if (musicSlider == null || audioMixer == null)
            return;

        float volume = Mathf.Clamp(
            musicSlider.value,
            0.0001f,
            1f
        );

        audioMixer.SetFloat(
            "music",
            Mathf.Log10(volume) * 20f
        );

        PlayerPrefs.SetFloat(
            "musicVolume",
            volume
        );

        PlayerPrefs.Save();
    }

    // =========================================================
    // SFX
    // =========================================================

    public void SetSFXVolume()
    {
        if (sfxSlider == null || audioMixer == null)
            return;

        float volume = Mathf.Clamp(
            sfxSlider.value,
            0.0001f,
            1f
        );

        audioMixer.SetFloat(
            "SFX",
            Mathf.Log10(volume) * 20f
        );

        PlayerPrefs.SetFloat(
            "SFXVolume",
            volume
        );

        PlayerPrefs.Save();
    }

    // =========================================================
    // LOAD
    // =========================================================

    public void LoadVolume()
    {
        float musicVolume =
            PlayerPrefs.GetFloat("musicVolume", 1f);

        float sfxVolume =
            PlayerPrefs.GetFloat("SFXVolume", 1f);

        if (musicSlider != null)
            musicSlider.value = musicVolume;

        if (sfxSlider != null)
            sfxSlider.value = sfxVolume;

        SetMusicVolume();
        SetSFXVolume();
    }

    // =========================================================
    // DEFAULT
    // =========================================================

    public void Default()
    {
        if (musicSlider != null)
            musicSlider.value = 1f;

        if (sfxSlider != null)
            sfxSlider.value = 1f;

        SetMusicVolume();
        SetSFXVolume();
    }
}