using System;
using UnityEngine;
using System.Collections;

public class EventHookAttribute : PropertyAttribute
{
    public string EventType;
    public bool FoldedOut = false;
    public EventHookAttribute(string eventType)
    {
        EventType = eventType;
    }

}

