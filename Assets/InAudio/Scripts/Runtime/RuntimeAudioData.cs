using System.Collections.Generic;
using InAudio;
using UnityEngine;

namespace InAudio
{
    public class RuntimeInfo
    {
        public AudioNode Node;
        public RuntimePlayer Player;
        public List<RuntimeInfo> PlacedIn;
        public int ListIndex;

        public void Set(AudioNode node, RuntimePlayer player)
        {
            Node = node;
            Player = player;
        }
    }
}

public class RuntimeAudioData : MonoBehaviour {
    public Dictionary<int, AudioEvent> Events;

    public AudioEvent GetEvent(int id)
    {
        AudioEvent audioEvent;
        Events.TryGetValue(id, out audioEvent);
        return audioEvent;
    }

    public void UpdateEvents(AudioEvent root)
    {
        Events = new Dictionary<int, AudioEvent>();
        BuildEventSet(root, Events);
    }
 
    void BuildEventSet(AudioEvent audioevent, Dictionary<int, AudioEvent> events)
    {
        if (audioevent.Type != EventNodeType.Folder && audioevent.Type != EventNodeType.Root)
        {
            events[audioevent.GUID] = audioevent;
        }
        for (int i = 0; i < audioevent.Children.Count; ++i)
        {
            BuildEventSet(audioevent.Children[i], events);
        }
    }
}
