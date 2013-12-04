using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadAchievementsAndStats : MonoBehaviour {
	
	public GameObject achievementListGridRoot;
	public GameObject achievementsElement;
	public GameObject statisticsListGridRoot;
	public GameObject statisticsElement;
	public Texture2D  achievementCompletedTexture;
	
	int achievementsIterator = 0;
	int statsIterator = 0;
	
	void Start ()
	{
		StartCoroutine("InitLoad");
	}
	
	IEnumerator InitLoad()
	{
		
		yield return new WaitForSeconds(.1f);
		init ();
	}

    //private System.IO.StreamWriter file;
	// Use this for initialization
	void init() {
		
		
		if(achievementListGridRoot!=null && achievementsElement!=null)
		{
			List<Achievement> achs = InstanceFinder.AchievementManager.AchievementsToList();
		    for (int i = 0; i < achs.Count; i++)
		    {
                AddAchievementMember(achievementListGridRoot, achs[i]);
		    }
			achievementListGridRoot.GetComponent<UIGrid>().Reposition();
		}
		
		if(statisticsListGridRoot!=null && statisticsElement!=null)
		{
			StatsManager sm = InstanceFinder.StatsManager;
			TrapInfo[] ti = sm.GetTrapInfo();

		    //using (file = new System.IO.StreamWriter(@"C:\stasts.txt"))
		    {

		        //AddStatisticMember(statisticsListGridRoot, "", "", null);
		        AddStatisticMember(statisticsListGridRoot, "Total amount of slaps", sm.PandaSlaps.ToString(), null);
		        AddStatisticMember(statisticsListGridRoot, "Lifetime score collected", sm.TotalScore.ToString(), null);
		        AddStatisticMember(statisticsListGridRoot, "Games played", sm.GamesPlayed.ToString(), null);
		        AddStatisticMember(statisticsListGridRoot, "Pandas killed", sm.PandasKilled.ToString(), null);
		        AddStatisticMember(statisticsListGridRoot, "Perfect kills", sm.PandasKilledPerfect.ToString(), null);
		        AddStatisticMember(statisticsListGridRoot, "Favourite trap", FindPrefferedTrap(ti).name, null);

		        for (int i = 0; i < ti.Length; i++)
		        {
		            AddStatisticMember(statisticsListGridRoot, "Killed on " + ti[i].name, ti[i].kills.ToString(), null);
		        }
				
				string liters = Localization.instance.Get("liters");
		        AddStatisticMember(statisticsListGridRoot, "Blood collected", sm.LiterBlood.ToString() + " " + liters, null);
		        AddStatisticMember(statisticsListGridRoot, "Total combo kills", sm.PandasComboKilled.ToString(), null);

		        statisticsListGridRoot.GetComponent<UIGrid>().Reposition();
		    }
		    //file = null;
		}
		
	}
	
	TrapInfo FindPrefferedTrap(TrapInfo[] ti)
	{
		TrapInfo max = ti[0];
		
		for(int i=1; i<ti.Length; i++)
		{
			if(max.kills < ti[i].kills)
				max = ti[i];
		}
		
		return max;
	}
	
	void AddAchievementMember(GameObject root, Achievement ach)
	{
		
		GameObject go = NGUITools.AddChild(root, achievementsElement);
		go.name = "Achievement" + achievementsIterator;
		achievementsIterator++;
	    go.transform.FindChild("TitleLabel").GetComponent<UILabel>().text = ach.name;
	    go.transform.FindChild("DescriptionLabel").GetComponent<UILabel>().text = ach.description;
	    if(achievementCompletedTexture != null && ach.HasBeenCompleted())
				go.transform.FindChild("ElementTexture").GetComponent<UITexture>().mainTexture = achievementCompletedTexture;
//		if(ach.achievementIcon!=null)
//			go.transform.FindChild("ElementTexture").GetComponent<UITexture>().mainTexture = ach.achievementIcon;
	}
	
	void AddStatisticMember(GameObject root, string name, string description, Texture image)
	{
		//file.WriteLine(name + "="+name);
		GameObject go = NGUITools.AddChild(root, statisticsElement);
		go.name = "Statistic" + statsIterator;
		statsIterator++;
        go.transform.FindChild("TitleLabel").GetComponent<UILabel>().text = name;
		go.transform.FindChild("DescriptionLabel").GetComponent<UILabel>().text =  description;
		if(image!=null)
			go.transform.FindChild("ElementTexture").GetComponent<UITexture>().mainTexture = image;
	}
}
