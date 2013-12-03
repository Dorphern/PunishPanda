using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using InAudio;
using InAudio.ExtensionMethods;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class IntegrityGUI
{
    public IntegrityGUI(InAudioBaseWindow window)
    {
    }

    public void OnEnable()
    {
           
    }

    public bool OnGUI()
    {
        EditorGUILayout.HelpBox("Do not Undo these operations! No guarantee about what could break.", MessageType.Warning);
        EditorGUILayout.Separator(); EditorGUILayout.Separator(); EditorGUILayout.Separator();
        EditorGUILayout.HelpBox("While Banks works in allmost every time, it can happen that audio nodes gets deattached from their bank when working in the editor. \nThis will reassign all nodes to their correct bank.", MessageType.Info);
        if (GUILayout.Button("Fix Bank Integrity"))
        {
            TreeWalker.ForEach(InAudioInstanceFinder.DataManager.BankLinkTree, DeleteAllNodesFromBanks);

            TreeWalker.ForEach(InAudioInstanceFinder.DataManager.AudioTree, AddNodesToBank);
        }

        EditorGUILayout.Separator(); EditorGUILayout.Separator(); EditorGUILayout.Separator();
        if (!UndoHelper.IsNewUndo)
        {
            EditorGUILayout.HelpBox("As you are not using Unity 4.3 or later, there is likely unused audio data stored." +
                                    "\nThis is because data is not always deleted since the Undo system does not support undo of deletion." +
                                    "\nCleanup will remove all unused data.", MessageType.Info);
        }
        else
        {
            EditorGUILayout.HelpBox("No nodes should be unused, but in the case there is this will remove all unused data.\nNo performance is lost if unused nodes remains, but will waste a bit of memory. This will clean up any unused data", MessageType.Info);
        }
        
        if (GUILayout.Button("Clean Up Unused Data"))
        {
            int deletedTotal = 0;

            var audioRoot = InAudioInstanceFinder.DataManager.AudioTree;

            //Audio node cleanup
            Action<AudioNode, HashSet<MonoBehaviour>> action = null;
            action = (node, set) =>
            {
                set.Add(node);
                if (node.NodeData != null)
                    set.Add(node.NodeData);
                for (int i = 0; i < node.Children.Count; ++i)
                {
                    action(node.Children[i], set);
                }
            };
            int nodesDeleted = Cleanup(audioRoot, action);
            if(nodesDeleted > 0)
                Debug.Log("Deleted " + nodesDeleted + " Unused Audio Nodes");
            deletedTotal += nodesDeleted;

            var eventRoot = InAudioInstanceFinder.DataManager.EventTree;

            //Audio node cleanup
            Action<AudioEvent, HashSet<MonoBehaviour>> eventAction = null;
            eventAction = (node, set) =>
            {
                set.Add(node);
                for (int i = 0; i < node.ActionList.Count; ++i)
                {
                    set.Add(node.ActionList[i]);
                }
                for (int i = 0; i < node.Children.Count; ++i)
                {
                    eventAction(node.Children[i], set);
                }
            };
            nodesDeleted = Cleanup(eventRoot, eventAction);
            if (nodesDeleted > 0)
                Debug.Log("Deleted " + nodesDeleted + " Unused Event Nodes");
            deletedTotal += nodesDeleted;

            var busRoot = InAudioInstanceFinder.DataManager.BusTree;
            //Audio node cleanup
            Action<AudioBus, HashSet<MonoBehaviour>> busAction = null;
            busAction = (node, set) =>
            {
                set.Add(node);
                for (int i = 0; i < node.Children.Count; ++i)
                {
                    busAction(node.Children[i], set);
                }
            };
            nodesDeleted = Cleanup(busRoot, busAction);
            if (nodesDeleted > 0)
                Debug.Log("Deleted " + nodesDeleted + " Unused Bus Nodes");

            nodesDeleted = DeleteUnusedBanks(InAudioInstanceFinder.DataManager.BankLinkTree);

            if (nodesDeleted > 0)
                Debug.Log("Deleted " + nodesDeleted + " Unused Audio Banks");
            deletedTotal += nodesDeleted;

            if (deletedTotal == 0)
            {
                Debug.Log("Nothing to clean up");
            }
        }
        

        return false;
    }

    private static int Cleanup<T>(T audioRoot, Action<T, HashSet<MonoBehaviour>> traverse) where T : MonoBehaviour
    {
        HashSet<MonoBehaviour> objects = new HashSet<MonoBehaviour>();
        var allNodes = audioRoot.GetComponents<MonoBehaviour>();
        for (int i = 0; i < allNodes.Length; ++i)
        {
            objects.Add(allNodes[i]);
        }

        HashSet<MonoBehaviour> inUse = new HashSet<MonoBehaviour>();
        
        traverse(audioRoot, inUse);

        int deleted = 0;
        //Delete all objects not in use
        foreach (MonoBehaviour node in objects) 
        {
            if (!inUse.Contains(node))
            {
                deleted += 1;
                UndoHelper.Destroy(node);
            }
        }
        return deleted;
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

    private static int DeleteUnusedBanks(AudioBankLink bankRoot) 
    {
        #region Standard cleanup
        Action<AudioBankLink, HashSet<MonoBehaviour>> bankAction = null;
        bankAction = (node, set) =>
        {
            set.Add(node);
            for (int i = 0; i < node.Children.Count; ++i)
            {
                bankAction(node.Children[i], set);
            }
        };

        HashSet<MonoBehaviour> objects = new HashSet<MonoBehaviour>();
        var allNodes = bankRoot.GetComponents<MonoBehaviour>();
        for (int i = 0; i < allNodes.Length; ++i)
        {
            objects.Add(allNodes[i]);
        }
        

        HashSet<MonoBehaviour> inUse = new HashSet<MonoBehaviour>();
        int deleteCount = 0;
        bankAction(bankRoot, inUse);
        List<string> toDelete = new List<string>();
        //Delete all objects not in use
        foreach (AudioBankLink node in objects)
        {
            if (!inUse.Contains(node))
            {
                ++deleteCount;
                toDelete.Add(node.ID.ToString());
                UndoHelper.Destroy(node);
            }
        }
        #endregion

        FileInfo[] banks = GetPrefabsAtPath(FolderSettings.BankRelativeDictory);

        for (int i = 0; i < banks.Length; ++i)
        {
            string name = banks[i].Name;
            name = name.Split(new [] {".prefab"}, StringSplitOptions.RemoveEmptyEntries)[0];
            var bankLink = TreeWalker.FindById(bankRoot, Convert.ToInt32(name, 10));
            if (bankLink == null)
            {
                AssetDatabase.DeleteAsset(FolderSettings.BankDeleteDictory + banks[i].Name);
                ++deleteCount;
            }
            
        }
        

        return deleteCount;
    }

    public static FileInfo[] GetPrefabsAtPath(string path)
    {
        DirectoryInfo dir = new DirectoryInfo(Application.dataPath + path);
        return dir.GetFiles("*.prefab");
    }
}
