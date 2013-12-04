using System;
using UnityEngine;

namespace InAudio
{
    public enum EventActionTypes
    {
        Play            = 1,
        Stop            = 2,
        Break           = 3, 
        //Pause = 4, //Not implemented
        StopAll         = 5,
        //SetVolume     = 6, //Not implemented
        SetBusVolume    = 7,
        LoadBank        = 8,
        UnloadBank      = 9,
        StopAllInBus    = 10,
        SetBusMute         = 11,
    };
}

public abstract class AudioEventAction : MonoBehaviour
{
    public float Delay;
    public InAudio.EventActionTypes EventActionType;

    public abstract string ObjectName { get; }
}