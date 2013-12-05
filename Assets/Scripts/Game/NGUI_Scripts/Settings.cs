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
					break;	
				}
			}
		}
	}
	
	public void OnDanishClicked()
	{
		
		if(InstanceFinder.Localization.currentLanguage!="Danish")
		{
			for(int i=0;i<InstanceFinder.Localization.languages.Length; i++)
			{
				if(InstanceFinder.Localization.languages[i].name=="Danish")
				{
					
					InstanceFinder.Localization.currentLanguage = InstanceFinder.Localization.languages[i].name;
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
			
			if(_musicSlider.value==1)
			{
						
				Music_animation.Play ("leverAnimation1");//goright
				InstanceFinder.StatsManager.MusicEnabled = false;
			}
			else
			{

				Music_animation.Play ("leverAnimation2");//goleft
				InstanceFinder.StatsManager.MusicEnabled = true;
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
			
			if(_soundEFXSlider.value==1)
			{
				
				SFX_animation.Play ("leverAnimation1");//goright
				InstanceFinder.StatsManager.SoundEffectsEnabled = false;
			}
			else
			{
					
				SFX_animation.Play ("leverAnimation2");//goleft
				InstanceFinder.StatsManager.SoundEffectsEnabled = true;
			}
		}
	}
	
}
