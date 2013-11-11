using System.Collections.Generic;
using HDRAudio.ExtensionMethods;
using UnityEditor;
using UnityEngine;

namespace HDRAudio
{
 
public static class AudioBankWorker {
    private static AudioBankLink CreateNode(GameObject go, AudioBankLink parent, int guid)
    {
        var node = go.AddComponent<AudioBankLink>();
        node.GUID = guid;
        node.Parent = parent;
        node.IsFoldedOut = true;
        node.AssignParent(parent);
        return node;
    }

    private static AudioBankLink CreateRoot(GameObject go, int guid)
    {
        var node = CreateNode(go, null, guid);
        node.Name = "Root";
        node.Type = AudioBankTypes.Folder;
        return node;
    }
 
    public static AudioBankLink CreateFolder(GameObject go, AudioBankLink parent, int guid)
    {
        var node = CreateNode(go, parent, guid);
        node.Name = parent.Name + " Child Folder";
        node.Type = AudioBankTypes.Folder;
        return node;
    }

    private static AudioBankLink CreateBankLink(GameObject go, AudioBankLink parent, int guid)
    {
        var node = CreateNode(go, parent, guid);
        node.Name = parent.Name + " Child"; 
        node.Type = AudioBankTypes.Link;
        return node;
    }

    public static AudioBankLink CreateBank(GameObject go, AudioBankLink parent, int guid)
    {
        AudioBankLink link = CreateBankLink(go, parent, guid);

        SaveAndLoad.CreateAudioBank(guid);
        return link;
    }

    public static AudioBankLink CreateTree(GameObject go)
    {
        var root = CreateRoot(go, GUIDCreator.Create());
        return root;
    }

    public static bool SwapClipInBank(AudioNode node, AudioClip newClip)
    {
        var bank = node.GetBank();
        
        var clipTuple = bank.LazyBankFetch.Clips;

        for (int i = 0; i < clipTuple.Count; i++)
        {
            if (clipTuple[i].Node == node)
            {
                
                clipTuple[i].Clip = newClip;

                return true;
            }
        }
        return false;
    }

    public static void AddNodeToBank(AudioNode node, AudioClip clip)
    {
        var bank = node.GetBank().LazyBankFetch;
        bank.Clips.Add(CreateTuple(node, clip));
    }

    public static void RemoveNodeFromBank(AudioNode node)
    {
        var bank = node.GetBank().LazyBankFetch;
        bank.Clips.RemoveAll(p => p.Node == node);
    }

    private static BankTuple CreateTuple(AudioNode node, AudioClip clip)
    {
        BankTuple tuple = new BankTuple();
        tuple.Node = node;
        tuple.Clip = clip;
        return tuple;
    }

    public static void ChangeBankOverriding(AudioNode node)
    {
        var currentBank = node.GetBank();
        node.OverrideParentBank = !node.OverrideParentBank;
        var newBank = node.GetBank();
        if (currentBank == newBank)
            return;

        //Double do this to register the correct state of the node
        node.OverrideParentBank = !node.OverrideParentBank;
        Undo.RegisterUndo(UndoHelper.Array(currentBank.LazyBankFetch, newBank.LazyBankFetch, node), "Undo Changing Used Bank");
        node.OverrideParentBank = !node.OverrideParentBank;
        
        MoveBetweenBanks(node, currentBank, newBank);
    }

    public static void MoveBetweenBanks(AudioNode node, AudioBankLink current, AudioBankLink newBankLink)
    {
        var currentBank = current.LazyBankFetch.Clips;
        var newBank = newBankLink.LazyBankFetch.Clips;
        var toMove = new HashSet<AudioNode>();
        BuildMoveSet(toMove, node);
        for (int i = currentBank.Count - 1; i >= 0; --i)
        {
            if (toMove.Contains(currentBank[i].Node))
            {
                newBank.Add(currentBank[i]);
                currentBank.RemoveAt(i);
            }
        }
    }

    public static void SetNewBank(AudioNode node, AudioBankLink newBankLink)
    {
        MoveBetweenBanks(node, node.GetBank(), newBankLink);

        node.BankLink = newBankLink;
        SetNewBankLink(node, newBankLink);
    }

    private static void SetNewBankLink(AudioNode node, AudioBankLink newBankLink)
    {
        for (int i = 0; i < node.Children.Count; ++i)
        {
            var child = node.Children[i];
            if (child.Type == AudioNodeType.Folder && child.OverrideParentBank)
                continue;
            else
            {
                if(child.Type != AudioNodeType.Folder)
                    child.BankLink = newBankLink;
                SetNewBankLink(child, newBankLink);
            }
        }
    }

    private static void BuildMoveSet(HashSet<AudioNode> nodeSet, AudioNode node)
    {
        if (node.Type == AudioNodeType.Audio)
        {
            nodeSet.Add(node);
        }
        else
        {
            for (int i = 0; i < node.Children.Count; ++i)
            {
                var childNode = node.Children[i];
                if (childNode.Type == AudioNodeType.Folder && childNode.OverrideParentBank)
                    continue;
                else 
                    BuildMoveSet(nodeSet, childNode);
            }
        }
    }
}
}
