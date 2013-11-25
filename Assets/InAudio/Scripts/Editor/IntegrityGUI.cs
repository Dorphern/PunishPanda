using System;
using System.Collections.Generic;
using System.Linq;
using InAudio;
using InAudio.TreeDrawer;
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
        EditorGUILayout.HelpBox("Do not Undo these operations! No guarantee about what could break.", MessageType.Warning);
        EditorGUILayout.Separator(); EditorGUILayout.Separator(); EditorGUILayout.Separator();
        if (GUILayout.Button("Fix Bank Integrity"))
        {
            TreeWalker.ForEach(HDRInstanceFinder.DataManager.BankLinkTree, DeleteAllNodesFromBanks);

            TreeWalker.ForEach(HDRInstanceFinder.DataManager.AudioTree, AddNodesToBank);
        }
        EditorGUILayout.HelpBox("While Banks works in allmost every time, it can happen that audio nodes gets deattached from their bank when working in the editor. \nThis will reassign all nodes to their correct bank.", MessageType.Info);

        EditorGUILayout.Separator(); EditorGUILayout.Separator(); EditorGUILayout.Separator();
        if (GUILayout.Button("Clean Up Unused Nodes"))
        {
            
        }
        if (!UndoHelper.IsNewUndo)
        {
            EditorGUILayout.HelpBox("As you are not using Unity 4.3 or later, there is likely unused audio data stored." +
                                    "\nThis is because objects are not actually deleted since the Undo system doesn't support undo of deletion." +
                                    "\nCleanup will remove all unused data.", MessageType.Info);
        }
        else
        {
            EditorGUILayout.HelpBox("As most, but not all functions support perfect undo, not all do.\nNo performance is lost, but you may gain a bit of memory. This will clean up any unused data", MessageType.Info);   
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
