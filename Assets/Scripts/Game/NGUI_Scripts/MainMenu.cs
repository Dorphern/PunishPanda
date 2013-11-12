using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	
	public GameObject settingsMenu;
	public GameObject mainMenu;
	
	public void OnPlayClicked()
	{
		Debug.Log("Play Pressed!");	
	}
	
	public void OnAchievementsClicked()
	{
		Debug.Log("Achievements Pressed!");	
	}
	
	public void OnUnlocksClicked()
	{
		Debug.Log("Unlocks Pressed!");	
	}
	
	public void OnSettingsClicked()
	{
		if(settingsMenu!=null)
		{
			//mainMenu.SetActive(false);
			//settingsMenu.SetActive(true);
		}
	}
	
}
