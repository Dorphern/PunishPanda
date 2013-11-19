using System;
using System.Collections.Generic;
using System.Linq;
using HDRAudio;
using HDRAudio.TreeDrawer;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class IntegrityGUI
{
    public IntegrityGUI(HDRBaseWindow window)
    {
    }

    public void OnEnable()
    {
           
    }

    public bool OnGUI()
    {
        EditorGUILayout.Separator(); EditorGUILayout.Separator(); EditorGUILayout.Separator();
        if (GUILayout.Button("Fix Bank Integrity"))
        {
            TreeWalker.ForEach(HDRInstanceFinder.DataManager.BankLinkTree, DeleteAllNodesFromBanks);

            TreeWalker.ForEach(HDRInstanceFinder.DataManager.AudioTree, AddNodesToBank);
        }

        return false;
    }

    private void AddNodesToBank(AudioNode audioNode)
    {
        if (audioNode.Type == AudioNodeType.Audio)
        {
            AudioBankWorker.AddNodeToBank(audioNode, (audioNode.NodeData as AudioData).Clip);
        }
    }

    private void DeleteAllNodesFromBanks(AudioBankLink audioBankLink)
    {
        if(audioBankLink.Type == AudioBankTypes.Link)
            audioBankLink.LazyBankFetch.Clips.Clear();
    }
}
