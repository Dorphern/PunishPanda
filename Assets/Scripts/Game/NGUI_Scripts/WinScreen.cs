using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms.Impl;
using PunishPanda;

public class WinScreen : MonoBehaviour {
	
	public GameObject winScreen;
	public UITexture funFactsTexture;
	public UISprite oneStarTexture;
	public UISprite twoStarTexture;
	public UISprite threeStarTexture;
	public UILabel  FunFactsLabel;
	public UILabel  scoreLabel;
	public UILabel  ScoreTypeLabel;
    public UILabel  newHighScoreLabel;
	public UILabel  TotalScoreLabel;
	
	public AnimationCurve ScoreCurve;
	public float scoreCurveDuration;
	

	
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
        Level level = InstanceFinder.GameManager.ActiveLevel;
        int score = level.GetScore();
        int highscore = levelData.HighScore;
		
		if(InstanceFinder.StatsManager!=null)
			InstanceFinder.StatsManager.TotalScore += score;
		
        if (score > highscore)
        {
            newHighScoreLabel.enabled = true;
            levelData.HighScore = score;
        }

        //scoreLabel.text = score.ToString();
        
        /*// we also need to deactivate controls
        //this is where calcualtions for score, stars and adding to lifetime score happens
		
        Level level = InstanceFinder.GameManager.ActiveLevel;
		
        //Set win screen
		
        funFactsTexture.mainTexture = level.FunFactsTexture;
        FunFactsLabel.text = level.FunFactsText;
		
        // score calculation
        int score = level.GetScore();
        int highScore = InstanceFinder.LevelManager.CurrentLevel.HighScore;
        scoreLabel.text = score.ToString();
		
        if(score > highScore)
        {
            highScoreLabel.text = "New High Score!";
        }
        else
        {
            highScoreLabel.text = "HighScore: " + highScore;
        }
		*/
        // star calculation

        int stars = level.Stars();
//        if(stars==1)
//        {
//            oneStarTexture.gameObject.SetActive(true);
//        }
//        else if(stars==2)
//        {
//            oneStarTexture.gameObject.SetActive(true);
//            twoStarTexture.gameObject.SetActive(true);
//        }
		
		StartCoroutine(PlayAnimations(stars));
		StartCoroutine(PlayScore());
		
        if(stars==3)
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
	
	IEnumerator PlayAnimations(int stars)
	{
		if(oneStarTexture!=null && stars>=1)
			oneStarTexture.gameObject.SetActive(true);
		
		yield return new WaitForSeconds(0.4f);
		
		if(twoStarTexture!=null && stars>=2)
			twoStarTexture.gameObject.SetActive(true);
		
		yield return new WaitForSeconds(0.4f);
		
		if(threeStarTexture!=null && stars==3)
			threeStarTexture.gameObject.SetActive(true);
		
	}
	
	IEnumerator PlayScore()
	{
		List<ComboKill> kills = InstanceFinder.ComboSystem.LevelDeaths.ComboKills;
		PointSystem pointSystem = InstanceFinder.PointSystem;
		int timeScore = InstanceFinder.GameManager.ActiveLevel.GetTimeScore();
		int total = 0, intermediateTotal = 0;
		int normalKills = 0, perfectKills = 0;
		float startTime, timeElapsed;
		
	
		// for every panda present the kill
		for(int i=0; i<kills.Count; i++)
		{
			
			ComboKill ck = kills[i];
		
			normalKills += ck.NormalKills;
			perfectKills += ck.PerfectKills;			
		}
		// normal kills
		ScoreTypeLabel.text = "Normal Kill";
		intermediateTotal = pointSystem.PerKill;
		for(int i=0; i< normalKills; i++)
		{
			//score increment loop
		
			startTime = Time.realtimeSinceStartup;
			timeElapsed = Time.realtimeSinceStartup - startTime;
			
			while(timeElapsed < scoreCurveDuration)
			{
				scoreLabel.text = (( ScoreCurve.Evaluate(timeElapsed/scoreCurveDuration)*intermediateTotal)).ToString("N0");
				TotalScoreLabel.text = (total + ( ScoreCurve.Evaluate(timeElapsed/scoreCurveDuration)*intermediateTotal)).ToString("N0");
				yield return null;
				timeElapsed = Time.realtimeSinceStartup - startTime;
			}
			total += intermediateTotal;
			yield return new WaitForSeconds(1f);
			
		}
		
		// perfect kills
		ScoreTypeLabel.text = "Perfect Kill";
		intermediateTotal = pointSystem.PerfectKill;
		for(int i=0; i< perfectKills; i++)
		{
			//score increment loop
		
			startTime = Time.realtimeSinceStartup;
			timeElapsed = Time.realtimeSinceStartup - startTime;
			
			while(timeElapsed < scoreCurveDuration)
			{
				scoreLabel.text = (( ScoreCurve.Evaluate(timeElapsed/scoreCurveDuration)*intermediateTotal)).ToString("N0");
				TotalScoreLabel.text = (total + ( ScoreCurve.Evaluate(timeElapsed/scoreCurveDuration)*intermediateTotal)).ToString("N0");
				yield return null;
				timeElapsed = Time.realtimeSinceStartup - startTime;
			}
			total += intermediateTotal;
			yield return new WaitForSeconds(1f);
			
		}
		
		// combo kills
		for(int i=0; i<kills.Count; i++)
		{	
			ComboKill ck = kills[i];
			
			if(ck.ComboCount > 1)
			{
				ScoreTypeLabel.text = ck.ComboCount + "x Combo";
				intermediateTotal = pointSystem.Combo * ck.ComboCount;
				
				
				startTime = Time.realtimeSinceStartup;
				timeElapsed = Time.realtimeSinceStartup - startTime;
				
				while(timeElapsed < scoreCurveDuration)
				{
					scoreLabel.text = (( ScoreCurve.Evaluate(timeElapsed/scoreCurveDuration)*intermediateTotal)).ToString("N0");
					TotalScoreLabel.text = (total + ( ScoreCurve.Evaluate(timeElapsed/scoreCurveDuration)*intermediateTotal)).ToString("N0");
					yield return null;
					timeElapsed = Time.realtimeSinceStartup - startTime;
				}
				total += intermediateTotal;
				yield return new WaitForSeconds(1f);
			}
		}
		
		// time score calc
		ScoreTypeLabel.text = "Time";
		
		
		intermediateTotal = timeScore;
		
		startTime = Time.realtimeSinceStartup;
		timeElapsed = Time.realtimeSinceStartup - startTime;
		
		
		if(timeScore!=0)
		{
			while(timeElapsed < scoreCurveDuration)
			{
				scoreLabel.text = (( ScoreCurve.Evaluate(timeElapsed/scoreCurveDuration)*timeScore)).ToString("N0");
				TotalScoreLabel.text = (total + ( ScoreCurve.Evaluate(timeElapsed/scoreCurveDuration)*intermediateTotal)).ToString("N0");
				yield return null;
				timeElapsed = Time.realtimeSinceStartup - startTime;
			}
		}
		else
			scoreLabel.text = (0).ToString();
		
		total += intermediateTotal;
		
		yield return new WaitForSeconds(1f);
			
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
