using UnityEngine;
using System.Collections;

public class Settings : MonoBehaviour {
	
	public GameObject settingsMenu;
	public GameObject mainMenu;
	
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
		settingsMenu.SetActive(false);
		mainMenu.SetActive(true);
	}
}
