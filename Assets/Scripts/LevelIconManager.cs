using UnityEngine;
using System.Collections;

public class LevelIconManager : MonoBehaviour {
	
	//Handler for the LEVELS SCREEN that checks how many
	//stars should be shown on a Level Icon.
	
    public int LevelNumber;
	public bool isUnlocked;
	
	public GameObject first;
	public GameObject second;
	public GameObject third;
	private UISprite firstSprite;
	private UISprite secondSprite;
	private UISprite thirdSprite;
	
	//More functionality:	
	public GameObject unlockedLabel;
	public GameObject lockedLabel;
	private UILabel Number;

	
	void Start () {
		firstSprite = first.GetComponent<UISprite>();
		secondSprite = second.GetComponent<UISprite>();
		thirdSprite = third.GetComponent<UISprite>();

	    string levelNumberString = LevelNumber.ToString();
		Number = unlockedLabel.GetComponent<UILabel>();
        Number.text = levelNumberString;
		Number = lockedLabel.GetComponent<UILabel>();
		Number.text = levelNumberString;
		
		
		var levels = InstanceFinder.LevelManager.CurrentWorld.Levels;
		int stars;
		
		if(LevelNumber <= levels.Count )
		{
			isUnlocked = levels[LevelNumber-1].UnlockedLevel;
			//STAR CALCULATION:
			stars = PunishPanda.Game.ScoreCalculator.Stars(levels[LevelNumber - 1], levels[LevelNumber-1].HighScore);
			//Debug.Log ("Level:"+LevelNumber+" has "+stars+" stars");
			if(stars == 1)
			{
				show1star();
			}
			else if(stars == 2)
			{
				show2stars();
			}
			else if(stars == 3)
			{
				show3stars ();
			}
		}
		else {
			//level doesnt exist in build so just lock it
			LockLevel();
		}
		
			
		if(isUnlocked == false)
			LockLevel ();	


		

	}
	
	
	public void show1star()
	{
		firstSprite.spriteName = "bloodStarSplat01";
		
	}
	
	public void show2stars()
	{
		firstSprite.spriteName = "bloodStarSplat01";
		secondSprite.spriteName = "bloodStarSplat02";
	}
	
	public void show3stars()
	{
		firstSprite.spriteName = "bloodStarSplat01";
		secondSprite.spriteName = "bloodStarSplat02";
		thirdSprite.spriteName = "bloodStarSplat03";
	}
	
	public void LockLevel()
	{
		//change appearence 
		unlockedLabel.SetActive(false);
		lockedLabel.SetActive (true);

		//disable collider (so we cant click button)
		(gameObject.GetComponent(typeof(Collider)) as Collider).enabled = false;
		
	}
	
	public void UnlockLevel()
	{
		//change appearence
		lockedLabel.SetActive (false);
		unlockedLabel.SetActive (true);
		
		//enable collider
		(gameObject.GetComponent(typeof(Collider)) as Collider).enabled = true;
	}
}
