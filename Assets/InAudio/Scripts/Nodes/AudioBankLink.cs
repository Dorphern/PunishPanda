using System.Collections.Generic;
using InAudio;
using UnityEngine;
using System.Collections;

namespace InAudio
{

    public enum AudioBankTypes
    {
        Folder, Link
   }
}

public class AudioBankLink : MonoBehaviour, ITreeNode<AudioBankLink>
{
    public int GUID;

    public AudioBankTypes Type;

    public string Name;

    public AudioBankLink Parent;

    public List<AudioBankLink> Children = new List<AudioBankLink>();

    public bool AutoLoad = false;

    [System.NonSerialized]
    public AudioBank LoadedBank;

#if UNITY_EDITOR
    public bool FoldedOut;

    public bool Filtered = false;
#endif

    public AudioBank LazyBankFetch
    {
        get
        {
            if (LoadedBank == null)
            {
                LoadedBank = BankLoader.Load(this);
            }

            return LoadedBank;
        }
    }

    public bool IsLoaded
    {
        get; set;
    }

    public AudioBankLink GetParent
    {
        get
        {
            return Parent;
        }
        set
        {
            Parent = value;
        }
    }

    public List<AudioBankLink> GetChildren
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
