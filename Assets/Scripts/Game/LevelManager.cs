﻿using System;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

public class LevelManager : MonoBehaviour
{
    public LevelData CurrentLevel
    {
        get
        {
            return currentWorld.Levels[currentLevelIndex];
        }
    }

    public int CurrentLevelIndex
    {
        get
        {
            return currentLevelIndex;
        }
    }

    public GameWorld CurrentWorld
    {
        get
        {
            return currentWorld;
        }
        set
        {
            if (value != null)
            {
                currentWorld = value;
                currentLevelIndex = -1;
            }
        }
    }

    public string LevelToLoad
    {
        get; private set;
    }

    public void LoadMainMenu()
    {
        isInMainMenu = true;
        LoadLevelWithTransition(mainMenuName);
    }

    public void LoadLevelByWorldIndex(int index)
    {
        if (currentWorld.Levels.Count > index)
        {
            isInMainMenu = false;
            currentLevelIndex = index;
            LoadLevelWithTransition(CurrentLevel.LevelName);
        }
    }

    public void Reload()
    {
        isInMainMenu = false;
        LoadLevelWithTransition(CurrentLevel.LevelName);
    }

    public void LoadNextLevel()
    {
        isInMainMenu = false;

        ++currentLevelIndex;
        if (MoreLevelsInWorld)
        {
            LoadLevelWithTransition(CurrentLevel.LevelName);
        }
        else
        {
            NextWorld();
            isInMainMenu = true;
            LoadLevelWithTransition("MainMenu");
        }
    }

    public GameWorld GetWorld(int index)
    {
        return worlds[index];
    }

    public bool IsInMainMenu
    {
        get
        {
            return isInMainMenu;
        }
    }

    public bool IsLoadingLevel
    {
        get;
        set;
    }

    public int WorldCount
    {
        get { return worlds.Count; }
    }

    public void TransitionIntoLevel()
    {
#if UNITY_EDITOR
        isInMainMenu = false;
        currentLevelIndex = 0;
#endif
    }


    void Awake()
    {
        currentWorld = worlds[0];
        currentLevelIndex = -1;
    }

    [SerializeField]
    private string transitionLevelName;

    [SerializeField]
    private string mainMenuName;

    [SerializeField]
    private List<GameWorld> worlds;

    private bool isInMainMenu = true;


    private GameWorld currentWorld;
    private int currentLevelIndex;

    private bool MoreLevelsInWorld
    {
        get
        {
            return currentWorld.Levels.Count > currentLevelIndex;
        }
    }

    private void LoadLevelWithTransition(string levelName)
    {
        LevelToLoad = levelName;
        Application.LoadLevel(transitionLevelName);
    }

    private void NextWorld()
    {
        int index = worlds.FindIndex(currentWorld);
        if (index + 1 < worlds.Count)
        {
            CurrentWorld = worlds[index + 1];
        }
        else
        {
            currentLevelIndex = -1;
        }
    }
	
	public void LoadLevel1()
	{
		InstanceFinder.LevelManager.LoadLevelByWorldIndex(0);
	}
}