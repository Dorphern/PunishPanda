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
            return _currentWorld.Levels[_currentLevelIndex];
        }
    }

    public int CurrentLevelIndex
    {
        get
        {
            return _currentLevelIndex;
        }
    }

    public GameWorld CurrentWorld
    {
        get
        {
            return _currentWorld;
        }
        set
        {
            if (value != null)
            {
                _currentWorld = value;
                _currentLevelIndex = -1;
            }
        }
    }

    //public void LoadLevelByName(string levelName)
    //{
    //    LevelToLoad = levelName;
    //    Application.LoadLevel(_transitionLevelName);
    //}

    public string LevelToLoad
    {
        get; private set;
    }

    public void LoadLevelByIndex(int index)
    {
        if (_currentWorld.Levels.Count > index)
        {
            _currentLevelIndex = index;
            LoadLevelWithTransition(_currentWorld.Levels[index].LevelName);
        }
    }

    public void LoadNextLevel()
    {
        if (MoreLevelsInWorld)
        {
            ++_currentLevelIndex;

            LoadLevelWithTransition(_currentWorld.Levels[_currentLevelIndex].LevelName);
        }
        else
        {
            NextWorld();
            LoadLevelWithTransition("MainMenu");
        }
    }

    public void NextWorld()
    {
        int index = _worlds.FindIndex(_currentWorld);
        if (index + 1 < _worlds.Count)
        {
            CurrentWorld = _worlds[index + 1];
        }
        else
        {
            _currentLevelIndex = -1;
        }
    }

    private void LoadLevelWithTransition(string levelName)
    {
        LevelToLoad = levelName;
        Application.LoadLevel(_transitionLevelName);
    }

    public bool MoreLevelsInWorld
    {
        get
        {
            return _currentWorld.Levels.Count > _currentLevelIndex + 1;
        }
    }

    public GameWorld GetWorld(int index)
    {
        return _worlds[index];
    }

    public int WorldCount
    {
        get { return _worlds.Count; }
    }

    void Awake()
    {
        _currentWorld = _worlds[0];
        _currentLevelIndex = -1;
    }

    [SerializeField]
    private string _transitionLevelName;

    [SerializeField]
    private List<GameWorld> _worlds;

    [NonSerialized]
    private GameWorld _currentWorld;
    [NonSerialized]
    private int _currentLevelIndex;
}