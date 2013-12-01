using UnityEngine;
using System.Collections;

public class LevelIconManager : MonoBehaviour {
	
	//Handler for the LEVELS SCREEN that checks how many
	//stars should be shown on a Level Icon.
	
	public string LevelNumber;
	public bool isLocked;
	
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
		
		Number = unlockedLabel.GetComponent<UILabel>();
		Number.text = LevelNumber;
		Number = lockedLabel.GetComponent<UILabel>();
		Number.text = LevelNumber;
		
		
		
		
		//TO DO:
		//CHECK IF LOCKED OR NOT
		int LevelIndex;
		//convert string to int.
		//LevelIndex = int.TryParse(LevelNumber,LevelIndex);
		
		//isLocked = InstanceFinder.LevelManager.CurrentWorld.Levels[0].UnlockedLevel;
		
		
		if(isLocked == true)
			LockLevel ();
		
		
		
		

	}
	
	
	public void show1star()
	{
		firstSprite.spriteName = "Star_black";
		
	}
	
	public void show2stars()
	{
		firstSprite.spriteName = "Star_black";
		secondSprite.spriteName = "Star_black";
	}
	
	public void show3stars()
	{
		firstSprite.spriteName = "Star_black";
		secondSprite.spriteName = "Star_black";
		thirdSprite.spriteName = "Star_black";
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
