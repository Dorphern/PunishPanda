using System;
using System.Collections.Generic;
using System.Linq;
using InAudio.ExtensionMethods;
using UnityEditor;
using UnityEditor.Graphs;
using UnityEngine;
using Object = UnityEngine.Object;

namespace InAudio
{
public static class AudioNodeWorker  {
    public static AudioNode CreateNode(GameObject go, AudioNode parent, int guid, AudioNodeType type)
    {
        var node = go.AddComponentUndo<AudioNode>();
        node.GUID = guid;
        node.Type = type;
        node.Name = parent.Name + " Child";
        node.Bus = parent.Bus;
        node.AssignParent(parent);

        return node;
    }

    public static AudioNode CreateRoot(GameObject go, int guid)
    {
        var node = go.AddComponent<AudioNode>();
        node.GUID = guid;
        node.Type = AudioNodeType.Root;
        node.FoldedOut = true;
        node.Name = "Root";
        return node;
    }

    public static AudioNode CreateTree(GameObject go, int numberOfChildren, AudioBus bus)
    {
        var Tree = CreateRoot(go, GUIDCreator.Create());
        Tree.Bus = bus;
        for (int i = 0; i < numberOfChildren; ++i)
        {
            CreateNode(go, Tree, GUIDCreator.Create(), AudioNodeType.Folder);
        }
        return Tree;
    }

    public static AudioNode CreateNode(GameObject go, AudioNode parent, AudioNodeType type)
    {
        var newNode = CreateNode(go, parent, GUIDCreator.Create(), type);
        AddDataClass(newNode);
        return newNode;
    }

    public static void AddNewDataType(AudioNode node)
    {
        switch (node.Type)
        {
            case AudioNodeType.Audio:
                node.NodeData = node.gameObject.AddComponent<AudioData>();
                break;
            case AudioNodeType.Random:
                node.NodeData = node.gameObject.AddComponent<RandomData>();
                for (int i = 0; i < node.Children.Count; ++i)
                    (node.NodeData as RandomData).weights.Add(50);
                break;
            case AudioNodeType.Sequence:
                node.NodeData = node.gameObject.AddComponent<SequenceData>();
                break;
            case AudioNodeType.Multi:
                node.NodeData = node.gameObject.AddComponent<MultiData>();
                break;
        }
    }

    public static void AddDataClass(AudioNode node)
    {
        switch (node.Type)
        {
            case AudioNodeType.Audio:
                node.NodeData = node.gameObject.AddComponentUndo<AudioData>();
                break;
            case AudioNodeType.Random:
                node.NodeData = node.gameObject.AddComponentUndo<RandomData>();
                for (int i = 0; i < node.Children.Count; ++i)
                    (node.NodeData as RandomData).weights.Add(50);
                break;
            case AudioNodeType.Sequence:
                node.NodeData = node.gameObject.AddComponentUndo<SequenceData>();
                break;
            case AudioNodeType.Multi:
                node.NodeData = node.gameObject.AddComponentUndo<MultiData>();
                break;
        }
    }

    public static void AddNewParent(AudioNode node, AudioNodeType parentType)
    {
        UndoHelper.RecordObjects(new Object[] { node, node.Parent, node.GetBank() }, "Undo Add New Parent for " + node.Name);
        var newParent = CreateNode(node.gameObject, node.Parent, parentType);
        var oldParent = node.Parent;
        newParent.Bus = node.Bus;
        newParent.FoldedOut = true;
        newParent.BankLink = oldParent.GetBank();
        int index = oldParent.Children.FindIndex(node);
        NodeWorker.RemoveFromParent(node);
        node.AssignParent(newParent);

        OnRandomNode(newParent);

        NodeWorker.RemoveFromParent(newParent);
        oldParent.Children.Insert(index, newParent);
    }

    private static void OnRandomNode(AudioNode parent)
    {
        if (parent.Type == AudioNodeType.Random)
            (parent.NodeData as RandomData).weights.Add(50);
    }
     
    public static AudioNode CreateChild(AudioNode parent, AudioNodeType newNodeType)
    {
        UndoHelper.RecordObjects(UndoHelper.Array(parent, parent.NodeData, parent.GetBank()), "Undo Node Creation");
        OnRandomNode(parent);

        var child = CreateNode(parent.gameObject, parent, GUIDCreator.Create(), newNodeType);
        parent.FoldedOut = true;
        child.Name = parent.Name + " Child";
        AddDataClass(child);
        return child;
    }

    public static void ConvertNodeType(AudioNode node, AudioNodeType newType)
    {
        if (!UndoHelper.IsNewUndo)
            UndoHelper.RecordObject(node, "Change Node Type");
        else
        {
            UndoHelper.RegisterFullObjectHierarchyUndo(node.gameObject);
        }
        
        if (newType == node.Type)
            return;

        node.Type = newType;
        AddNewDataType(node);
    }

    public static AudioNode Duplicate(AudioNode audioNode)
    {
        List<Object> toUndo = TreeWalker.FindAll(audioNode, node => node.GetBank().LazyBankFetch).ConvertList<AudioBank, Object>();

        toUndo.Add(audioNode.Parent);
        toUndo.Add(audioNode.GetBank());

        UndoHelper.RecordObjects(toUndo.ToArray(), "Undo Duplication Of " + audioNode.Name);

        if (audioNode.Parent.Type == AudioNodeType.Random)
        {
            (audioNode.Parent.NodeData as RandomData).weights.Add(50);   
        }
        return NodeWorker.DuplicateHierarchy(audioNode, (@oldNode, newNode) =>
        { 
            var gameObject = audioNode.gameObject;
            if(oldNode.NodeData != null)
            { 
                Type type = oldNode.NodeData.GetType();
                newNode.NodeData = gameObject.AddComponent(type) as NodeTypeData;
                EditorUtility.CopySerialized(oldNode.NodeData, newNode.NodeData);
                if (newNode.Type == AudioNodeType.Audio)
                {
                    AudioBankWorker.AddNodeToBank(newNode, (oldNode.NodeData as AudioData).Clip);
                }
            }
        });
    }

    public static void DeleteNode(AudioNode node)
    {
        UndoHelper.RecordObjects(UndoHelper.Array(node.Parent, node.Parent.NodeData, node.GetBank().LazyBankFetch), "Undo Deletion of " + node.Name);
        for (int i = node.Children.Count - 1; i > 0; --i)
            DeleteNode(node.Children[i]);

        if (node.Parent.Type == AudioNodeType.Random) //We also need to remove the child from the weight list
        {
            var data = node.Parent.NodeData as RandomData;
            if(data != null)
                data.weights.RemoveAt(node.Parent.Children.FindIndex(node)); //Find in parent, and then remove the weight in the random node
        }

        AudioBankWorker.RemoveNodeFromBank(node);

        node.Parent.Children.Remove(node);
    }
}
}
