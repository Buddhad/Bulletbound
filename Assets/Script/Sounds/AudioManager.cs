using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public Sound[] musicSounds, sfxSounds,masterSound;
    public AudioSource musicSource, sfxSource,masterSource;

    private void Awake() {
        if(Instance==null){

            Instance=this;
            DontDestroyOnLoad(gameObject);
        }else{
            Destroy(gameObject);
        }
    }

    private void Start() {
        PlayMusic("BGM");
    }
    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            musicSource.clip = s.audioClip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);
            if (s == null)
            {
                Debug.Log("SFX Not Found");
            }
            else
            {
                sfxSource.PlayOneShot(s.audioClip);
            }
        }
    public void PlayMaster(string name)
    {
        Sound s = Array.Find(masterSound, x => x.name == name);
            if (s == null)
            {
                Debug.Log("Master Not Found");
            }
            else
            {
                masterSource.PlayOneShot(s.audioClip);
            }
        }

    public void ToggleMusic(){
    musicSource.mute=!musicSource.mute;
    }
    public void ToggleSFX(){
        sfxSource.mute=!sfxSource.mute;
    }
    public void ToggleMaster(){
        masterSource.mute=!masterSource.mute;
    }

    public void MusicVolume(float volume){
        musicSource.volume=volume;
    }
    public void SFXVolume(float sfxvolume){
        sfxSource.volume=sfxvolume;
    }
    public void MasterVolume(float masterVolume){
        masterSource.volume=masterVolume;
    }


}
