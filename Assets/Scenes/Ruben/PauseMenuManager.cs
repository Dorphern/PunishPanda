using UnityEngine;
using System.Collections;

public class PauseMenuManager : MonoBehaviour {
	
	public GameObject PauseMenu; 
	public GameObject HintScreen;
	public GameObject LevelsScreen; 
	
	void Start()
	{
	    //HintScreen = GameObject.Find("Hint(Goal)Screen");
	    //PauseMenu = GameObject.Find("PauseMENU");
		//HintScreen.SetActiveRecursively(false);
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
			LevelsScreen.SetActiveRecursively(true);
	    	PauseMenu.SetActiveRecursively(false);
		}
		if(gameObject.name == "LevelsBackButton")
		{
			LevelsScreen.SetActiveRecursively(false);
	    	PauseMenu.SetActiveRecursively(true);
		}
	}
	
}
