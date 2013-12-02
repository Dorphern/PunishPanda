using System;
using System.Linq;
using InAudio;
using InAudio.ExtensionMethods;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomEditor(typeof(EventTester))]
public class EventTestDrawer : Editor
{
    EventTester eventTester
    {
        get { return target as EventTester; }
    }

    private int LineHeight = 22;
    private int DragHeight = 20;
    private GUIStyle labelStyle;

    public int GetPropertyHeight()
    {
        return eventTester.EventList.Events.Count * LineHeight + DragHeight + 20;
    }
    public override void OnInspectorGUI()
    {
        Rect pos = EditorGUILayout.BeginVertical(GUILayout.Height(GetPropertyHeight()));

    
        var labelPos = pos;
        Color backgroundColor = GUI.color;

        GUI.skin.label.alignment = TextAnchor.UpperLeft;
        if(labelStyle != null)
            labelStyle = GUI.skin.GetStyle("label");

        labelPos.height = 14;
        var events = eventTester.EventList.Events;
        GUI.skin.label.alignment = TextAnchor.MiddleLeft;
        labelPos.y += 5;
        for (int i = 0; i < events.Count; ++i)
        {
            labelPos.height = 20;
            //EditorGUILayout.Separator();   

            AudioEvent audioEvent = eventTester.EventList.Events[i];
            if (audioEvent != null)
                GUI.Label(labelPos, audioEvent.GetName, labelStyle);
            else
                GUI.Label(labelPos, "Missing event", labelStyle);

 
            Rect buttonPos = labelPos;
            buttonPos.x = pos.width - 200; //Align to right side
            buttonPos.width = 50;
            if (audioEvent == null)
                GUI.enabled = false;

            buttonPos.width += 40;
            if (GUI.Button(buttonPos, "Post Event"))
            {
                HDRSystem.PostEvent(eventTester.gameObject, audioEvent);
            }
            buttonPos.width -= 40;
            buttonPos.x += 100;
            if (GUI.Button(buttonPos, "Find"))
            {
                EditorWindow.GetWindow<EventWindow>().Find(audioEvent);
            }
            GUI.enabled = true;
            buttonPos.x = pos.width - 44;
            buttonPos.width = 35;
            if (GUI.Button(buttonPos, "X")) 
            {
                events.RemoveAt(i);
            }
            labelPos.y += LineHeight;
            
        }
        labelPos.y += 10;
        EditorGUILayout.Separator();
        labelPos.height = DragHeight;
        
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUI.color = backgroundColor;
        GUI.Button(labelPos, "Drag event here to add event");
        OnDragging.OnDraggingObject(DragAndDrop.objectReferences, labelPos, CanDrop,
            objects => objects.ForEach(obj => events.Add(obj as AudioEvent)));

        GUI.color = backgroundColor;

        labelPos.height += 1;
        
        EditorGUILayout.Separator();
        EditorGUILayout.EndVertical();

    }

    private bool CanDrop(Object[] objects)
    {
        return
            objects.All(obj => (obj as AudioEvent) != null) &&
            objects.All(obj => (obj as AudioEvent).Type == EventNodeType.Event);
    }

    private void DeleteAtIndex(SerializedProperty prop, int index)
    {
        int arraySize = prop.arraySize;
        prop.DeleteArrayElementAtIndex(index);
        for (int i = index; i < arraySize - 1; ++i)
        {
            prop.GetArrayElementAtIndex(i).objectReferenceValue = prop.GetArrayElementAtIndex(i + 1).objectReferenceValue;
        }
        prop.arraySize--;
    }
    private void HandleDrag(SerializedProperty prop)
    {
        if (Event.current.type == EventType.DragUpdated || Event.current.type == EventType.DragPerform)
        {
            bool canDropObject = true;
            int clipCount = DragAndDrop.objectReferences.Count(obj =>
            {
                var audioEvent = obj as AudioEvent;
                if (audioEvent == null)
                    return false;
                return audioEvent.Type == EventNodeType.Event;
            });

            if (clipCount != DragAndDrop.objectReferences.Length || clipCount == 0)
                canDropObject = false;

            if (canDropObject)
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Generic;

                if (Event.current.type == EventType.DragPerform)
                {
                    int arraySize = prop.arraySize;
                    prop.arraySize++;

                    prop.GetArrayElementAtIndex(arraySize - 1).objectReferenceValue = DragAndDrop.objectReferences[0];
                }
            }
            else
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.None;
            }
        }
    }
}
