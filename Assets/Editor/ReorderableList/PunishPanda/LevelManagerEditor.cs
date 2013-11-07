//using System;
//using UnityEngine;
//using System.Collections;

//// Copyright (c) 2012-2013 Rotorz Limited. All rights reserved.
//// Use of this source code is governed by a BSD-style license that can be
//// found in the LICENSE file.

//using UnityEditor;

//using Rotorz.ReorderableList;

//[CustomEditor(typeof(LevelManager))]
//public class LevelManagerEditor : Editor
//{
//    private LevelManager GetManager
//    {
//        get
//        {
//            return target as LevelManager;
//        }
//    }

//    public override void OnInspectorGUI()
//    {
//        serializedObject.Update();

//        ReorderableListGUI.Title("Worlds");
//        ReorderableListGUI.ListField(GetManager.Worlds, DrawItem);

//        serializedObject.ApplyModifiedProperties();
//    }

//    private GameWorld DrawItem(Rect position, GameWorld item)
//    {
//        item.WorldName = EditorGUI.TextField(position, "World Name", item.WorldName);
//        return item;
//    }

//}
