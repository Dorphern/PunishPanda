using InAudio;
using InAudio.ExtensionMethods;
using UnityEditor;
using UnityEngine;

namespace InAudio.InAudioEditorGUI
{
public static class AudioDataDrawer
{
    public static void Draw(AudioNode node)
    {
        UndoHelper.GUIUndo(node, "Name Change", ref node.Name, () => 
            EditorGUILayout.TextField("Name", node.Name));

        EditorGUILayout.Separator();
        AudioData audio = node.NodeData as AudioData;
        var clip = (AudioClip)EditorGUILayout.ObjectField(audio.Clip, typeof(AudioClip), false);
        if (clip != audio.EditorClip) //Assign new clip
        {
            UndoHelper.RecordObjectFull(new Object[] { node.NodeData, node.GetBank().LazyBankFetch }, "Changed " + node.Name + " Clip");
            audio.EditorClip = clip;
            AudioBankWorker.SwapClipInBank(node, clip);
            EditorUtility.SetDirty(node.GetBank().LazyBankFetch.gameObject);
            EditorUtility.SetDirty(node.NodeData.gameObject);
        }

        NodeTypeDataDrawer.Draw(node);
    }
}
}