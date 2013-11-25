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

    private float LineHeight = 22;
    private float DragHeight = 20;
    private GUIStyle eventTypeStyle;

    

    public override void OnInspectorGUI()
    {
        Rect pos = EditorGUILayout.BeginVertical();

        //Yeah... the only way I could find to make Unity expand the area
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
 
        var labelPos = pos;
        Color backgroundColor = GUI.color;

        GUI.skin.label.alignment = TextAnchor.UpperLeft;
        var labelStyle = GUI.skin.GetStyle("label");
        //int fontSize = labelStyle.fontSize;
        if (eventTypeStyle == null)
            eventTypeStyle = new GUIStyle(GUI.skin.GetStyle("label"));
        //eventTypeStyle.fontSize = fontSize + 1;

        //labelStyle.fontSize = fontSize;
        //eventTypeStyle.fontSize = 12;
        labelPos.height = 14;
        eventTypeStyle.fontStyle = FontStyle.Bold;
        labelPos.x += 13;
        GUI.Label(labelPos, "Postable Events", eventTypeStyle);

        GUI.skin.label.alignment = TextAnchor.MiddleLeft;
        for (int i = 0; i < eventTester.Events.Count; ++i)
        {
            EditorGUILayout.Separator();   
            labelPos.y += LineHeight;
            labelPos.height = 20;
            AudioEvent audioEvent = eventTester.Events[i];
            if (audioEvent != null)
                GUI.Label(labelPos, audioEvent.GetName, labelStyle);
            else
                GUI.Label(labelPos, "Missing event", labelStyle);

            EditorGUILayout.TextField("");
            Rect buttonPos = labelPos;
            buttonPos.x = pos.width - 200; //Align to right side
            buttonPos.width = 50;
            if (audioEvent == null)
                GUI.enabled = false;

            buttonPos.width += 30;
            if (GUI.Button(buttonPos, "Post Event"))
            {
                HDRSystem.PostEvent(eventTester.gameObject, audioEvent);
            }
            buttonPos.width -= 30;
            buttonPos.x += 90;
            if (GUI.Button(buttonPos, "Find"))
            {
                EditorWindow.GetWindow<EventWindow>().Find(audioEvent);
            }
            GUI.enabled = true;
            buttonPos.x = pos.width - 44;
            buttonPos.width = 35;
            if (GUI.Button(buttonPos, "X"))
            {
                eventTester.Events.RemoveAt(i);
            }
        }
        EditorGUILayout.Separator();
          

        labelPos.y += DragHeight + 15;
        labelPos.height = DragHeight;
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUI.color = backgroundColor;
        GUI.Button(labelPos, "Drag event here to add event");
        OnDragging.OnDraggingObject(DragAndDrop.objectReferences, labelPos, canDrop,
            objects => objects.ForEach(obj => eventTester.Events.Add(obj as AudioEvent)));

        GUI.color = backgroundColor;

        labelPos.height += 1;

        EditorGUILayout.EndVertical();
    }

    private bool canDrop(Object[] objects)
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
