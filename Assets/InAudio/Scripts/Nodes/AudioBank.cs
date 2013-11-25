using System.Collections.Generic;
using InAudio;
using UnityEngine;

namespace InAudio
{
    [System.Serializable]
    public class BankTuple
    {
        public AudioNode Node;
        public AudioClip Clip;
    }
}

public class AudioBank : MonoBehaviour
{
    public int GUID;
    public List<BankTuple> Clips = new List<BankTuple>();
}

