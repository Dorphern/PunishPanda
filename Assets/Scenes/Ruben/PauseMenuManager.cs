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
	private UITexture textureComponent;
	private TweenAlpha hintAlphaComponent;
	private TweenAlpha tintAlphaComponent;
		
	private bool MenuIsActive;
	private Texture2D HintTexture;
	private Texture2D TutorialTexture;
	
	
	
	void Start()
	{
		pausegame = GetComponent<PauseGame>();

		//get start-tutorial texture:
		TutorialTexture = InstanceFinder.LevelManager.CurrentLevel.TutorialTexture;
		//get menu-hint texture:
		HintTexture = InstanceFinder.LevelManager.CurrentLevel.HintscreenTexture;
		
		
		//get components
		textureComponent = HintObj.GetComponent<UITexture>();
		hintAlphaComponent = HintObj.GetComponent<TweenAlpha>();
		tintAlphaComponent = PauseTint.GetComponent<TweenAlpha>();
		
		//set start-tutorial texture to component
		textureComponent.mainTexture = TutorialTexture;
		//set it to its native-dimensions
		textureComponent.width = 1024;
		textureComponent.height = 1024;
		

		//display start-tutorial texture (if it has one..)
		StartCoroutine(showTutorial());
		
		
		
		
		

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
		//set menu-hint texture
		textureComponent.mainTexture = HintTexture;
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
	
	//for showhing tutorial screen/animation at levelstart
	IEnumerator showTutorial()
	{
		
		
		pausegame.StopTime();
		PauseAndReset.SetActive (false);
		PauseTint.SetActive(true);
		HintScreen.SetActive(true);
		
		hintAlphaComponent.Play();
		tintAlphaComponent.Play();
		
		Debug.Log ("waiting..");
		float pauseEndTime = Time.realtimeSinceStartup + 3;
    	while (Time.realtimeSinceStartup < pauseEndTime)
    	{
        	yield return 0;
    	}
		Debug.Log ("DONE WAITING");
		
		//play fade out and wait for it to finish.
		hintAlphaComponent.PlayReverse();
		tintAlphaComponent.PlayReverse();
		float fadeOutTime = Time.realtimeSinceStartup + 1;
    	while (Time.realtimeSinceStartup < fadeOutTime)
    	{
        	yield return 0;
    	}
		
		
		pausegame.ResumeGame();
		PauseAndReset.SetActive (true);
		PauseTint.SetActive(false);
		HintScreen.SetActive(false);
		
	}
}
