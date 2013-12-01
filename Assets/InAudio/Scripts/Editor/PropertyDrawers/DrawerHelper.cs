using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Collections;
namespace InAudio.InAudioEditorGUI
{ 
public static  class DrawerHelper  {
    public static void DeleteAtIndex(SerializedProperty prop, int index)
    {
        int arraySize = prop.arraySize;
        prop.DeleteArrayElementAtIndex(index);
        for (int i = index; i < arraySize - 1; ++i)
        {
            prop.GetArrayElementAtIndex(i).objectReferenceValue = prop.GetArrayElementAtIndex(i + 1).objectReferenceValue;
        }
        prop.arraySize--;
    }
    public static void HandleDrag(SerializedProperty prop)
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
}