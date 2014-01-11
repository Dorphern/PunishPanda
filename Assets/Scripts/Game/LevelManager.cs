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
        currentLevelIndex = -1;
		SaveData();

        if (WidescreenCheck.IsWideScreen())
            LoadLevel(mainMenuName);
        else
            LoadLevel(mainMenuNameNonWide);
    }
	
	[HideInInspector]
	public bool loadLevelsScreenFlag = false;
	
	public void LoadLevelsMenu()
    {
        System.GC.Collect();
        isInMainMenu = true;
		loadLevelsScreenFlag = true;
		SaveData();

        if (WidescreenCheck.IsWideScreen())
            LoadLevel(mainMenuName);
        else
            LoadLevel(mainMenuNameNonWide);
    }

    public void LoadLevelByWorldIndex(int index)
    {
        
		SaveData();
        System.GC.Collect();
        if (currentWorld.Levels.Count > index && index!=-1)
        {
            isInMainMenu = false;
            currentLevelIndex = index;
			AddStatistics();

            LoadLevel(GetNameByIndexAndRatio(currentLevelIndex));
        }
    }


    private string GetNameByIndexAndRatio(int index)
    {
        //Then widescreen
        if (WidescreenCheck.IsWideScreen())
        {
            return currentWorld.Levels[index].LevelNameWidescreen;
        }
        else
        {
            return currentWorld.Levels[index].LevelNameNonWide;
        }
    }

    public void Reload()
    {
        System.GC.Collect();
        isInMainMenu = false;
        InstanceFinder.GameManager.ActiveLevel.OnLevelReset();
		AddStatistics();
		SaveData();
        LoadLevel(GetNameByIndexAndRatio(currentLevelIndex));
    }

    public void LoadNextLevel()
    {
        System.GC.Collect();
        isInMainMenu = false;
        ++currentLevelIndex;
        InstanceFinder.GameManager.ActiveLevel.OnNextLevel();
        if (MoreLevelsInWorld)
        {
			AddStatistics();
			SaveData();
            LoadLevel(GetNameByIndexAndRatio(currentLevelIndex));
        }
        else
        {
            NextWorld();
            isInMainMenu = true;
			SaveData();
            if (WidescreenCheck.IsWideScreen())
                LoadLevel(mainMenuName);
            else
                LoadLevel(mainMenuNameNonWide);
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
    private string mainMenuNameNonWide;

    [SerializeField]
    private List<GameWorld> worlds;

    private bool isInMainMenu = true;


    private GameWorld currentWorld;
     
    public int currentLevelIndex;

    private bool MoreLevelsInWorld
    {
        get
        {
            return currentWorld.Levels.Count > currentLevelIndex && currentLevelIndex!=-1;
        }
    }

    public void LoadLevel(string name)
    {
        if(InstanceFinder.GameManager.ActiveLevel != null)
            InstanceFinder.GameManager.ActiveLevel.OnNextLevel();
        Application.LoadLevel(name);
    }

    private void LoadLevelWithTransition(string levelName)
    {
        LevelToLoad = levelName;
        LoadLevel(transitionLevelName);
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
	
	void AddStatistics()
	{
		if(InstanceFinder.StatsManager!=null)
			InstanceFinder.StatsManager.GamesPlayed++;
	}
	
	// adding this to ensure that data is saved when a level change occurs
	void SaveData()
	{
		InstanceFinder.AchievementManager.SaveAchievements();
		InstanceFinder.StatsManager.Save();	
	}
}