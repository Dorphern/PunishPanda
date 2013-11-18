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
		InstanceFinder.LevelManager.LoadNextLevel();
	}
	
	void Start()
	{
		InstanceFinder.GameManager.ActiveLevel.onLevelComplete += OnLevelComplete;
	}
	
	
	
	private void OnLevelComplete()
	{
		//this is where calcualtions for score, stars and adding to lifetime score happens
		//  .GameManager.ActiveLevel.
		winScreen.SetActive(true);
	}
	
}
