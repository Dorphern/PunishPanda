using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class SoundSettings : MonoBehaviour {

    [SerializeField]    
    [EventHookAttribute("SFX Enable")]
    List<AudioEvent> onSFXEnable = new List<AudioEvent>();

    [SerializeField]
    [EventHookAttribute("SFX Mute")]
    List<AudioEvent> onSFXDisable = new List<AudioEvent>();

    [SerializeField]
    [EventHookAttribute("Music Enable")]
    List<AudioEvent> onMusicEnable = new List<AudioEvent>();

    [SerializeField]
    [EventHookAttribute("Music Mute")]
    List<AudioEvent> onMusicDisable = new List<AudioEvent>();

    void Start()
    {
        if (InstanceFinder.StatsManager.MusicEnabled)
            OnMusicEnable();
        else
            OnMusicDisable();

        if (InstanceFinder.StatsManager.SoundEffectsEnabled)
            OnSFXEnable();
        else
            OnSFXDisable();
        
    }


    public void OnSFXEnable()
    {
        HDRSystem.PostEvents(gameObject, onSFXEnable);
    }

    public void OnSFXDisable()
    {
        HDRSystem.PostEvents(gameObject, onSFXDisable);
    }

    public void OnMusicEnable()
    {
        HDRSystem.PostEvents(gameObject, onMusicEnable);
    }

    public void OnMusicDisable()
    {
        HDRSystem.PostEvents(gameObject, onMusicDisable);
    }
}
