using UnityEngine;
using System.Collections;

public class Settings : MonoBehaviour {

	MenuManager menuMan;
	public UISlider _musicSlider;
	public UISlider _soundEFXSlider;
	public UILabel languageLabel;
	
	void Awake()
	{
		menuMan = GetComponent<MenuManager>();	
		
		bool music = InstanceFinder.StatsManager.MusicEnabled;
		bool sound = InstanceFinder.StatsManager.SoundEffectsEnabled;
		string lang = InstanceFinder.StatsManager.language;
		
		if(music)
			_musicSlider.value = 1;
		else
			_musicSlider.value = 0;
		
		if(sound)
			_soundEFXSlider.value = 1;
		else
			_soundEFXSlider.value = 0;
		
		if(languageLabel!=null)
		{
			bool initflag = true;
			for(int i=0;i<InstanceFinder.Localization.languages.Length; i++)
			{
				if(InstanceFinder.Localization.languages[i].name==lang)
				{
					languageLabel.text = lang;
					initflag = false;
				}
			}
			//if the language file is not found default to the default value
			if(initflag)
			{
				languageLabel.text = InstanceFinder.StatsManager.language;
			}
		}
		// language button initialization goes here
		
	}
	
	public void OnCalibrateFingerClicked()
	{
		menuMan.SwitchToMenu(MenuTypes.Finger);
	}
	
	public void OnCreditsClicked()
	{
		Debug.Log("Credits!");
	}
	
	public void OnReturnClicked()
	{
		menuMan.SwitchToMenu(MenuTypes.MainMenu);
	}
	
	// placeholder code for the placeholder button
	public void OnLanguageClicked()
	{
		if(languageLabel!=null)
		{
			for(int i=0;i<InstanceFinder.Localization.languages.Length; i++)
			{
				if(InstanceFinder.Localization.languages[i].name!=languageLabel.text)
				{
					if(InstanceFinder.Localization.languages.Length == i-1)
					{
						InstanceFinder.Localization.currentLanguage = InstanceFinder.Localization.languages[0].name;
						languageLabel.text = InstanceFinder.Localization.languages[0].name;
						InstanceFinder.StatsManager.language = InstanceFinder.Localization.languages[0].name;
						InstanceFinder.StatsManager.Save();
						break;
					}
					else
					{
						InstanceFinder.Localization.currentLanguage = InstanceFinder.Localization.languages[i].name;
						languageLabel.text = InstanceFinder.Localization.languages[i].name;
						InstanceFinder.StatsManager.language = InstanceFinder.Localization.languages[i].name;
						InstanceFinder.StatsManager.Save();
						break;
					}
				}
			}
		}
	}
			
	
	// crappy workaround, there is no delegate that returns the value ffs!
	public void OnMusicSliderChanged()
	{
		if(_musicSlider!=null)
		{
			if(_musicSlider.value==0)
			{
				InstanceFinder.StatsManager.MusicEnabled = false;
				InstanceFinder.StatsManager.Save();
			}
			else
			{
				InstanceFinder.StatsManager.MusicEnabled = true;
				InstanceFinder.StatsManager.Save();
			}
		}
	}
	
	public void OnSoundEFXSliderChanged()
	{
		if(_musicSlider!=null)
		{
			if(_soundEFXSlider.value==0)
			{
				Debug.Log ("Sound OFF");
				InstanceFinder.StatsManager.SoundEffectsEnabled = false;
				InstanceFinder.StatsManager.Save();
			}
			else
			{
				Debug.Log ("Sound ON");
				InstanceFinder.StatsManager.SoundEffectsEnabled = true;
				InstanceFinder.StatsManager.Save();
			}
		}
	}
	
}
