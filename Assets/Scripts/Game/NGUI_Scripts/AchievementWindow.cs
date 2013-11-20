using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AchievementWindow : MonoBehaviour {
	
	public UILabel achievementTitleLabel;
	public UILabel achievementTextLabel;
	public UITexture achievementIcon;
	
	public UIRunTween rt;
	
	bool forward = true;
	Queue<Achievement> completedAchievements;
	bool isRunning = false;
	
	
	void Start()
	{
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
			if(achievementTitleLabel!=null)
				achievementTitleLabel.text = ach.name;
			if(achievementTextLabel!=null)
				achievementTextLabel.text = ach.description;
			if(achievementIcon!=null)
				achievementIcon.mainTexture = ach.achievementIcon;
			// add a bit of time between setting up the achievement window and dropping it
			yield return new WaitForSeconds(0.01f);
			rt.RunTween();
			yield return new WaitForSeconds(3f);
			rt.RunTween();
			yield return new WaitForSeconds(1.5f);
		}
		isRunning = false;
		
	}
	
}
