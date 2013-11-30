using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;
using PunishPanda;

public class WinScreen : MonoBehaviour {
	
	public GameObject winScreen;
	public GameObject NewHighScoreTexture;
	public UITexture funFactsTexture;
	public UISprite oneStarTexture;
	public UISprite twoStarTexture;
	public UISprite threeStarTexture;
	public UILabel  FunFactsLabel;
	public UILabel  scoreLabel;
	public UILabel  ScoreTypeLabel;
    public UILabel  newHighScoreLabel;
	public UILabel  TotalScoreLabel;
	public UIRunTween ZoomInTween;
	public UIRunTween ZoomOutTween;
	public UIRunTween ZoomOutScoreTween;
	
	public AnimationCurve ScoreCurve;
	public float scoreCurveDuration;
	
	int oneStarScore, twoStarScore, threeStarScore;
	
	int total = 0, intermediateTotal = 0;

	
	public void OnLevelsButtonClicked()
	{ 
		InstanceFinder.LevelManager.LoadLevelsMenu();
	}
	
	public void OnRestartButtonClicked()
	{
		InstanceFinder.LevelManager.Reload();
	}
	
	public void OnNextLevelButtonClicked()
	{
		InstanceFinder.LevelManager.LoadNextLevel();
	}
	
	void Start()
	{
		InstanceFinder.GameManager.ActiveLevel.onLevelComplete += OnLevelComplete;
	}
	
	private void OnLevelComplete()
	{
		UnLockLevels();    
		StartCoroutine(WaitWinScreen());
	}
			
	private IEnumerator WaitWinScreen()
	{
		yield return new WaitForSeconds(2.0f);
        SetWinScreenData();
		winScreen.SetActive(true);

        InstanceFinder.StatsManager.Save();
	}
     
    private void SetWinScreenData()
    {
        

        LevelData levelData = InstanceFinder.LevelManager.CurrentLevel;
        funFactsTexture.mainTexture = levelData.FunFactsTexture;
        if (Localization.instance.currentLanguage == "English")
            FunFactsLabel.text = levelData.FunFactsText;
        else
            FunFactsLabel.text = levelData.DanishFunFactsText;
        Level level = InstanceFinder.GameManager.ActiveLevel;
        int score = level.GetScore();
        int highscore = levelData.HighScore;
		
		oneStarScore = levelData.OneStar;
		twoStarScore = levelData.TwoStars;
		threeStarScore = levelData.ThreeStars;
		
		if(InstanceFinder.StatsManager!=null)
			InstanceFinder.StatsManager.TotalScore += score;
		
//        if (score > highscore)
//        {
//            newHighScoreLabel.enabled = true;
//            levelData.HighScore = score;
//        }

		
		StartCoroutine(PlayWinAnimations(score, highscore));
		
        if(level.Stars()==3)
        {
			if(InstanceFinder.AchievementManager != null)
			{
				// if the achievement has been completed
				if(InstanceFinder.AchievementManager.SetProgressToAchievement(levelData.LevelName,3))
				{
					// that means that the stars for this level had not been collected until now
					// thus we add progress to the star hunter achievement
					InstanceFinder.AchievementManager.AddProgressToAchievement("Star hunter", 1);
				}
			}
        }

    }
	
//	void PlayStarAnimations(int score)
//	{
//		if(oneStarTexture!=null && !oneStarTexture.gameObject.active && score>=oneStarScore)
//			oneStarTexture.gameObject.SetActive(true);
//		if(twoStarTexture!=null && !twoStarTexture.gameObject.active && score>=twoStarScore)
//			twoStarTexture.gameObject.SetActive(true);		
//		if(threeStarTexture!=null && !threeStarTexture.gameObject.active && score>=threeStarScore)
//			threeStarTexture.gameObject.SetActive(true);
//		
//	}

	
	IEnumerator PlayWinAnimations(int score, int highscore)
	{
		List<ComboKill> kills = InstanceFinder.ComboSystem.LevelDeaths.ComboKills;
		PointSystem pointSystem = InstanceFinder.PointSystem;
		int timeScore = InstanceFinder.GameManager.ActiveLevel.GetTimeScore();
		float startTime, timeElapsed;
		int normalKills = 0, perfectKills = 0;
		total = 0;
		intermediateTotal = 0;
		
	
		// for every panda present the kill
		for(int i=0; i<kills.Count; i++)
		{
			
			ComboKill ck = kills[i];
		
			normalKills += ck.NormalKills;
			perfectKills += ck.PerfectKills;			
		}
		// normal kills


        ScoreTypeLabel.text = Localization.instance.Get("NormalKill");
		intermediateTotal = pointSystem.PerKill;
		
		
		yield return new WaitForSeconds(1f);
		for(int i=0; i< normalKills; i++)
		{
			
			//score increment loop
			ZoomInTween.RunTween();
			yield return new WaitForSeconds(0.2f);
			startTime = Time.realtimeSinceStartup;
			timeElapsed = Time.realtimeSinceStartup - startTime;
			
			while(timeElapsed < scoreCurveDuration)
			{
				scoreLabel.text = (( ScoreCurve.Evaluate(timeElapsed/scoreCurveDuration)*intermediateTotal)).ToString("N0");
				int val =   Convert.ToInt32 (  total + ( ScoreCurve.Evaluate(timeElapsed/scoreCurveDuration)*intermediateTotal));
				TotalScoreLabel.text = (val).ToString("N0");
				//PlayStarAnimations(val);
				yield return null;
				timeElapsed = Time.realtimeSinceStartup - startTime;
			}
			
			scoreLabel.text = ((intermediateTotal)).ToString("N0");
			TotalScoreLabel.text = (total + ( intermediateTotal)).ToString("N0");
			total += intermediateTotal;
			
			ZoomOutTween.RunTween();
			yield return new WaitForSeconds(0.5f);
			
		}
		
		// perfect kills
		ScoreTypeLabel.text = Localization.instance.Get("PerfectKill");
		intermediateTotal = pointSystem.PerfectKill;
		for(int i=0; i< perfectKills; i++)
		{
			//score increment loop
			ZoomInTween.RunTween();
			yield return new WaitForSeconds(0.2f);		
			startTime = Time.realtimeSinceStartup;
			timeElapsed = Time.realtimeSinceStartup - startTime;
			
			while(timeElapsed < scoreCurveDuration)
			{
				scoreLabel.text = (( ScoreCurve.Evaluate(timeElapsed/scoreCurveDuration)*intermediateTotal)).ToString("N0");
				int val =  Convert.ToInt32(  total + ( ScoreCurve.Evaluate(timeElapsed/scoreCurveDuration)*intermediateTotal));
				TotalScoreLabel.text = (val).ToString("N0");
				//PlayStarAnimations(val);
				yield return null;
				timeElapsed = Time.realtimeSinceStartup - startTime;
			}
			scoreLabel.text = ((intermediateTotal)).ToString("N0");
			TotalScoreLabel.text = (total + ( intermediateTotal)).ToString("N0");
			total += intermediateTotal;
			ZoomOutTween.RunTween();
			yield return new WaitForSeconds(0.5f);
			
		}
		
		// combo kills
		for(int i=0; i<kills.Count; i++)
		{	
			ComboKill ck = kills[i];
			
			if(ck.ComboCount > 1)
			{
				ScoreTypeLabel.text = ck.ComboCount + Localization.Localize("Combo");
				intermediateTotal = pointSystem.Combo * ck.ComboCount;
				
				ZoomInTween.RunTween();
				yield return new WaitForSeconds(0.2f);				
				startTime = Time.realtimeSinceStartup;
				timeElapsed = Time.realtimeSinceStartup - startTime;
				
				while(timeElapsed <= scoreCurveDuration)
				{
					scoreLabel.text = (( Mathf.Clamp(ScoreCurve.Evaluate(timeElapsed/scoreCurveDuration),0f,1f)*intermediateTotal)).ToString("N0");
					int val =  Convert.ToInt32(  total + ( ScoreCurve.Evaluate(timeElapsed/scoreCurveDuration)*intermediateTotal));
					TotalScoreLabel.text = (val).ToString("N0");
					//PlayStarAnimations(val);
					yield return null;
					timeElapsed = Time.realtimeSinceStartup - startTime;
				}
				scoreLabel.text = ((intermediateTotal)).ToString("N0");
				TotalScoreLabel.text = (total + ( intermediateTotal)).ToString("N0");
				total += intermediateTotal;
				ZoomOutTween.RunTween();
				yield return new WaitForSeconds(0.5f);
			}
		}
		
		// time score calc
		ScoreTypeLabel.text = Localization.instance.Get("Time");
		
		
		intermediateTotal = timeScore;
		
		startTime = Time.realtimeSinceStartup;
		timeElapsed = Time.realtimeSinceStartup - startTime;
		
		ZoomInTween.RunTween();
		yield return new WaitForSeconds(0.2f);
		if(timeScore!=0)
		{
			
			while(timeElapsed < scoreCurveDuration)
			{
				scoreLabel.text = (( ScoreCurve.Evaluate(timeElapsed/scoreCurveDuration)*intermediateTotal)).ToString("N0");
				int val = Convert.ToInt32(  total + ( ScoreCurve.Evaluate(timeElapsed/scoreCurveDuration)*intermediateTotal));
				TotalScoreLabel.text = (val).ToString();
				//PlayStarAnimations(val);
				yield return null;
				timeElapsed = Time.realtimeSinceStartup - startTime;
			}
			scoreLabel.text = ((intermediateTotal)).ToString("N0");
			TotalScoreLabel.text = (total + ( intermediateTotal)).ToString("N0");
		}
		else
		{
			scoreLabel.text = (0).ToString();
		}
		
		yield return new WaitForSeconds(0.5f);
		ZoomOutTween.RunTween();
        //TODO FIX THIS
		//ZoomOutScoreTween.RunTween();
		yield return new WaitForSeconds(0.5f);
		
		total += intermediateTotal;
		
		
		
		
		if(oneStarTexture!=null && twoStarTexture!=null && threeStarTexture!=null)
		{
			if(score>=oneStarScore)
			{
				oneStarTexture.gameObject.SetActive(true);
				
			}
			
			if(score>=twoStarScore)
			{
				yield return new WaitForSeconds(0.8f);
				twoStarTexture.gameObject.SetActive(true);
				
			}
			
			if(score>=threeStarScore)
			{
				yield return new WaitForSeconds(1f);
				threeStarTexture.gameObject.SetActive(true);
				
			}
			
		}
		
		// if new highscore reached
		if(highscore < score)
		{
			NewHighScoreTexture.SetActive(true);
		}
		else
		{
			//newHighScoreLabel.text = "Highscore: " + highscore;
			newHighScoreLabel.gameObject.SetActive(true);
		}
			
	}

    private static void UnLockLevels()
    {
        var levelManager = InstanceFinder.LevelManager;
        levelManager.CurrentLevel.UnlockedFunFact = true;
        var levels = levelManager.CurrentWorld.Levels;

        if (levels.Count - 1 > levelManager.CurrentLevelIndex)
        {
            levels[levelManager.CurrentLevelIndex + 1].UnlockedLevel = true;
        }
        

    }
}
