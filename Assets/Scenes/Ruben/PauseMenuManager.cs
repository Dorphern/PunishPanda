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
	public GameObject WhiteTint;
	
	private UITexture textureComponent;
	private TweenAlpha hintAlphaComponent;
	private TweenAlpha tintAlphaComponent;
	private TweenAlpha whiteTintAlphaComponent;
	private bool skippedTutorial;
	
		
	private bool MenuIsActive;
	private Texture2D HintTexture;
	private Texture2D TutorialTexture;
	
	
	
	void Start()
	{
		pausegame = GetComponent<PauseGame>();
		


		//get components
		textureComponent = HintObj.GetComponent<UITexture>();
		hintAlphaComponent = HintObj.GetComponent<TweenAlpha>();
		tintAlphaComponent = PauseTint.GetComponent<TweenAlpha>();
		whiteTintAlphaComponent = WhiteTint.GetComponent<TweenAlpha>();
		
		if(!InstanceFinder.GameManager.debugMode)
		{
			//get start-tutorial texture:
	    	if (Localization.instance.currentLanguage == "English")
	    	{
            	HintTexture = InstanceFinder.LevelManager.CurrentLevel.HintscreenTexture;
	        	TutorialTexture = InstanceFinder.LevelManager.CurrentLevel.TutorialTexture;
	    	}
	    	else
	    	{
            	HintTexture = InstanceFinder.LevelManager.CurrentLevel.DanishHintscreenTexture;
	        	TutorialTexture = InstanceFinder.LevelManager.CurrentLevel.DanishTutorialTexture;
	    	}
			
			//set start-tutorial texture to component
			textureComponent.mainTexture = TutorialTexture;
			//set it to its native-dimensions
			if(textureComponent.mainTexture != null)
			{
				textureComponent.width = TutorialTexture.width;
				textureComponent.height = TutorialTexture.height;
			
	
				skippedTutorial = false;
			
				StartCoroutine(showTutorial());
			}
		}
	}
	
	public void OnPauseClick()
	{
		//Enable Screen tint
		PauseTint.SetActive(true);
		tintAlphaComponent.PlayForward();
		//Enable PauseMENU
		PauseMenu.SetActive (true);
		MenuIsActive = true;
		
	}
	
	public void OnResumeClick()
	{
		tintAlphaComponent.Reset();
		PauseTint.SetActive(false);
		MenuIsActive = false;
	}
	
	public void OnHintClick()
	{

		//set menu-hint texture
		textureComponent.mainTexture = HintTexture;
		if(textureComponent.mainTexture != null)
		{
			textureComponent.width = HintTexture.width;
			textureComponent.height = HintTexture.height;
			HintScreen.SetActive(true);
	    	PauseMenu.SetActive(false);
			tintAlphaComponent.Reset();
			PauseTint.SetActive(false);
			WhiteTint.SetActive(true);
		
		
			hintAlphaComponent.PlayForward();
			whiteTintAlphaComponent.PlayForward();
		}
		else
		{
			Debug.Log ("A Hint picture is NOT setup for this level!");
		}
			
	}
	
	public void OnHintReturnClick()
	{
		skippedTutorial = true;
		
		hintAlphaComponent.Reset();
		whiteTintAlphaComponent.Reset ();
	
		HintScreen.SetActive(false);
		WhiteTint.SetActive(false);
		PauseAndReset.SetActive (true);

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
	
		//short start delay
		float startDelay = Time.realtimeSinceStartup + 0.5f;
    	while (Time.realtimeSinceStartup < startDelay)
    	{
        	yield return 0;
    	}
		WhiteTint.SetActive(true);
		HintScreen.SetActive(true);
		
		hintAlphaComponent.Play();
		whiteTintAlphaComponent.Play();

		float pauseEndTime = Time.realtimeSinceStartup + 3;
    	while (Time.realtimeSinceStartup < pauseEndTime)
    	{
        	yield return 0;
    	}

		//TIMED FADEOUT:
//		if(skippedTutorial == false)
//		{
//			hintAlphaComponent.PlayReverse();
//			whiteTintAlphaComponent.PlayReverse ();
//			
//			float fadeOutTime = Time.realtimeSinceStartup + 2;
//    		while (Time.realtimeSinceStartup < fadeOutTime)
//    		{
//        		yield return 0;
//    		}
//		
//			pausegame.ResumeGame();
//			PauseAndReset.SetActive (true);
//			WhiteTint.SetActive(false);
//			HintScreen.SetActive(false);
//		}

	}
}
