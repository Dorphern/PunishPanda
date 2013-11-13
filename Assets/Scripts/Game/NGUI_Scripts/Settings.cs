using UnityEngine;
using System.Collections;

public class Settings : MonoBehaviour {

	MenuManager menuMan;
	
	void Start()
	{
		menuMan = GetComponent<MenuManager>();	
	}
	
	public void OnCalibrateFingerClicked()
	{
		Debug.Log("Calibrate!");
	}
	
	public void OnCreditsClicked()
	{
		Debug.Log("Credits!");
	}
	
	public void OnReturnClicked()
	{
		menuMan.SwitchToMenu(MenuTypes.MainMenu);
	}
}
