using InAudio;
using InAudio.ExtensionMethods;
using UnityEditor;
using UnityEngine;

namespace InAudio.InAudioEditorGUI
{
public static class AudioBankLinkDrawer
{
    public static void Draw(AudioBankLink node)
    {
        EditorGUILayout.BeginVertical();

        UndoHelper.GUIUndo(node, "Name Change", () =>
        {
            return EditorGUILayout.TextField("Name", node.Name);
        }, s => node.Name = s);


        if (node.Type != AudioBankTypes.Folder)
        {
            EditorGUILayout.IntField("ID", node.GUID);
            EditorGUILayout.Separator();
            
            bool autoLoad = EditorGUILayout.Toggle("Auto load", node.AutoLoad);
            if (autoLoad != node.AutoLoad)
            {
                UndoHelper.RecordObjectFull(node, "Bank Auto Load");
                node.AutoLoad = autoLoad;
            }
        }
        
        Rect lastArea = GUILayoutUtility.GetLastRect();
        lastArea.y += 28;
        lastArea.width = 200;
        if(GUI.Button(lastArea, "Find Folders using this bank"))
        {
            EditorWindow.GetWindow<AudioWindow>().Find(audioNode => audioNode.GetBank() != node);
        }

        EditorGUILayout.EndVertical();
        //UndoCheck.Instance.CheckDirty(node);
      
    }
}
}