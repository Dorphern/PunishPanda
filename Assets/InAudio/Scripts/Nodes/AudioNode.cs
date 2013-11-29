using System;
using UnityEngine;
using System.Collections.Generic;
using InAudio;

[AddComponentMenu(FolderSettings.ComponentPathInternalData+"Audio/Audio Node")]
public class AudioNode : MonoBehaviour, ITreeNode<AudioNode>
{
    public int GUID;

    public AudioNodeType Type;

    public NodeTypeData NodeData;
  
    //If we loose the connection, we can rebuild 
    public int ParentGUID;

    public string Name;

    public AudioNode Parent;

    public bool OverrideParentBus;
    public AudioBus Bus;

    public bool OverrideParentBank;
    public AudioBankLink BankLink;

    public List<AudioNode> Children = new List<AudioNode>();

#if UNITY_EDITOR
    public bool Filtered = false;

    public bool FoldedOut;

#endif

    public bool LimitInstances;

    public int MaxInstances;

    public InstanceStealingTypes InstanceStealingTypes;

    [NonSerialized]
    public List<InstanceInfo> CurrentInstances = new List<InstanceInfo>(0);

   
    public AudioNode GetParent
    {
        get { return Parent; }
        set { Parent = value; }
    }

    public List<AudioNode> GetChildren
    {
        get { return Children; }
    }

    public string GetName
    {
        get { return Name; }
    }

    public bool IsRoot
    {
        get { return Type == AudioNodeType.Root; }
    }

    public int ID
    {
        get { return GUID; }
        set { GUID = value; }
    }

    public bool IsPlayable
    {
        get { return Type != AudioNodeType.Root && Type != AudioNodeType.Folder; }
    }

#if UNITY_EDITOR
    public bool IsFoldedOut
    {
        get { return FoldedOut; }
        set { FoldedOut = value; }
    }

    public bool IsFiltered
    {
        get { return Filtered; }
        set { Filtered = value; }
    }
#endif
}

namespace InAudio
{
    public struct InstanceInfo
    {
        public double Timestamp;
        public RuntimePlayer Player;

        public InstanceInfo(double timestamp, RuntimePlayer player)
        {
            Timestamp = timestamp;
            Player = player;
        }
    }

    public enum InstanceStealingTypes
    {
        NoStealing = 0,
        Oldest = 1, 
        Newest = 2,
    }

    public enum AudioNodeType
    {
        Root = 0,
        Folder = 1,
        Audio = 2,
        Random = 3,
        Sequence = 4, 
        Voice = 5,
        Multi = 6,
        Track = 7,
    }
}
