using UnityEditor;
using UnityEngine;
namespace InAudio.HDREditorGUI
{ 
public static class RandomDataDrawer {
    public static void Draw(AudioNode node)
    {
        UndoHandler.CheckUndo(new UnityEngine.Object[] { node, node.NodeData }, "Random Data Node Change");
        node.Name = EditorGUILayout.TextField("Name", node.Name);
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
        UndoHandler.CheckGUIChange();
        
    }
}
}