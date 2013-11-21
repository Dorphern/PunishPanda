using System;
using System.Collections.Generic;
using System.Globalization;
using Rotorz.ReorderableList;
using UnityEditor;
using UnityEngine;
using System.Collections;

public class LevelDataDrawer : IReorderableListAdaptor
{
    public List<LevelData> levelDataList;

    public int Count
    {
        get { return levelDataList.Count; }
    }

    public bool CanDrag(int index)
    {
        return true;
    }

    public bool CanRemove(int index)
    {
        return true;
    }

    public void Add()
    {
        levelDataList.Add(new LevelData());
    }

    public void Insert(int index)
    {
        levelDataList.Insert(index, new LevelData());
    }

    public void Duplicate(int index)
    {
        levelDataList.Add(levelDataList[index]);
    }

    public void Remove(int index)
    {
        levelDataList.RemoveAt(index);
    }

    public void Move(int sourceIndex, int destIndex)
    {
        if (destIndex > sourceIndex)
            --destIndex;

        LevelData item = levelDataList[sourceIndex];
        levelDataList.RemoveAt(sourceIndex);
        levelDataList.Insert(destIndex, item);
    }

    public void Clear()
    {
        levelDataList.Clear();
    }

    private const float itemHeight = 17.0f;

    public void DrawItem(Rect position, int index)
    {
        var currentLevel = levelDataList[index];

        position.height = itemHeight;
        Rect togglePos = position;
        togglePos.height = itemHeight;

        currentLevel.Toggled = EditorGUI.Foldout(position, currentLevel.Toggled, currentLevel.LevelName);
        if (currentLevel.Toggled)
        {
            position.y += itemHeight;
            currentLevel.LevelName = EditorGUI.TextField(position, "File name", currentLevel.LevelName);

            position.y += itemHeight;
            currentLevel.MaxTimeScore = EditorGUI.FloatField(position, "Max Time Score", currentLevel.MaxTimeScore);
            position.y += itemHeight;
            currentLevel.LevelLength = EditorGUI.FloatField(position, "Level Length (Seconds)", currentLevel.LevelLength);

            position.y += itemHeight;
            currentLevel.OneStar = EditorGUI.IntField(position, "One Star", currentLevel.OneStar);
            position.y += itemHeight;
            currentLevel.TwoStars = EditorGUI.IntField(position, "Two Stars", currentLevel.TwoStars);
            position.y += itemHeight;
            currentLevel.ThreeStars = EditorGUI.IntField(position, "Three Stars", currentLevel.ThreeStars);
            position.y += itemHeight;
            currentLevel.HighScore = EditorGUI.IntField(position, "Highscore", currentLevel.HighScore);
            
            position.y += itemHeight;
            currentLevel.UnlockedLevel = EditorGUI.Toggle(position, "Unlocked Level", currentLevel.UnlockedLevel);
            position.y += itemHeight;
            currentLevel.UnlockedFunFact = EditorGUI.Toggle(position, "Unlocked Fact", currentLevel.UnlockedFunFact);

            position.y += itemHeight;
            currentLevel.FunFactsText = EditorGUI.TextField(position, "Fact text", currentLevel.FunFactsText);
            position.y += itemHeight;

            position.height = 100;
            position.width = 100;
            EditorGUI.PrefixLabel(position, 0, new GUIContent("Fun Fact Texture"));
            position.x += 150;
            currentLevel.FunFactsTexture = EditorGUI.ObjectField(position, currentLevel.FunFactsTexture, typeof(Texture2D), false) as Texture2D;
        }
    }

    public float GetItemHeight(int index)
    {
        var currentLevel = levelDataList[index];
        if (currentLevel.Toggled)
            return itemHeight * 11 + 100;
        else
            return itemHeight;
    }
}
