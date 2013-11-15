using UnityEngine;
using System.Collections;

public class PauseMenuManager : MonoBehaviour {
	
	//Attach this script to Buttons(objects with box colliders)
	//in the In-Game-GUI
	PauseGame pausegame; 
	
	public GameObject PauseMenu; 
	public GameObject HintScreen;
	public GameObject LevelsScreen; 
	
	void Start()
	{
		pausegame = GetComponent<PauseGame>();
	}
	
	void OnClick()
	{
		//case for going In and out of HINT SCREEN
		if(gameObject.name == "GoalButtonOffset")
		{
	    	HintScreen.SetActiveRecursively(true);
	    	PauseMenu.SetActiveRecursively(false);
		}
		
		if(gameObject.name == "HintBackButton")
		{
			HintScreen.SetActiveRecursively(false);
	    	PauseMenu.SetActiveRecursively(true);
		}
		
		//case for going In and out of LEVELS SCREEN
		if(gameObject.name == "LevelsButtonOffset")
		{
			InstanceFinder.LevelManager.LoadLevelsMenu();
		}
		if(gameObject.name == "LevelsBackButton")
		{
			LevelsScreen.SetActiveRecursively(false);
	    	PauseMenu.SetActiveRecursively(true);
		}
		
		//case for going to MAIN MENU
		if(gameObject.name == "MainMenuButtonOffset")
		{
			OnMainMenuClick();
		}
		
	}
	
	
	public void OnMainMenuClick()
	{
		//Unpause game to get the normal TimeScale back
		pausegame.ResumeGame();
		InstanceFinder.LevelManager.LoadMainMenu();
	}
}
