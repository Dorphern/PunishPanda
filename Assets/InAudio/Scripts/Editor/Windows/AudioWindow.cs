using System;
using InAudio.ExtensionMethods;
using UnityEditor;
using UnityEngine;

public class AudioWindow : InAudioBaseWindow
{
    private AudioCreatorGUI audioCreatorGUI;

    void OnEnable()
    {
        BaseEnable();

        if (audioCreatorGUI == null)
        {
            audioCreatorGUI = new AudioCreatorGUI(this);
            
        }
        audioCreatorGUI.OnEnable();
    }

    public void Find(Func<AudioNode, bool> filter)
    {
        audioCreatorGUI.FindAudio(filter);
    }

    public void Find(AudioNode toFind)
    {
        audioCreatorGUI.FindAudio(toFind);
    }

    public static void Launch()
    {
        EditorWindow window = EditorWindow.GetWindow(typeof(AudioWindow));
        window.Show(); 
        
        //window.minSize = new Vector2(800, 200);
        window.title = "Audio Window";

    }

    private GameObject cleanupGO;

    void Update()
    {
        if (cleanupGO == null)
        {
            cleanupGO = Resources.Load("PrefabGO") as GameObject;
            DontDestroyOnLoad(cleanupGO);
        }

        BaseUpdate();
        if (audioCreatorGUI != null && Manager != null) 
            audioCreatorGUI.OnUpdate();
    }

    void OnGUI()
    {
        GUI.SetNextControlName("TreeView");
        GUI.TextField(new Rect(-100,-100,20,10), "TreeView" );

        //int nextControlID = GUIUtility.GetControlID(FocusType.Passive) + 1;
        //Debug.Log(nextControlID);  
        if (!HandleMissingData())
        {
            return;
        }

        if (audioCreatorGUI == null)
            audioCreatorGUI = new AudioCreatorGUI(this);

        isDirty = false;
        DrawTop(topHeight);
        
        isDirty |= audioCreatorGUI.OnGUI(LeftWidth, (int)position.height - topHeight);
   
        if(isDirty)
            Repaint();

        PostOnGUI();
    }

    private void DrawTop(int topHeight)
    {
        EditorGUILayout.BeginVertical(GUILayout.Height(topHeight));
        EditorGUILayout.EndVertical();
    }
}   
