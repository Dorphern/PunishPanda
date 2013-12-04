using UnityEditor;

namespace InAudio.InAudioEditorGUI
{ 
public static class MultiDataDrawer  {
    public static void Draw(AudioNode node)
    {

        UndoHelper.GUIUndo(node, "Name Change", () =>
            EditorGUILayout.TextField("Name", node.Name),
            s => node.Name = s);
        NodeTypeDataDrawer.Draw(node); 
    }
}
}