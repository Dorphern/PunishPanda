using UnityEngine;
using System;
using System.Collections;
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
	
	
	private bool completed = false;
	// expressing progress as a float
	private float currentProgress = 0f;
	private DateTime goalCompletionTime;
	
	public void LoadAchievement(float progress)
	{
		if(progress>=goal)
		{
			completed = true;
		}
		currentProgress = progress;
	}
	
	public bool AddProgress(float progress)
	{
		if(completed)
			return false;
		
		currentProgress += progress;
		
		if(currentProgress>=goal)
		{
			completed = true;
			goalCompletionTime = DateTime.Now;
			return true;
		}
		return false;
	}
	
	public bool SetProgress(float progress)
	{
		if(completed)
			return false;
		
		currentProgress = progress;
		if(progress>=goal)
		{
			completed = true;
			goalCompletionTime = DateTime.Now;
			return true;
		}
		return false;
	}
	
	public bool HasBeenCompleted()
	{
		return completed;	
	}
	
	public float GetProgress()
	{
		return currentProgress;
	}
	
	public float GetPersentageProgress()
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
	public bool debug = false;
	
	public delegate void AchievementGoalHandler(Achievement achievement);
	public event AchievementGoalHandler onAchievementCompleted;
	
	private Dictionary<string,Achievement> achievements;
	
	public bool AddProgressToAchievement(string name, float progress)
	{
		if(!achievements.ContainsKey(name))
		{
			if(debug)
				Debug.Log("Attempted to add progress to achievement that doesn't exist: " + name);
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
			if(debug)
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
		LoadAchievements();	
	}
	
	private void LoadAchievements()
	{
		achievements = new Dictionary<string, Achievement>();
	    var localization = Localization.instance;
		for(int i=0;i<achievementList.Count;i++)
		{
            


			if(achievements.ContainsKey(achievementList[i].name))
			{
				if(debug)
					Debug.Log("Duplicate achievements with the name " + achievementList[i].name);
				continue;
			}
			
			float progress = PlayerPrefs.GetFloat(achievementList[i].name, 0f);
			if(progress!= -1)
				achievementList[i].LoadAchievement(progress);
			
            
			achievements.Add(achievementList[i].name, achievementList[i]);
		}
	}
	
	public void SaveAchievements()
	{
		List<string> keys = new List<string>(achievements.Keys);
		Achievement ach;

	    //using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Achiev.txt"))
	    {
	        for (int i = 0; i < keys.Count; i++)
	        {
	            ach = achievements[keys[i]];

                /*string achievVal = ach.name + "=" + ach.name;
                string achievDes = ach.description + "=" + ach.description;
                file.WriteLine(achievVal);
                file.WriteLine(achievDes);*/

	            float progress = ach.GetProgress();
	            PlayerPrefs.SetFloat(ach.name, progress);
	        }
	    }
	}
	
	public List<Achievement> AchievementsToList()
	{
		return new List<Achievement>(achievements.Values);
	}
}




