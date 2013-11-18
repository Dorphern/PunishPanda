using System.Collections.Generic;
using HDRAudio.Runtime;
using UnityEngine;

public class AudioBus : MonoBehaviour, ITreeNode<AudioBus>
{
    //Volume set in the editor
    public float Volume = 1.0f;



    public int GUID;

    public string Name;


    public AudioBus Parent;

    //If we loose the connection, we can rebuild
    public int ParentGUID;

    public List<AudioBus> Children = new List<AudioBus>();

    //Do we need to update the attach audio players?
    [System.NonSerialized]
    public bool Dirty = true;

    //The nodes during runtime that is in this bus
    [System.NonSerialized]
    public List<RuntimePlayer> NodesInBus = new List<RuntimePlayer>();

    //The volume to set it's children to
    [System.NonSerialized]
    public float RuntimeVolume = 1.0f;

    //What the volume for itself is
    [System.NonSerialized]
    public float RuntimeSelfVolume = 1.0f;

    //The volume in the hiarchy
    [System.NonSerialized]
    public float CombinedVolume = 1.0f;


    [System.NonSerialized]
    public Fader Fader = new Fader();


#if UNITY_EDITOR
    public bool FoldedOut;

    public bool Filtered = false;
#endif

    public List<RuntimePlayer> GetRuntimePlayers()
    {
        if (NodesInBus == null)
            NodesInBus = new List<RuntimePlayer>();
        return NodesInBus;
    }

    public AudioBus GetParent
    {
        get { return Parent; }
        set { Parent = value; }
    }

    public List<AudioBus> GetChildren
    {
        get { return Children; }
    }


    public string GetName
    {
        get { return Name; }
    }

    public bool IsRoot
    {
        get { return Parent == null; }
    }

    public int ID
    {
        get { return GUID; }
        set { GUID = value; }
    }

    
    #if UNITY_EDITOR
    public bool IsFoldedOut
    {
        get
        {
            return FoldedOut;
        }
        set
        {
            FoldedOut = value;
        }
    }

    public bool IsFiltered
    {
        get
        {
            return Filtered;
        }
        set
        {
            Filtered = value;
        }
    }
    #endif
}
