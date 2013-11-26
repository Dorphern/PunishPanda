using InAudio;
using UnityEditor;
using UnityEngine;
using System.Linq;

public static class DataDrawerHelper {

    public static void DrawBus(AudioNode node)
    {

        if (!node.IsRoot)
        {
            
            bool overrideParent = EditorGUILayout.Toggle("Override Parent Bus", node.OverrideParentBus);
            if (overrideParent != node.OverrideParentBus)
            {
                UndoHelper.RecordObjectFull(node, "Override parent");
                node.OverrideParentBus = overrideParent;
            }
            if (!node.OverrideParentBus)
                GUI.enabled = false;
        }
        EditorGUILayout.BeginHorizontal();

        //
        if (node.Bus != null)
            EditorGUILayout.TextField("Used Bus", AudioBusWorker.GetParentBus(node).Name);
        else
        {
            GUILayout.Label("Missing node");
        }


        //EditorGUILayout.BeginHorizontal(GUILayout.Width(175));
        if (GUILayout.Button("Find"))
        {
            SearchHelper.SearchFor(node.Bus);
        }

        //EditorGUILayout.BeginVertical();
        GUI.enabled = true; 
        GUILayout.Button("Drag bus here to assign");
        //EditorGUILayout.EndVertical();

        var buttonArea = GUILayoutUtility.GetLastRect();
        var bus = HandleBusDrag(buttonArea);
        if (bus != null && bus != node.Bus)
        {
            UndoHelper.RecordObjectFull(node, "Assign bus");
            node.Bus = bus;
            node.OverrideParentBus = true;
            Event.current.Use();
        }

        //EditorGUILayout.EndHorizontal();
        //if(!node.IsRoot)
            EditorGUILayout.EndHorizontal();
    }

    private static AudioBus HandleBusDrag(Rect area)
    {
        if (area.Contains(Event.current.mousePosition) && Event.current.type == EventType.DragUpdated || Event.current.type == EventType.DragPerform)
        {
            bool canDropObject = true;
            int clipCount = DragAndDrop.objectReferences.Count(obj => obj is AudioBus);

            if (clipCount != DragAndDrop.objectReferences.Length || clipCount == 0)
                canDropObject = false;

            if (canDropObject)
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Generic;

                if (Event.current.type == EventType.DragPerform)
                {
                    return DragAndDrop.objectReferences[0] as AudioBus;
                }
            }
            else
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.None;
            }
        }
        return null;
    }
}
