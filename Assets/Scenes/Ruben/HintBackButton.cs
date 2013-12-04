using UnityEngine;
using System.Collections;

public class HintBackButton : MonoBehaviour {

	// Use this for initialization
	public PauseMenuManager pauseMenuManager; 
	public PauseGame pauseGame;
	
	void Start () {
	
	}
	
	public void OnPress(bool isDown)
	{
	    if(isDown)
	    {
			//call PauseMenuManger function: OnHintReturnClick
			pauseMenuManager.OnHintReturnClick();
			//resume game
			pauseGame.ResumeGame();
	    }
	 
	    if(!isDown)
	    {

	    }
	} 
	
	
}
