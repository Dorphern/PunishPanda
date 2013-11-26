using System.Collections.Generic;
using UnityEngine;
using System.Collections;

[System.Serializable]
public class AudioEventList
{
    //Be carefull recompiling this class. 
    //If Unitys' serializer does not understand it is the same field, every hook will lose its data
    public List<AudioEvent> Events = new List<AudioEvent>();
}
 