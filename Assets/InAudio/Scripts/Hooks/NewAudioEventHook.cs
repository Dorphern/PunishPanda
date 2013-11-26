using System.Collections.Generic;
using UnityEngine;
using System.Collections;

[AddComponentMenu("InAudio/New Audio Event Hook")]
public class NewAudioEventHook : MonoBehaviour
{
    [EventHookAttribute("On Enable")]
    public AudioEventList OnEnableAudioEvents = new AudioEventList();

    [EventHookAttribute("On Start")]
    public AudioEventList StartAudioEvents = new AudioEventList();

    [EventHookAttribute("On Disable")]
    public AudioEventList OnDisableAudioEvents = new AudioEventList();

    [EventHookAttribute("On Destroy")]
    public AudioEventList OnDestroyAudioEvents = new AudioEventList();

    [EventHookAttribute("On Visible")]
    public AudioEventList OnBecameVisibleAudioEvents = new AudioEventList();

    [EventHookAttribute("On Became Invisible")]
    public AudioEventList OnBecameInvisibleAudioEvents = new AudioEventList();

    void OnEnable()
    {
        HDRSystem.PostEvents(gameObject, OnEnableAudioEvents.Events);
    }

    void Start()
    {
        HDRSystem.PostEvents(gameObject, StartAudioEvents.Events);
    }

    void OnDisable()
    {
        HDRSystem.PostEvents(gameObject, OnDisableAudioEvents.Events);
    }

    void OnDestroy() 
    {
        HDRSystem.PostEvents(gameObject, OnDestroyAudioEvents.Events);
    }

    void OnBecameVisible()
    {
        HDRSystem.PostEvents(gameObject, OnBecameVisibleAudioEvents.Events);
    }

    void OnBecameInvisible()
    {
        HDRSystem.PostEvents(gameObject, OnBecameInvisibleAudioEvents.Events);
    }
}
