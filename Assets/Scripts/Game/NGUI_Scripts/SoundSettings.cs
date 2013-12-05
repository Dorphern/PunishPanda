using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class SoundSettings : MonoBehaviour {

    [SerializeField]    
    [EventHookAttribute("SFX Unmute")]
    List<AudioEvent> onSFXEnable = new List<AudioEvent>();

    [SerializeField]
    [EventHookAttribute("SFX Mute")]
    List<AudioEvent> onSFXDisable = new List<AudioEvent>();

    [SerializeField]
    [EventHookAttribute("Music Unmute")]
    List<AudioEvent> onMusicEnable = new List<AudioEvent>();

    [SerializeField]
    [EventHookAttribute("Music Mute")]
    List<AudioEvent> onMusicDisable = new List<AudioEvent>();

    void Start()
    {
        InstanceFinder.SoundSettings = this;
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
        Debug.Log("SFX Enable");
        HDRSystem.PostEvents(gameObject, onSFXEnable);
        InstanceFinder.StatsManager.SoundEffectsEnabled = true;
    }

    public void OnSFXDisable()
    {
        Debug.Log("SFX Disable");
        HDRSystem.PostEvents(gameObject, onSFXDisable);
        InstanceFinder.StatsManager.SoundEffectsEnabled = false;
    }

    public void OnMusicEnable()
    {
        Debug.Log("Music Enable");
        InstanceFinder.StatsManager.MusicEnabled = true;
        HDRSystem.PostEvents(gameObject, onMusicEnable);
    }

    public void OnMusicDisable()
    {
        InstanceFinder.StatsManager.MusicEnabled = false;
        Debug.Log("Music Disable");
        HDRSystem.PostEvents(gameObject, onMusicDisable);
    }
}
