using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AchievementWindow : MonoBehaviour {
	
	public UILabel achTitleLabel;
	public UILabel achTextLabel;
	public UITexture achievementIcon;
	
	UIRunTween rt;
	bool forward = true;
	List<Achievement> completedAchievements;
	bool isRunning = false;
	
	
	void Start()
	{
	InstanceFinder.AchievementManager.onAchievementCompleted += OnAchievementComplete;
	}
	
	void OnAchievementComplete(Achievement achievement)
	{
		completedAchievements.Add(achievement);
		
		if(!isRunning)
		{
			isRunning = true;
			StartCoroutine(runAchievementWindow());
		}
	}
	
	IEnumerator runAchievementWindow()
	{
		rt.RunTween();
		yield return new WaitForSeconds(3f);
		rt.RunTween();
		yield return new WaitForSeconds(3f);
		isRunning = false;
	}
	
}
