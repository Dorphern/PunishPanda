using UnityEngine;
using System;
using System.Collections.Generic;


[Serializable]
public class Achievement
{
	public string name;
	public string description;
	public float goal;
	
	
	// add stuff about achievement like the image that will be shown in the achievement 
	// window
	
	public Texture2D achievementIcon;
	
	
	private bool achieved = false;
	// expressing progress as a float
	private float currentProgress = 0f;
	private DateTime goalCompletionTime;
	
	
	public bool AddProgress(float progress)
	{
		if(achieved)
			return false;
		
		progress += currentProgress;
		
		if(currentProgress>=goal)
		{
			achieved = true;
			goalCompletionTime = DateTime.Now;
			return true;
		}
		return false;
	}
	
	public bool SetProgress(float progress)
	{
		if(achieved)
			return false;
		
		currentProgress = progress;
		if(progress>=goal)
		{
			achieved = true;
			goalCompletionTime = DateTime.Now;
			return true;
		}
		return false;
	}
	
	public bool HasBeenCompleted()
	{
		return achieved;	
	}
	
	public float GetPersentOfProgress()
	{
		if(currentProgress<goal)
			return currentProgress/goal;
		
		return 1f;
	}
	
	public string GetAchievementCompletionDate()
	{
		DateTime date = goalCompletionTime.Date;
		return date.ToString("d");
	}
	
	public string GetAchievementCompletionDigitalTime()
	{
		return goalCompletionTime.ToString("HH:mm:ss");
	}
	
	public string GetAchievementCompletionAnalogTime()
	{
		return goalCompletionTime.ToString("t");
	}
	
	public DateTime GetAchievementCompletionDateTime()
	{
		return goalCompletionTime;
	}
}

public class AchievementManager : MonoBehaviour {

	public List<Achievement> achievementList = new List<Achievement>();
	
	public delegate void AchievementGoalHandler(Achievement achievement);
	public event AchievementGoalHandler onAchievementCompleted;
	
	private Dictionary<string,Achievement> achievements;
	
	public bool AddProgressToAchievement(string name, float progress)
	{
		if(!achievements.ContainsKey(name))
		{
			Debug.LogError("Attempted to add progress to achievement that doesn't exist: " + name);
		}
		else
		{
			Achievement ach;
			achievements.TryGetValue(name, out ach);
			
			if(ach.AddProgress(progress))
			{
				onAchievementCompleted(ach);
				return true;		
			}
		}
		return false;
	}
	
	public bool SetProgressToAchievement(string name, float progress)
	{
		if(!achievements.ContainsKey(name))
		{
			Debug.LogError("Attempted to set progress to achievement that doesn't exist: " + name);
		}
		else
		{
			Achievement ach;
			achievements.TryGetValue(name, out ach);
			
			if(ach.SetProgress(progress))
			{
				onAchievementCompleted(ach);
				return true;		
			}
		}
		return false;
	}
	
	void Start() 
	{
		//LoadAchievements();	
	}
	
	private void LoadAchievements()
	{
		for(int i=0;i<achievementList.Count;i++)
		{
			if(achievements.ContainsKey(achievementList[i].name))
			{
				Debug.Log("Duplicate achievement with the name " + achievementList[i].name);
				continue;
			}
			
			achievements.Add(achievementList[i].name, achievementList[i]);
		}
	}
	

}




