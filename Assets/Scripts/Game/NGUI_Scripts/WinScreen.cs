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
	    var levelManger = InstanceFinder.LevelManager;
		levelManger.LoadNextLevel();
	}
	
	void Start()
	{
		InstanceFinder.GameManager.ActiveLevel.onLevelComplete += OnLevelComplete;
	}
	
	
	
	private void OnLevelComplete()
	{
		//this is where calcualtions for score, stars and adding to lifetime score happens 
        //  .GameManager.ActiveLevel.
        #region Unlock next level & current level funfact
        var levelManger = InstanceFinder.LevelManager;
	    levelManger.CurrentLevel.UnlockedFunFact = true;
	    var levels = levelManger.CurrentWorld.Levels;
	    if (levels.Count - 1 > levelManger.CurrentLevelIndex)
	    {
	        levels[levelManger.CurrentLevelIndex + 1].UnlockedLevel = true;
        }
        #endregion

        winScreen.SetActive(true);
	}
	
}
