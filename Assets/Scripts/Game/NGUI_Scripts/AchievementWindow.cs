using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AchievementWindow : MonoBehaviour {
	
	public UILabel achievementTitleLabel;
	public UILabel achievementTextLabel;
	public UITexture achievementIcon;
	public GameObject GUIcamera;
	public UIRunTween rt;
	
	bool forward = true;
	Queue<Achievement> completedAchievements;
	bool isRunning = false;
	Localization localization;
	
	void Start()
	{
		localization = Localization.instance;
		completedAchievements = new Queue<Achievement>();
		InstanceFinder.AchievementManager.onAchievementCompleted += OnAchievementComplete;
	}
	
	void OnAchievementComplete(Achievement achievement)
	{
		
		completedAchievements.Enqueue(achievement);
		
		if(!isRunning) 
		{
			isRunning = true;

            StartCoroutine(runAchievementWindow());
		}
	}
	
	IEnumerator runAchievementWindow()
	{
		while(completedAchievements.Count>0)
		{
			Achievement ach = completedAchievements.Dequeue();
			if(ach!=null){
				GUIcamera.SetActive(true);
				if(achievementTitleLabel!=null)
				{
					achievementTitleLabel.text = localization.Get(ach.name);
				}
				if(achievementTextLabel!=null)
					achievementTextLabel.text = localization.Get(ach.description);
				// disabling this since it causes null pointer exceptions
//				if(achievementIcon!=null && ach.achievementIcon!=null)
//					achievementIcon.mainTexture = ach.achievementIcon;
				// add a bit of time between setting up the achievement window and dropping it
				
				
				yield return new WaitForSeconds(0.01f);
				yield return new WaitForEndOfFrame();
				rt.RunTween();
				yield return new WaitForSeconds(3f);
				yield return new WaitForEndOfFrame();
				rt.RunTween();
				yield return new WaitForSeconds(1.5f);
				yield return new WaitForEndOfFrame();
				GUIcamera.SetActive(false);
			}
		}
		isRunning = false;
		
	}
	
}
