using UnityEngine;
using System.Collections;

public class PauseMenuManager : MonoBehaviour {
	
	//Attach this script to Buttons(objects with box colliders)
	//in the In-Game-GUI
	PauseGame pausegame; 
	
	public GameObject PauseMenu;
	public GameObject PauseTint;
	public GameObject HintScreen;
	public GameObject HintObj;
	private UITexture HintComponent;
	
	private bool MenuIsActive;
	private Texture2D HintTexture;
	
	
	void Start()
	{
		pausegame = GetComponent<PauseGame>();
		//get hint texture:
		HintTexture = InstanceFinder.LevelManager.CurrentLevel.HintscreenTexture;
		UITexture HintComponent = HintObj.GetComponent<UITexture>();
		
		HintComponent.mainTexture = HintTexture;
		
	}
	
	public void OnPauseClick()
	{
		//Enable Screen tint
		PauseTint.SetActive(true);
		//Enable PauseMENU
		PauseMenu.SetActive (true);
		MenuIsActive = true;
		
	}
	
	public void OnResumeClick()
	{
		PauseTint.SetActive(false);
		MenuIsActive = false;
	}
	
	public void OnHintClick()
	{
		HintScreen.SetActive(true);
	    PauseMenu.SetActive(false);
	}
	
	public void OnHintReturnClick()
	{
		HintScreen.SetActive(false);
	    PauseMenu.SetActive(true);
	}
	
	
	
	public void OnLevelsClick()
	{
		//Unpause game to get the normal TimeScale back
		pausegame.ResumeGame();
		InstanceFinder.LevelManager.LoadLevelsMenu();
	}
	
	public void OnMainMenuClick()
	{
		//Unpause game to get the normal TimeScale back
		pausegame.ResumeGame();
		InstanceFinder.LevelManager.LoadMainMenu();
	}

	//BUTTON HANDLER:
	//NOTE -- MAY NOT NEED THIS ANYMORE....(because using UIbuttons)
	void OnClick()
	{
		//case for hitting PAUSE
		if(gameObject.name == "PauseSprite")
		{
			OnPauseClick();
		}
		
		//case for RESUME click
		if(gameObject.name == "ResumeButtonOffset")
		{
			OnResumeClick();
		}
		
		//case for going In and out of HINT SCREEN
		if(gameObject.name == "GoalButtonOffset")
		{
			OnHintClick ();
		}
		
		if(gameObject.name == "HintBackButton")
		{
			HintScreen.SetActive(false);
	    	PauseMenu.SetActive(true);
		}
		
		//case for going to LEVELS SCREEN
		if(gameObject.name == "LevelsButtonOffset")
		{
			OnLevelsClick();
		}
		
		//case for going to MAIN MENU
		if(gameObject.name == "MainMenuButtonOffset")
		{
			OnMainMenuClick();
		}
		
	}
	
	public void DisablePauseMenu()
	{
		if(MenuIsActive == false)
			PauseMenu.SetActive (false);
	}
	
}
