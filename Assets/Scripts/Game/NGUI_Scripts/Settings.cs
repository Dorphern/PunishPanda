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
		
		Debug.Log ("STARTEDsttngs");
		//GET THE LOADED VALUES
		music = InstanceFinder.StatsManager.MusicEnabled;
		sound = InstanceFinder.StatsManager.SoundEffectsEnabled;
		//string lang = InstanceFinder.StatsManager.language;
		
		
		//initialization flags
//		MusicAnimationFlag = true;
//		SFXAnimationFlag = true;
		
		
		if(sound)
		{
			SFX_animation.Play ("leverAnimation2");//goleft
		}
		else
		{
			SFX_animation.Play ("leverAnimation1");//goright
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
			Debug.Log(InstanceFinder.Localization.languages.Length);
			for(int i=0;i<InstanceFinder.Localization.languages.Length; i++)
			{
				Debug.Log(InstanceFinder.Localization.languages[i].name);
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
		//Debug.Log ("got SFX: "+_soundEFXSlider.value);
		return _soundEFXSlider.value;
	}
	
	//Gets called AFTER a slider toggle
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
//			Debug.Log ("SFXAnimationFlag: "+SFXAnimationFlag);
			InstanceFinder.SoundSettings.OnSFXEnable();
//			if(SFXAnimationFlag == false)
//			{
//				SFXAnimationFlag = true;
//				//dont animate on 2nd call for ON;
//				Debug.Log ("didnt animate left");
//				return;
//				
//			}
//			SFX_animation.Play ("leverAnimation2");//goleft
		}
		else
		{
//			Debug.Log ("SFXAnimationFlag: "+SFXAnimationFlag);
			InstanceFinder.SoundSettings.OnSFXDisable();
//			if(SFXAnimationFlag == false)
//			{
//				SFXAnimationFlag = true;
//				//dont animate on 2nd call for ON;
//				return;
//				
//			}
//			SFX_animation.Play ("leverAnimation1");//goright	
		}
		

	}
	
	
	
	
	public float getMusicvalue()
	{
		//Debug.Log ("got Mus: "+ _musicSlider.value);
		return _musicSlider.value;
	}
	
	
	// crappy workaround, there is no delegate that returns the value ffs!
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
		}
		else
		{
			InstanceFinder.SoundSettings.OnMusicDisable();
		}

				

	}
	
	
	
	public void firstTimeSFX()
	{
		Debug.Log ("Initializing Sound: "+sound);
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
		Debug.Log ("Initializing Music: "+music);
		if(music)
			_musicSlider.value = 0;
		else
			_musicSlider.value = 1;
		
		

	}
}
