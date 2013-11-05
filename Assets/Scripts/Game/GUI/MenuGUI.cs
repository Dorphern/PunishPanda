using UnityEngine;
using System.Collections;

public class MenuGUI : MonoBehaviour {
    public void Initialize(LevelManager levelManager)
    {
        this.levelManager = levelManager;
    }

    private LevelManager levelManager;

    void OnGUI()
    {
        if (!levelManager.IsLoadingLevel)
        {
            if (levelManager.IsInMainMenu)
            {
                Rect position = new Rect(50, 50, 100, 80);
                for (int i = 0; i < levelManager.WorldCount; i++)
                {
                    var world = levelManager.GetWorld(i);
                    if (GUI.Button(position, world.WorldName))
                    {
                        levelManager.CurrentWorld = world;
                    }
                    position.x += 150;
                }

                Rect levelPosition = new Rect(50, 150, 100, 80);
                for (int i = 0; i < levelManager.CurrentWorld.Levels.Count; i++)
                {
                    var level = levelManager.CurrentWorld.Levels[i];
                    if (GUI.Button(levelPosition, "Load " + level.LevelName))
                    {
                        levelManager.LoadLevelByWorldIndex(i);
                    }
                    levelPosition.x += 150;
                }
            }
        }
    }
}
