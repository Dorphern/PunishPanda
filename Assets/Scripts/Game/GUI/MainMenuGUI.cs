using UnityEngine;
using System.Collections;

public class MainMenuGUI : MonoBehaviour {
    public void Initialize(LevelManager levelManager)
    {
        _levelManager = levelManager;
    }

    private LevelManager _levelManager;


    void OnGUI()
    {
        Rect position = new Rect(50, 50, 100, 80);
        for (int i = 0; i < _levelManager.WorldCount; i++)
        {
            var world = _levelManager.GetWorld(i);
            if (GUI.Button(position, world.WorldName))
            {
                _levelManager.CurrentWorld = world;
            }
            position.x += 150;
        }

        Rect levelPosition = new Rect(50, 150, 100, 80);
        for (int i = 0; i < _levelManager.CurrentWorld.Levels.Count; i++)
        {
            var level = _levelManager.CurrentWorld.Levels[i];
            if (GUI.Button(levelPosition, "Load " + level.LevelName))
            {
                _levelManager.LoadLevelByIndex(i);
            }
            levelPosition.x += 150;
        }

        Rect nextRect = new Rect(Screen.width - 150, 50, 100, 80);
        if (GUI.Button(nextRect, "Next level"))
        {
            _levelManager.LoadNextLevel();
        }
    }

    
    
}
