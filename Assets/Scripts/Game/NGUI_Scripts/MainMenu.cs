using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	
	MenuManager menuMan;
	
	void Start()
	{
		menuMan = GetComponent<MenuManager>();	
	}
	
	public void OnPlayClicked()
	{
		//Debug.Log("Play Pressed!");	
		menuMan.SwitchToMenu (MenuTypes.Levels);
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
		menuMan.SwitchToMenu(MenuTypes.Settings);
	}
	
	
}
