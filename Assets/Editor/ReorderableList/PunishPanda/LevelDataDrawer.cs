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

    public void DrawItem(Rect position, int index)
    {
        var currentLevel = levelDataList[index];
        Rect togglePos = position;
        togglePos.height = 17;
        currentLevel.FoldedOut = EditorGUI.Toggle(togglePos, currentLevel.LevelName, currentLevel.FoldedOut);
        
        position.y += 17;
        position.height = 17;
        if (currentLevel.FoldedOut)
        {
            Rect nameRect = position;
            float totalWidth = position.width;
            position.width -= 60;
            currentLevel.LevelName = EditorGUI.TextField(position, currentLevel.LevelName);
            position.x += totalWidth - 60;
            position.width = 60;
            currentLevel.Mode = (PunishPanda.Game.GameModes) EditorGUI.EnumPopup(position, currentLevel.Mode);
            nameRect.y += 17;
            currentLevel.Score.One = EditorGUI.IntField(nameRect, "One", currentLevel.Score.One);
            nameRect.y += 17;
            currentLevel.Score.Two = EditorGUI.IntField(nameRect, "Two", currentLevel.Score.Two);
            nameRect.y += 17;
            currentLevel.Score.Three = EditorGUI.IntField(nameRect, "Three", currentLevel.Score.Three);
        }
    }

    public float GetItemHeight(int index)
    {
        var currentLevel = levelDataList[index];
        if (currentLevel.FoldedOut)
            return 17.0f*5;
        return 17;
    }
}
