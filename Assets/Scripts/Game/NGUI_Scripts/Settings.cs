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
	private bool MusicAnimationFlag;
	private bool SFXAnimationFlag;
	private bool firstSFX;
	private bool firstMusic;
	
	private bool sound;
	private bool music;
		
	void Start()
	{
		menuMan = GetComponent<MenuManager>();
		SFX_animation = SFX_Lever.GetComponent<Animation>();
		Music_animation = Music_Lever.GetComponent<Animation>();
		
		//GET THE LOADED VALUES
		music = InstanceFinder.StatsManager.MusicEnabled;
		sound = InstanceFinder.StatsManager.SoundEffectsEnabled;
		//string lang = InstanceFinder.StatsManager.language;
		
		
		//flags for skipping animation when we enter settings
		MusicAnimationFlag = true;
		SFXAnimationFlag = true;
		
		
		if(sound)
		{
			SFX_animation.Play ("leverAnimation2");//goleft
			SFXAnimationFlag = false;
		}
		else
		{
			SFX_animation.Play ("leverAnimation1");//goright
		}
		
		if(music)
		{
			Music_animation.Play ("leverAnimation2");//goleft
			MusicAnimationFlag = false;
		}
		else
		{
			Music_animation.Play ("leverAnimation1");//goright
		}

		
		firstSFX = true;
		firstMusic = true;
		

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
			
	

	
	
	public float getSFXvalue()
	{
		return _soundEFXSlider.value;
	}
	
	public void OnSoundEFXSliderChanged()
	{
		if(firstSFX	== true)
		{	
			firstSFX = false;
			firstTimeSFX();
			return;
		}

		float temp = getSFXvalue ();

		if(temp == 0)//left
		{
			InstanceFinder.SoundSettings.OnSFXEnable();
			if(SFXAnimationFlag == false)
			{
				SFXAnimationFlag = true;
				//dont animate on 2nd call for ON;
				return;
			}
			else
			{
				SFX_animation.Play ("leverAnimation2");//goleft
			}
		}
		else
		{
			InstanceFinder.SoundSettings.OnSFXDisable();
			if(SFXAnimationFlag == false)
			{
				SFXAnimationFlag = true;
				return;
			}
			else
			{
				SFX_animation.Play ("leverAnimation1");//goright
			}
		}
	}
	
		
	public float getMusicvalue()
	{
		return _musicSlider.value;
	}
	
	
	public void OnMusicSliderChanged()
	{

		if(firstMusic	== true)
		{	
			firstMusic = false;
			firstTimeMusic();
			return;
		}
		
		float temp = getMusicvalue ();

		if(temp == 0)//left
		{
			InstanceFinder.SoundSettings.OnMusicEnable();
			if(MusicAnimationFlag == false)
			{
				MusicAnimationFlag = true;
				//dont animate call for ON;
				return;	
			}
			else
			{
				Music_animation.Play ("leverAnimation2");//goleft
			}
		}
		else
		{
			InstanceFinder.SoundSettings.OnMusicDisable();
			if(MusicAnimationFlag == false)
			{
				MusicAnimationFlag = true;
				return;
			}
			else
			{
				Music_animation.Play ("leverAnimation1");//goright
			}
		}
	}
	
	
	
	public void firstTimeSFX()
	{
		//Debug.Log ("Initializing Sound: "+sound);
		if(sound)
		{
			_soundEFXSlider.value = 0; //point left		
		}
		else
		{
			_soundEFXSlider.value = 1; //point right
		}		
	}
	
	private void firstTimeMusic()
	{
		//Debug.Log ("Initializing Music: "+music);
		if(music)
			_musicSlider.value = 0; //point left
		else
			_musicSlider.value = 1;
	}
	
}
