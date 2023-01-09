using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : GenericSingletonClass<AudioManager>
{
    public AudioMixer _MasterMixer;
    public AudioSource playerSource;
    public AudioClip digClip;
    public AudioClip typingClip;
    public AudioClip pourClip;
    public AudioClip buyClip;
    public AudioClip harvestClip;

    public void SetMasterVolume(Slider volume){
        _MasterMixer.SetFloat ("MasterVolume", volume.value);
    }

    public void SetBGMVolume(Slider volume){
        _MasterMixer.SetFloat ("BGMVolume", volume.value);
    }

    public void SetSFXVolume(Slider volume){
        _MasterMixer.SetFloat ("SFXVolume", volume.value);
    }

    public void PlayClip(AudioClip clip)
    {
        playerSource.clip = clip;
        playerSource.Play();
    }

    public void PlayDigClip()
    {
        PlayClip(digClip);
    }

    public void PlayTypingClip()
    {
        PlayClip(typingClip);
    }

    public void PlayPouringClip()
    {
        PlayClip(pourClip);
    }

    public void PlayBuyClip()
    {
        PlayClip(buyClip);
    }

    public void PlayHarvestClip()
    {
        PlayClip(harvestClip);
    }
}
