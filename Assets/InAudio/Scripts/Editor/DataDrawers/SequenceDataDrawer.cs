using UnityEditor;
using UnityEngine;

namespace InAudio.InAudioEditorGUI
{
public static class SequenceDataDrawer
{
    public static void Draw(AudioNode node)
    {
        UndoHelper.GUIUndo(node, "Name Change", ref node.Name, () =>
            EditorGUILayout.TextField("Name", node.Name));
        NodeTypeDataDrawer.Draw(node);
    }
}
}