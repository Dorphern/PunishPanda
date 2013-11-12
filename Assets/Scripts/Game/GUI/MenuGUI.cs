using UnityEngine;
using System.Collections;

public class MenuGUI : MonoBehaviour {
    public void Initialize(LevelManager levelManager)
    {
        this.levelManager = levelManager;
    }

    private LevelManager levelManager;
	
	/*
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
                    string worldName = LanguageManager.Instance.GetTextValue("Menu.World");
                    if (GUI.Button(position, worldName + i))
                    {
                        levelManager.CurrentWorld = world;
                    }
                    position.x += 150;
                }

                Rect levelPosition = new Rect(50, 150, 100, 80);
                string loadName = LanguageManager.Instance.GetTextValue("Menu.LoadLevel");
                for (int i = 0; i < levelManager.CurrentWorld.Levels.Count; i++)
                {
                    var level = levelManager.CurrentWorld.Levels[i];
                    if (GUI.Button(levelPosition, loadName + i))
                    {
                        levelManager.LoadLevelByWorldIndex(i);
                    }
                    levelPosition.x += 150;
                }

                Rect languagePosition = new Rect(Screen.width - 150, 50, 100, 80);
                string english = LanguageManager.Instance.GetTextValue("Language.English");
                if (GUI.Button(languagePosition, english))
                {
                    LanguageManager.Instance.ChangeLanguage("en");
                }

                languagePosition.y += 150;
                string danish = LanguageManager.Instance.GetTextValue("Language.Danish");
                if (GUI.Button(languagePosition, danish))
                {
                    LanguageManager.Instance.ChangeLanguage("da");
                }
            }
        }
    }
    */
}
