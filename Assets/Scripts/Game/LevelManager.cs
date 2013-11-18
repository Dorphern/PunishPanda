using System;
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
        System.GC.Collect();
        isInMainMenu = true;
		SaveData();
        Application.LoadLevel(mainMenuName);
    }
	
	[HideInInspector]
	public bool loadLevelsScreenFlag = false;
	
	public void LoadLevelsMenu()
    {
        System.GC.Collect();
        isInMainMenu = true;
		loadLevelsScreenFlag = true;
		SaveData();
        Application.LoadLevel(mainMenuName);
    }

    public void LoadLevelByWorldIndex(int index)
    {
		SaveData();
        System.GC.Collect();
        if (currentWorld.Levels.Count > index)
        {
            isInMainMenu = false;
            currentLevelIndex = index;
            Application.LoadLevel(CurrentLevel.LevelName);
        }
    }

    public void Reload()
    {
        System.GC.Collect();
        isInMainMenu = false;
		SaveData();
        Application.LoadLevel(CurrentLevel.LevelName);
    }

    public void LoadNextLevel()
    {
        System.GC.Collect();
        isInMainMenu = false;
		SaveData();
        ++currentLevelIndex;
        if (MoreLevelsInWorld)
        {
            Application.LoadLevel(CurrentLevel.LevelName);
        }
        else
        {
            NextWorld();
            isInMainMenu = true;
            Application.LoadLevel("MainMenu");
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
     
    public int currentLevelIndex;

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
	
	// adding this to ensure that data is saved when a level change occurs
	void SaveData()
	{
		InstanceFinder.StatsManager.gamesPlayed++;
		InstanceFinder.StatsManager.Save();	
	}
}