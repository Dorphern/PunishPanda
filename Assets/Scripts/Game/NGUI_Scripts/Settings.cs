using UnityEngine;
using System.Collections;

public class Settings : MonoBehaviour {

	MenuManager menuMan;
	public UISlider _musicSlider;
	public UISlider _soundEFXSlider;
	
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
	
	public void OnLanguageClicked()
	{
		Debug.Log("Language Changed!");
	}
	
	// crappy workaround, there is no delegate that returns the value ffs!
	public void OnMusicSliderChanged()
	{
		if(_musicSlider!=null)
		{
			Debug.Log("music Slider val: " + _musicSlider.value);
		}
	}
	
	public void OnSoundEFXSliderChanged()
	{
		if(_musicSlider!=null)
		{
			Debug.Log("efx Slider val: " + _soundEFXSlider.value);
		}
	}
	
}
