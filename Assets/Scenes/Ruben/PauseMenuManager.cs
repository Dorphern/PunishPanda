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
	public GameObject LevelNumberLabel;
	private UILabel levelNumberComponent;
	
	private UITexture textureComponent;
	private TweenAlpha hintAlphaComponent;
	private TweenAlpha tintAlphaComponent;
	private TweenAlpha whiteTintAlphaComponent;
	
		
	private bool MenuIsActive;
	private Texture2D HintTexture;
	private Texture2D TutorialTexture;
	
	
	
	void Start()
	{
		pausegame = GetComponent<PauseGame>();
		
		//get Level # for label
		levelNumberComponent = LevelNumberLabel.GetComponent<UILabel>();
		int lvlNumInt = InstanceFinder.LevelManager.CurrentLevelIndex;
		lvlNumInt ++;
		string levelNumString = lvlNumInt.ToString();
		levelNumberComponent.text = levelNumString;

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
			
				StartCoroutine(startTutorial ());
				//startTutorial();
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
	
	//called "onPress"
	public void OnHintReturnClick()
	{
		StartCoroutine(ExitTutorial());
		
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
	IEnumerator startTutorial()
	{
		// wait one update for data initialization
		yield return null;
		
		//pausegame.StopTime();
		pausegame.TutorialPause();
		PauseAndReset.SetActive (false);
	

		WhiteTint.SetActive(true);
		HintScreen.SetActive(true);
		
		hintAlphaComponent.Play();
		whiteTintAlphaComponent.Play();
	}
	
	IEnumerator ExitTutorial()
	{
		hintAlphaComponent.PlayReverse();
		whiteTintAlphaComponent.PlayReverse ();
			
		float fadeOutTime = Time.realtimeSinceStartup + 1;
    	while (Time.realtimeSinceStartup < fadeOutTime)
   		{
      		yield return 0;
		}
		hintAlphaComponent.Reset();
		whiteTintAlphaComponent.Reset ();
		WhiteTint.SetActive(false);
		HintScreen.SetActive(false);
	}
}
