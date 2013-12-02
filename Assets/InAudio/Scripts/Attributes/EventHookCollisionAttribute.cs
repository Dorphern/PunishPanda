using System;
using UnityEngine;
using System.Collections;

public class EventHookCollisionAttribute : PropertyAttribute
{
    public string EventType;
    public bool FoldedOut = false;
    public AudioEventHook EventHook;
    public EventHookCollisionAttribute(string eventType, AudioEventHook eventHook)
    {
        EventHook = eventHook;
        EventType = eventType;
    }

}

