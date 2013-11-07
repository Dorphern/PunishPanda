using System;
using UnityEngine;
using System.Collections;

// Copyright (c) 2012-2013 Rotorz Limited. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

using UnityEditor;

using Rotorz.ReorderableList;

[CustomEditor(typeof(GameWorld))]
public class LevelDrawerEditor : Editor
{

    private GameWorld GetWorld
    {
        get
        {
            return target as GameWorld;
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        //ReorderableListGUI.Title("Name");
        LevelDataDrawer drawer = new LevelDataDrawer();
        drawer.levelDataList = GetWorld.Levels;
        GetWorld.WorldName = EditorGUILayout.TextField("World Name", GetWorld.WorldName);
        ReorderableListGUI.Title("Levels");
        ReorderableListGUI.ListField(drawer);

        serializedObject.ApplyModifiedProperties();
    }

    private LevelData DrawItem(Rect position, LevelData item)
    {
        GUI.Label(position, item.LevelName);
        return item;
    }

    private void DrawEmpty()
    {
        
    }
}
