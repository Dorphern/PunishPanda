using UnityEngine;
using System.Collections;

public class GameGUI : MonoBehaviour {
    public void Initialize(LevelManager levelManager)
    {
        _levelManager = levelManager;
    }

    private LevelManager _levelManager;

    void OnGUI()
    {
        if (!_levelManager.IsLoadingLevel)
        {
            if (_levelManager.IsInMainMenu)
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
                        _levelManager.LoadLevelByWorldIndex(i);
                    }
                    levelPosition.x += 150;
                }
            }
        }
    }
}
