using System;
using System.Collections.Generic;
using InAudio.ExtensionMethods;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace InAudio.InAudioEditorGUI
{
    public static class AudioBusDrawer
    {
        private static int beDuckedByID;

        public static void Draw(AudioBus node)
        {
            EditorGUILayout.BeginVertical();

            UndoHelper.GUIUndo(node, "Name Change", ref node.Name, () => 
                EditorGUILayout.TextField("Name", node.Name));

            EditorGUILayout.IntField("ID", node.ID);

            EditorGUILayout.Separator();

            UndoHelper.GUIUndo(node, "Volume Change", ref node.Volume, () =>
                EditorGUILayout.Slider("Master Volume", node.Volume, 0.0f, 1.0f));

            if (!Application.isPlaying)
                UndoHelper.GUIUndo(node, "Runtime Volume Change", ref node.SelfVolume, () =>
                    EditorGUILayout.Slider("Runtime Volume", node.SelfVolume, 0.0f, 1.0f));
            else
            {
                UndoHelper.GUIUndo(node, "Runtime Volume Change", ref node.RuntimeSelfVolume, () =>
                    EditorGUILayout.Slider("Runtime Volume", node.RuntimeSelfVolume, 0.0f, 1.0f));
            }

            EditorGUILayout.Separator();

            GUI.enabled = false;

            if (Application.isPlaying)
            {
                ////EditorGUILayout.Slider("Parent Volume", node.CombinedVolume, 0, 1.0f);
                //EditorGUILayout.Slider("Faded Volume", node.RuntimeSelfVolume, 0, 1.0f);
                EditorGUILayout.Separator();
                EditorGUILayout.Slider("Final Volume", node.RuntimeVolume, 0, 1.0f);
            }
            else
            {
                EditorGUILayout.Slider("Combined Volume", node.CombinedVolume, 0, 1.0f);
            }
            GUI.enabled = true;

            /*
            EditorGUILayout.Separator();

            UndoHelper.GUIUndo(node, "Duck Amount Change", () => 
                EditorGUILayout.Slider("Duck Amount", node.DuckAmount, -1.0f, 0.0f), 
                v => node.DuckAmount = v);

            UndoHelper.GUIUndo(node, "Releate Time Change", () =>
                Mathf.Max(EditorGUILayout.FloatField("Release Time", node.ReleaseTime), 0),
                v => node.ReleaseTime = v);

            EditorGUILayout.Separator(); EditorGUILayout.Separator();
            EditorGUILayout.BeginHorizontal();
            
            beDuckedByID = EditorGUILayout.IntField("Bus ID to be Ducked by", beDuckedByID);
            if (GUILayout.Button("Add", GUILayout.Width(40)))
            {
                AudioBus bus = TreeWalker.FindById(InAudioInstanceFinder.DataManager.BusTree, beDuckedByID);
                
                if (CanBeDuckedBy(node, bus))
                {
                    UndoHelper.RecordObjectFull(node, "Added Ducked By Bus");
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
                    UndoHelper.RecordObjectFull(node, "Remove Ducked By");
                    node.DuckedBy.RemoveAt(i);
                }

                lastArea.y += 20;
            }

            lastArea.y += 15;
            lastArea.x -= 20;*/
         /*   GUILayout.Label("Nodes Playing In This Specific Bus");

                lastArea.x += 20;
                lastArea.y = lastArea.y + lastArea.height + 2;
                lastArea.height = 17;
                List<RuntimePlayer> players = node.GetRuntimePlayers();
                for (int i = 0; i < players.Count; i++)
                {
                    GUI.Label(lastArea, players[i].NodePlaying.Name);

                    lastArea.y += 20;
                }
            */
            EditorGUILayout.EndVertical();
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