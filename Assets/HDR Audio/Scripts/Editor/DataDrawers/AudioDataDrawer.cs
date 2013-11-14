using HDRAudio;
using HDRAudio.ExtensionMethods;
using UnityEditor;
using UnityEngine;

public static class AudioDataDrawer {
    public static void Draw(AudioNode node)
    {
        UndoHandler.CheckUndo(new Object[] { node});
        node.Name = EditorGUILayout.TextField("Name", node.Name);
        UndoHandler.CheckGUIChange();
        EditorGUILayout.Separator();
        AudioData audio = node.NodeData as AudioData;
        var clip = (AudioClip)EditorGUILayout.ObjectField(audio.Clip, typeof(AudioClip), false);
        if (clip != audio.EditorClip) //Assign new clip
        {
            Undo.RegisterUndo(new Object[] {  node.NodeData, node.GetBank().LazyBankFetch }, "Changed " + node.Name + " Clip");
            audio.EditorClip = clip;
            AudioBankWorker.SwapClipInBank(node, clip);
        }

        NodeTypeDataDrawer.Draw(node);
    }
}
