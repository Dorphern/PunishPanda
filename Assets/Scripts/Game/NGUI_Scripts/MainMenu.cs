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
		menuMan.SwitchToMenu(MenuTypes.Achievements);
	}
	
	public void OnUnlocksClicked()
	{
		menuMan.SwitchToMenu(MenuTypes.Unlocks);
	}
	
	public void OnSettingsClicked()
	{
		menuMan.SwitchToMenu(MenuTypes.Settings);
	}
	
	
}
