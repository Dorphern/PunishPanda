using InAudio;
using InAudio.ExtensionMethods;
using UnityEditor;
using UnityEngine;

namespace InAudio.HDREditorGUI
{
public static class AudioDataDrawer
{
    public static void Draw(AudioNode node)
    {
        node.Name = EditorGUILayout.TextField("Name", node.Name);
        if(GUI.changed)
            Undo.RegisterUndo(node, "Node Name");

        EditorGUILayout.Separator();
        AudioData audio = node.NodeData as AudioData;
        var clip = (AudioClip)EditorGUILayout.ObjectField(audio.Clip, typeof(AudioClip), false);
        if (clip != audio.EditorClip) //Assign new clip
        {
            UndoHelper.RecordObjects(new Object[] { node.NodeData, node.GetBank().LazyBankFetch }, "Changed " + node.Name + " Clip");
            audio.EditorClip = clip;
            AudioBankWorker.SwapClipInBank(node, clip);
        }

        NodeTypeDataDrawer.Draw(node);
    }
}
}