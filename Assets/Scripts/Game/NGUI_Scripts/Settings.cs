using UnityEngine;
using System.Collections;

public class Settings : MonoBehaviour {

	MenuManager menuMan;
	public UISlider _musicSlider;
	public UISlider _soundEFXSlider;
	
	public GameObject SFX_Lever;
	public GameObject Music_Lever;
	private Animation SFX_animation;
	private Animation Music_animation;
	private int MusicAnimationFlag;
	private int SFXAnimationFlag;
	
	void Start()
	{
		menuMan = GetComponent<MenuManager>();
		SFX_animation = SFX_Lever.GetComponent<Animation>();
		Music_animation = Music_Lever.GetComponent<Animation>();
		
		
		bool music = InstanceFinder.StatsManager.MusicEnabled;
		bool sound = InstanceFinder.StatsManager.SoundEffectsEnabled;
		//string lang = InstanceFinder.StatsManager.language;
		

		if(music)
			_musicSlider.value = 1;
		else
			_musicSlider.value = 0;

		
		if(sound)
			_soundEFXSlider.value = 1;
		else
			_soundEFXSlider.value = 0;

		

		
		//initialize Switches
		MusicAnimationFlag = 0;
		SFXAnimationFlag = 0;
		OnSoundEFXSliderChanged();
		OnMusicSliderChanged();
	}
	
	public void OnCalibrateFingerClicked()
	{
		menuMan.SwitchToMenu(MenuTypes.Finger);
	}
	
	public void OnCreditsClicked()
	{
		//Debug.Log("Credits!");
		menuMan.SwitchToMenu (MenuTypes.Credits);
	}
	
	public void OnReturnClicked()
	{
		InstanceFinder.StatsManager.Save();
		menuMan.SwitchToMenu(MenuTypes.MainMenu);
	}
	
	public void OnEnglishClicked()
	{
		if(InstanceFinder.Localization.currentLanguage!="English")
		{
			for(int i=0;i<InstanceFinder.Localization.languages.Length; i++)
			{
				if(InstanceFinder.Localization.languages[i].name=="English")
				{
					InstanceFinder.Localization.currentLanguage = InstanceFinder.Localization.languages[i].name;
					InstanceFinder.StatsManager.language = InstanceFinder.Localization.languages[i].name;
					InstanceFinder.StatsManager.Save();
					break;	
				}
			}
		}
	}
	
	public void OnDanishClicked()
	{
		
		if(InstanceFinder.Localization.currentLanguage!="Danish")
		{
			Debug.Log(InstanceFinder.Localization.languages.Length);
			for(int i=0;i<InstanceFinder.Localization.languages.Length; i++)
			{
				Debug.Log(InstanceFinder.Localization.languages[i].name);
				if(InstanceFinder.Localization.languages[i].name=="Danish")
				{
					
					InstanceFinder.Localization.currentLanguage = InstanceFinder.Localization.languages[i].name;
					InstanceFinder.StatsManager.language = InstanceFinder.Localization.languages[i].name;
					InstanceFinder.StatsManager.Save();
					break;	
				}
			}
		}
	}
			
	
	// crappy workaround, there is no delegate that returns the value ffs!
	public void OnMusicSliderChanged()
	{
		if(_musicSlider!=null)
		{
			//hack for dealing with extra animation (when entering settings)
			if(MusicAnimationFlag == 1)
			{	
				MusicAnimationFlag++;
				return;
			}
			else
				MusicAnimationFlag++;
			
			if(_musicSlider.value==0)
			{
				
				Music_animation.Play ("leverAnimation2");//goleft
	
				InstanceFinder.StatsManager.MusicEnabled = false;
				//Debug.Log ("Music OFF");
			}
			else
			{

				Music_animation.Play ("leverAnimation1");//goright

				InstanceFinder.StatsManager.MusicEnabled = true;
				//Debug.Log ("Music ON");
			}
		}
	}
	
	public void OnSoundEFXSliderChanged()
	{
		if(_soundEFXSlider!=null)
		{
			//hack for dealing with extra animation (when entering settings)
			if(SFXAnimationFlag == 1)
			{	
				SFXAnimationFlag++;
				return;
			}
			else
				SFXAnimationFlag++;
			
			if(_soundEFXSlider.value==0)
			{
				SFX_animation.Play ("leverAnimation2");//goleft

				InstanceFinder.StatsManager.SoundEffectsEnabled = false;
				//Debug.Log ("Sound OFF");
			}
			else
			{
				SFX_animation.Play ("leverAnimation1");//goright		
				
				InstanceFinder.StatsManager.SoundEffectsEnabled = true;
				//Debug.Log ("Sound ON");
			}
		}
	}
	
}
