using System.Collections.Generic;
using InAudio.ExtensionMethods;
using UnityEngine;

namespace InAudio
{

public static class AudioBusWorker
{
    private static AudioBus CreateRoot(GameObject go, int guid)
    {
        var node = go.AddComponent<AudioBus>();
        node.GUID = guid;
        node.FoldedOut = true;
        node.Name = "Root";
        return node;
    }

    public static void RemoveAudioNode(AudioEvent audioevent, int index)
    {
        audioevent.ActionList.RemoveAt(index);
    }

    private static AudioBus CreateBus(GameObject go, AudioBus parent, int guid)
    {
        var node = go.AddComponentUndo<AudioBus>();
        node.GUID = guid;
        node.name = parent.Name + " Child";
        node.AssignParent(parent);
        return node;
    }

    public static AudioBus CreateTree(GameObject go)
    {
        var tree = CreateRoot(go, GUIDCreator.Create());
        return tree;
    }

    public static void DeleteBus(AudioBus bus, AudioNode root)
    {
        HashSet<AudioBus> toDelete = new HashSet<AudioBus>(); 
        GetBussesToDelete(toDelete, bus);

        var runtimePlayers = bus.GetRuntimePlayers();
        for (int i = 0; i < runtimePlayers.Count; ++i)
        {
            runtimePlayers[i].SetNewBus(bus.Parent);
        }

        List<AudioNode> affectedNodes = new List<AudioNode>();
        NodeWorker.FindAllNodes(root, node => toDelete.Contains(node.Bus), affectedNodes);

        for (int i = 0; i < affectedNodes.Count; ++i)
        {
            affectedNodes[i].Bus = bus.Parent;
        }
        bus.Parent.Children.Remove(bus);
        UndoHelper.Destroy(bus);
    }


    private static void GetBussesToDelete(HashSet<AudioBus> toDelete, AudioBus bus)
    {
        toDelete.Add(bus);
        for (int i = 0; i < bus.Children.Count; ++i)
        {
            GetBussesToDelete(toDelete, bus.Children[i]);
        }
    }

    public static AudioBus CreateChild(AudioBus parent)
    {
        var child = CreateBus(parent.gameObject, parent, GUIDCreator.Create());
        child.FoldedOut = true;
        child.Name = parent.Name + " Child";

        return child;
    }

    public static AudioBus GetParentBus(AudioNode node)
    {
        if (node.IsRoot)
            return node.Bus;
        if (node.OverrideParentBus)
            return node.Bus;

        return GetParentBus(node.Parent);
    }
}
}
