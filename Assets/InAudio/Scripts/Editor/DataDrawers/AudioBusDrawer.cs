using System;
using System.Collections.Generic;
using InAudio.ExtensionMethods;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace InAudio.HDREditorGUI
{
    public static class AudioBusDrawer
    {
        private static int beDuckedByID;

        public static void Draw(AudioBus node)
        {
            EditorGUILayout.BeginVertical();

            node.Name = EditorGUILayout.TextField("Name", node.Name);
            EditorGUILayout.IntField("ID", node.ID);
            //In AuxWindow, update all bus volumes 
            EditorGUILayout.Separator();
            node.Volume = EditorGUILayout.Slider("Volume", node.Volume, 0.0f, 1.0f);
            GUI.enabled = false;
            EditorGUILayout.Slider("Combined Volume", node.CombinedVolume, 0, 1.0f);
            GUI.enabled = true;
            EditorGUILayout.Separator();
            node.DuckAmount = EditorGUILayout.Slider("Duck Amount", node.DuckAmount, -1.0f, 0.0f);
            node.ReleaseTime = Mathf.Max(EditorGUILayout.FloatField("Release Time", node.ReleaseTime), 0);
            EditorGUILayout.Separator(); EditorGUILayout.Separator();
            EditorGUILayout.BeginHorizontal();
            
            beDuckedByID =  EditorGUILayout.IntField("Bus ID to be Ducked by", beDuckedByID);
            if (GUILayout.Button("Add", GUILayout.Width(40)))
            {
                AudioBus bus = TreeWalker.FindById(HDRInstanceFinder.DataManager.BusTree, beDuckedByID);
                
                if (CanBeDuckedBy(node, bus))
                {
                    node.DuckedBy.Add(bus);
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField("Ducked By These Busses");

            
            Rect lastArea = GUILayoutUtility.GetLastRect();
            lastArea.y = lastArea.y + lastArea.height + 2;
            lastArea.height = 17;
            lastArea.x += 20;
            for (int i = 0; i < node.DuckedBy.Count; i++)
            {
                GUI.Label(lastArea, node.DuckedBy[i].GetName);
                Rect buttonArea = lastArea;
                buttonArea.x = lastArea.x + lastArea.width - 80;
                buttonArea.width = 30;
                if (GUI.Button(buttonArea, "X"))
                {
                    node.DuckedBy.RemoveAt(i);
                }

                lastArea.y += 20;
            }

            lastArea.y += 15;
            lastArea.x -= 20;
            GUILayout.Label("Nodes Playing In This Specific Bus");
            if (Application.isPlaying)
            {
                lastArea.x += 20;
                lastArea.y = lastArea.y + lastArea.height + 2;
                lastArea.height = 17;
                List<RuntimePlayer> players = node.GetRuntimePlayers();
                for (int i = 0; i < players.Count; i++)
                {
                    GUI.Label(lastArea, players[i].NodePlaying.Name);

                    lastArea.y += 20;
                }
            }
            EditorGUILayout.EndVertical();
            if(GUI.changed)
                Undo.RegisterUndo(node, "Node");
            //Undo.ClearSnapshotTarget();
        }

        private static void OnDrop(AudioBus node, Object[] dragging)
        { 
            node.DuckedBy.Add(dragging[0] as AudioBus);
        }

        private static bool CanBeDuckedBy(AudioBus node, Object dragging)
        {
            var draggingBus = dragging as AudioBus;
            if (draggingBus == null)
                return false;
            if (draggingBus.Parent == null)
                return false;
            if (NodeWorker.IsChildOf(node, draggingBus))
                return false;
            if (NodeWorker.IsParentOf(node, draggingBus.Parent))
                return false;
            return true;
        }

    }

}