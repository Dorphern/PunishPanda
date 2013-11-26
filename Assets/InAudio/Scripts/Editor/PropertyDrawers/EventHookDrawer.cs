#if UNITY_4_1 || UNITY_4_2
using System.Linq;
using InAudio;
using InAudio.InAudioEditorGUI;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(EventHookAttribute))]
public class EventHookDrawer : PropertyDrawer
{
    EventHookAttribute EventAttribute { get { return ((EventHookAttribute)attribute); } }

    private float LineHeight = 22;
    private float DragHeight = 20;
    private GUIStyle eventTypeStyle;

    public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
    {
        float extraHeight = prop.arraySize*LineHeight + DragHeight + 25; 
        return base.GetPropertyHeight(prop, label) + extraHeight;
    }

    public override void OnGUI (Rect pos, SerializedProperty prop, GUIContent label)
    {
        var labelPos = pos;
        Color backgroundColor = GUI.color;
 
        GUI.skin.label.alignment = TextAnchor.UpperLeft;
        var labelStyle = GUI.skin.GetStyle("label");
        
        //int fontSize = labelStyle.fontSize;
        if(eventTypeStyle == null)
            eventTypeStyle = new GUIStyle(GUI.skin.GetStyle("label"));
        //eventTypeStyle.fontSize = fontSize + 1;

        //labelStyle.fontSize = fontSize;
        //eventTypeStyle.fontSize = 12;
        labelPos.height = 14;
        eventTypeStyle.fontStyle = FontStyle.Bold;
        labelPos.x += 13;
        GUI.Label(labelPos, EventAttribute.EventType, eventTypeStyle);
        //if (EventAttribute.FoldedOut)
        {
            Debug.Log("asfasf");
            GUI.skin.label.alignment = TextAnchor.MiddleLeft;
            for (int i = 0; i < prop.arraySize; ++i)
            {
                labelPos.y += LineHeight;
                labelPos.height = 20;
                AudioEvent audioEvent = prop.GetArrayElementAtIndex(i).objectReferenceValue as AudioEvent;
                if (audioEvent != null)
                    GUI.Label(labelPos, audioEvent.GetName, labelStyle);
                else
                    GUI.Label(labelPos, "Missing event", labelStyle);

                Rect buttonPos = labelPos;
                buttonPos.x = pos.width - 100; //Align to right side
                buttonPos.width = 50;
                if (audioEvent == null)
                    GUI.enabled = false;

                if (GUI.Button(buttonPos, "Find"))
                {
                    EditorWindow.GetWindow<EventWindow>().Find(audioEvent);
                }
                GUI.enabled = true;
                buttonPos.x = pos.width - 44;
                buttonPos.width = 35;
                if (GUI.Button(buttonPos, "X"))
                {
                    DrawerHelper.DeleteAtIndex(prop, i);

                }
            }
            labelPos.y += DragHeight + 4;
            labelPos.height = DragHeight;
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUI.color = backgroundColor;
            GUI.Button(labelPos, "Drag event here to add " + EventAttribute.EventType + " event");
            if (labelPos.Contains(Event.current.mousePosition))
            {
                DrawerHelper.HandleDrag(prop);
            }

            GUI.color = backgroundColor;

            labelPos.height += 1;
        }
        //GUI.Label(labelPos, "Drag event here to add");

    }
}
#else

using InAudio.InAudioEditorGUI;
using UnityEditor;
using UnityEngine;


[CustomPropertyDrawer(typeof(EventHookAttribute))]
public class EventHookDrawer : PropertyDrawer
{
    EventHookAttribute EventAttribute { get { return ((EventHookAttribute)attribute); } }

    private float LineHeight = 22;
    private float DragHeight = 20;
    private GUIStyle eventTypeStyle;

    public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
    {
        SerializedProperty array = prop.FindPropertyRelative("Events"); 
        float extraHeight = array.arraySize * 20 + 20 + 25;
        return base.GetPropertyHeight(prop, label) + extraHeight;
    }

    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
    {
        var labelPos = pos;
        Color backgroundColor = GUI.color;

        GUI.skin.label.alignment = TextAnchor.UpperLeft;
        var labelStyle = GUI.skin.GetStyle("label");

        if (eventTypeStyle == null)
            eventTypeStyle = new GUIStyle(GUI.skin.GetStyle("label"));

        SerializedProperty array = prop.FindPropertyRelative("Events");

        labelPos.height = 14;
        eventTypeStyle.fontStyle = FontStyle.Bold;
        //labelPos.x += 13;
        GUI.Label(labelPos, EventAttribute.EventType, eventTypeStyle);
        //if (EventAttribute.FoldedOut)
        {
            labelPos.y -= 5; 
            GUI.skin.label.alignment = TextAnchor.MiddleLeft;
            for (int i = 0; i < array.arraySize; ++i)
            {
                labelPos.y += LineHeight;
                labelPos.height = 20;
                AudioEvent audioEvent = array.GetArrayElementAtIndex(i).objectReferenceValue as AudioEvent;
                if (audioEvent != null)
                    GUI.Label(labelPos, audioEvent.GetName, labelStyle);
                else
                    GUI.Label(labelPos, "Missing event", labelStyle);

                Rect buttonPos = labelPos;
                buttonPos.x = pos.width - 100; //Align to right side
                buttonPos.width = 50;
                if (audioEvent == null)
                    GUI.enabled = false;

                if (GUI.Button(buttonPos, "Find"))
                {
                    EditorWindow.GetWindow<EventWindow>().Find(audioEvent);
                }
                GUI.enabled = true;
                buttonPos.x = pos.width - 44;
                buttonPos.width = 35;
                if (GUI.Button(buttonPos, "X"))
                {
                    DrawerHelper.DeleteAtIndex(array, i);

                }
            }
            labelPos.y += DragHeight + 4;
            labelPos.height = DragHeight;
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUI.color = backgroundColor;
            GUI.Button(labelPos, "Drag event here to add " + EventAttribute.EventType + " event");
            if (labelPos.Contains(Event.current.mousePosition))
            {
                DrawerHelper.HandleDrag(array);
            }

            GUI.color = backgroundColor;

            labelPos.height += 1;
        }
    }
}

#endif
//*/