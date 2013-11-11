using HDRAudio;
using HDRAudio.ExtensionMethods;
using UnityEditor;
using UnityEngine;

public static class AudioBankLinkDrawer
{
    public static void Draw(AudioBankLink node)
    {
       
        //UndoCheck.Instance.CheckUndo(node, "Audio Bank:" + node.Name);
        EditorGUILayout.BeginVertical();
        UndoCheck.Instance.CheckUndo(node);
        node.Name = EditorGUILayout.TextField("Name", node.Name);
        UndoCheck.Instance.CheckDirty(node);
        if (node.Type != AudioBankTypes.Folder)
        {
            EditorGUILayout.IntField("ID", node.GUID);
            EditorGUILayout.Separator();
            
            bool autoLoad = EditorGUILayout.Toggle("Auto load", node.AutoLoad);
            if (autoLoad != node.AutoLoad)
            {
                Undo.RegisterUndo(node, "Bank Auto Load");
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
