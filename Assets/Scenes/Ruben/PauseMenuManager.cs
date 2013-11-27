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
	public GameObject PauseAndReset;
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
		PauseTint.SetActive(false);
		PauseAndReset.SetActive (true);
	    //PauseMenu.SetActive(true);
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

	
	public void DisablePauseMenu()
	{
		if(MenuIsActive == false)
			PauseMenu.SetActive (false);
	}
	
}
