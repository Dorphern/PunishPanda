using UnityEditor;
using UnityEngine;
namespace InAudio.InAudioEditorGUI
{ 
public static class RandomDataDrawer {
    public static void Draw(AudioNode node)
    {
        //UndoHandler.CheckUndo(new UnityEngine.Object[] { node, node.NodeData }, "Random Data Node Change");
        UndoHelper.GUIUndo(node, "Name Change", () =>
            EditorGUILayout.TextField("Name", node.Name),
            s => node.Name = s);
        NodeTypeDataDrawer.Draw(node);
        EditorGUILayout.Separator();

        if (node.NodeData.SelectedArea == 0)
        {
            EditorGUILayout.BeginVertical();

            EditorGUILayout.LabelField("Weights");


            for (int i = 0; i < node.Children.Count; ++i)
            {
                var child = node.Children[i];
                var weights = (node.NodeData as RandomData).weights;
   
                weights[i] = EditorGUILayout.IntSlider(child.Name, weights[i], 0, 100);
            }

            EditorGUILayout.EndVertical();
        }
        //UndoHandler.CheckGUIChange();
        
    }
}
}