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
		position.height = 17;
        var currentLevel = levelDataList[index];
        Rect togglePos = position;
        togglePos.height = 17;
        float totalWidth = position.width;
        position.width -= 60;
        currentLevel.LevelName = EditorGUI.TextField(position, currentLevel.LevelName);
        Rect levelTypePos = position;
        levelTypePos.x += totalWidth - 60;
        levelTypePos.width = 60;
        position.y += 17;
        currentLevel.Mode = (PunishPanda.Game.GameModes) EditorGUI.EnumPopup(levelTypePos, currentLevel.Mode);
        currentLevel.UnlockedLevel = EditorGUI.Toggle(position, "Unlocked", currentLevel.UnlockedLevel);
    }

    public float GetItemHeight(int index)
    {
        return 17 * 2;
    }
}
