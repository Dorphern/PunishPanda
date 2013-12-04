using System;
using System.Linq;
using InAudio;
using UnityEditor;
using UnityEngine;

namespace InAudio.InAudioEditorGUI
{

public static class NodeTypeDataDrawer
{
    //private static GameObject go;
    public static void Draw(AudioNode node)
    {

        EditorGUILayout.BeginVertical(GUILayout.MaxWidth(700));
        NodeTypeData baseData = node.NodeData;
        EditorGUILayout.Separator();

        UndoHelper.GUIUndo(node, "Name Change", ref baseData.SelectedArea, () =>
            GUILayout.Toolbar(baseData.SelectedArea, new[] { "Audio", "Attenuation" }));
        EditorGUILayout.Separator(); EditorGUILayout.Separator();

        if (baseData.SelectedArea == 0)
        {
            EditorGUILayout.IntField("ID", node.GUID);

            Seperators(2);

            #region Volume

            UndoHelper.GUIUndo(baseData, "Random Volume", ref baseData.RandomVolume, () =>
                EditorGUILayout.Toggle("Random volume", baseData.RandomVolume));

            if (!baseData.RandomVolume)
            {
                UndoHelper.GUIUndo(baseData, "Volume", () =>
                    EditorGUILayout.Slider("Volume", baseData.MinVolume, 0, 1),
                    v =>
                    {
                        baseData.MinVolume = v;
                        if (baseData.MinVolume > baseData.MaxVolume)
                            baseData.MaxVolume = baseData.MinVolume + 0.1f;
                    });
            }
            else
            {
                UndoHelper.GUIUndo(baseData, "Random Volume", ref baseData.MinVolume, ref baseData.MaxVolume, (out float minVolume, out float maxVolume) =>
                {
                    EditorGUILayout.MinMaxSlider(new GUIContent("Volume"), ref baseData.MinVolume, ref baseData.MaxVolume, 0, 1);
                    minVolume = Mathf.Clamp(EditorGUILayout.FloatField("Min volume", baseData.MinVolume), 0, baseData.MaxVolume);
                    maxVolume = Mathf.Clamp(EditorGUILayout.FloatField("Max volume", baseData.MaxVolume), baseData.MinVolume, 1);
                });
            }
            #endregion

            Seperators(2);

            #region Parent pitch

            float minPitch = 0.001f;
            float maxPitch = 3;
            UndoHelper.GUIUndo(baseData, "Random Pitch", ref baseData.RandomPitch, () =>
                EditorGUILayout.Toggle("Random Pitch", baseData.RandomPitch));

            if (!baseData.RandomPitch)
            {
                UndoHelper.GUIUndo(baseData, "Pitch", () =>
                    EditorGUILayout.Slider("Pitch", baseData.MinPitch, minPitch, maxPitch),
                    v => {
                        baseData.MinPitch = v;
                        if (baseData.MinPitch > baseData.MaxPitch)
                            baseData.MaxPitch = baseData.MinPitch + 0.1f;
                        baseData.MaxPitch = Mathf.Clamp(baseData.MaxPitch, minPitch, 3.0f);
                    });
            }
            else
            {
                UndoHelper.GUIUndo(baseData, "Random Pitch", 
                    ref baseData.MinPitch, ref baseData.MaxPitch, 
                    (out float v1, out float v2) => {
                        EditorGUILayout.MinMaxSlider(new GUIContent("Pitch"), ref baseData.MinPitch, ref baseData.MaxPitch, minPitch, maxPitch);
                        v1 = Mathf.Clamp(EditorGUILayout.FloatField("Min pitch", baseData.MinPitch), minPitch, baseData.MaxPitch);
                        v2 = Mathf.Clamp(EditorGUILayout.FloatField("Max pitch", baseData.MaxPitch), baseData.MinPitch, maxPitch);
                });
            }

            #endregion

            Seperators(2);

            #region Delay

            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical();

            UndoHelper.GUIUndo(baseData, "Randomize Delay", ref baseData.RandomizeDelay, () => 
                EditorGUILayout.Toggle("Randomize Delay", baseData.RandomizeDelay));
            if (baseData.RandomizeDelay)
            {
                UndoHelper.GUIUndo(baseData, "Delay Change", ref baseData.InitialDelayMin, ref baseData.InitialDelayMax,
                    (out float v1, out float v2) =>
                    {
                        v1 = Mathf.Clamp(EditorGUILayout.FloatField("Min delay", baseData.InitialDelayMin), 0, baseData.InitialDelayMax);
                        v2 = Mathf.Clamp(EditorGUILayout.FloatField("Max delay", baseData.InitialDelayMax), baseData.InitialDelayMin, float.MaxValue);        
                    });
                
            }
            else
            {
                UndoHelper.GUIUndo(baseData, "Delay", ref baseData.InitialDelayMin, () =>
                {
                    float delay = Mathf.Max(EditorGUILayout.FloatField("Initial delay", baseData.InitialDelayMin), 0);
                    if (delay > baseData.InitialDelayMax)
                        baseData.InitialDelayMax = baseData.InitialDelayMin + 0.001f;
                    return delay;
                });
                
            }

            GUILayout.EndVertical();

            GUILayout.BeginVertical();

            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
            #endregion

            Seperators(2);

            #region Audio bus

            DataDrawerHelper.DrawBus(node);

            #endregion

            Seperators(2);

            #region Loops

            GUILayout.BeginVertical();

            UndoHelper.GUIUndo(baseData, "Use looping", ref baseData.Loop, () => EditorGUILayout.Toggle("Loop", baseData.Loop));
            if (baseData.Loop)
            {
                GUI.enabled = baseData.Loop;

                UndoHelper.GUIUndo(baseData, "Loop Infinite", ref baseData.LoopInfinite, 
                    () => EditorGUILayout.Toggle("Loop Infinite", baseData.LoopInfinite));
                if (baseData.Loop)
                    GUI.enabled = !baseData.LoopInfinite;

                UndoHelper.GUIUndo(baseData, "Loop Randomize", ref baseData.RandomizeLoops,
                    () => EditorGUILayout.Toggle("Randomize Loop Count", baseData.RandomizeLoops));

                if (!baseData.RandomizeLoops)
                {
                    UndoHelper.GUIUndo(baseData, "Loop Count", 
                        ref baseData.MinIterations, () => (byte) Mathf.Clamp(EditorGUILayout.IntField("Loop Count", baseData.MinIterations), 0, 255));
                }
                else
                {
                    GUILayout.BeginHorizontal();
                    UndoHelper.GUIUndo(baseData, "Loop Count", ref baseData.MinIterations, ref baseData.MaxIterations,
                        (out byte v1, out byte v2) =>
                        {
                            v1 = (byte)Mathf.Clamp(EditorGUILayout.IntField("Min Loop Count", baseData.MinIterations), 0, 255);
                            v2 = (byte)Mathf.Clamp(EditorGUILayout.IntField("Max Loop Count", baseData.MaxIterations), 0, 255);

                            //Clamp to 0-255 and so that min/max doesn't overlap
                            v2 = (byte)Mathf.Clamp(v2, v1, 255);
                            v1 = (byte)Mathf.Clamp(v1, 0, v2);        
                        });

                    GUILayout.EndHorizontal();
                }

            }

            GUI.enabled = true;

            GUILayout.EndVertical();

            #endregion

            Seperators(2);

            #region Instance limiting
            UndoHelper.GUIUndo(node, "Limit Instances", ref node.LimitInstances, () => EditorGUILayout.Toggle("Limit Instances", node.LimitInstances));
            GUI.enabled = node.LimitInstances;
            if (node.LimitInstances)
            {
                UndoHelper.GUIUndo(node, "Max Instance Cont", ref node.MaxInstances, () => Math.Max(EditorGUILayout.IntField("Max Instance Count", node.MaxInstances), 0));
                UndoHelper.GUIUndo(node, "Stealing Type", ref node.InstanceStealingTypes, () => (InstanceStealingTypes) EditorGUILayout.EnumPopup("Stealing Type", node.InstanceStealingTypes));            
            }
            GUI.enabled = true;

            #endregion
        }
        else
        {
            #region Attenuation
            UndoHelper.GUIUndo(baseData, "Override Parent", ref baseData.OverrideAttenuation, () => GUILayout.Toggle(baseData.OverrideAttenuation, "Override Parent"));

            GUI.enabled = baseData.OverrideAttenuation;

            UndoHelper.GUIUndo(node, "Rolloff Mode", ref baseData.RolloffMode, () => (AudioRolloffMode)EditorGUILayout.EnumPopup("Volume Rolloff", baseData.RolloffMode));

            UndoHelper.GUIUndo(baseData, "Set Rolloff Distance", ref baseData.MinDistance, ref baseData.MaxDistance,
                (out float v1, out float v2) =>
                {
                    v1 = EditorGUILayout.FloatField("Min Distance", baseData.MinDistance);
                    v2 = EditorGUILayout.FloatField("Max Distance", baseData.MaxDistance);
                    v1 = Mathf.Max(v1, 0.00001f);
                    v2 = Mathf.Max(v2, v1 + 0.01f);
                });

            if (baseData.RolloffMode == AudioRolloffMode.Custom)
            {
                EditorGUILayout.HelpBox(
                    "Unity does not support setting custom rolloff via scripts. Please select Logarithmic or Linear rolloff instead.",
                    MessageType.Error, true);
                GUI.enabled = false;
            }

            /*AudioSource targetComp = go.GetComponent<AudioSource>();

            if (targetComp != null)
            {
                targetComp.minDistance = baseData.MinDistance;
                targetComp.maxDistance = baseData.MaxDistance;
                targetComp.rolloffMode = baseData.RolloffMode;

                if (baseData.RolloffMode != AudioRolloffMode.Custom)
                {
                    EditorGUILayout.BeginScrollView(new Vector2(0, 325), GUIStyle.none, GUIStyle.none,
                        GUILayout.Width(400), GUILayout.Height(350));
                    Rect area = EditorGUILayout.BeginVertical();

                    try
                    {
                        var editor = Editor.CreateEditor(targetComp);
                        editor.OnInspectorGUI();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.EndScrollView();
                    GUI.Button(area, "", new GUIStyle());
                }
            }*/

            #endregion 

            GUI.enabled = true;
        }
        EditorGUILayout.EndVertical();
    }

    private static void Seperators(int layoutNumbers)
    {
        for (int i = 0; i < layoutNumbers; i++)
        {
            EditorGUILayout.Separator();
        }
    }
}
}