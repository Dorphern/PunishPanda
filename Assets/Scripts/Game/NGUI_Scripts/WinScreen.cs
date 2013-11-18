using UnityEngine;
using System.Collections;

public class WinScreen : MonoBehaviour {
	
	public GameObject winScreen;
	
	public void OnLevelsButtonClicked()
	{
		InstanceFinder.LevelManager.LoadLevelsMenu();
	}
	
	public void OnRestartButtonClicked()
	{
		InstanceFinder.LevelManager.Reload();
	}
	
	public void OnNextLevelButtonClicked()
	{
        var levelManager = InstanceFinder.LevelManager;
        levelManager.LoadNextLevel();
	}
	
	void Start()
	{
		InstanceFinder.GameManager.ActiveLevel.onLevelComplete += OnLevelComplete;
	}
	
	
	
	private void OnLevelComplete()
	{
	    //this is where calcualtions for score, stars and adding to lifetime score happens 
        //  .GameManager.ActiveLevel.
        
	    UnLockLevels();

        winScreen.SetActive(true);
	}

    private static void UnLockLevels()
    {
        var levelManager = InstanceFinder.LevelManager;
        levelManager.CurrentLevel.UnlockedFunFact = true;
        var levels = levelManager.CurrentWorld.Levels;
        if (levels.Count - 1 > levelManager.CurrentLevelIndex)
        {
            levels[levelManager.CurrentLevelIndex + 1].UnlockedLevel = true;
        }
        InstanceFinder.StatsManager.Save();
    }
}
