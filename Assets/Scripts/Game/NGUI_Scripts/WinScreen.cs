﻿using UnityEngine;
using System.Collections;

public class WinScreen : MonoBehaviour {
	
	public GameObject winScreen;
	public UITexture funFactsTexture;
	public UISprite oneStarTexture;
	public UISprite twoStarTexture;
	public UISprite threeStarTexture;
	public UILabel   FunFactsLabel;
	public UILabel   scoreLabel;
	public UILabel   highScoreLabel;
    public UILabel  newHighScoreLabel;
	
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

        scoreLabel.text = score.ToString();
        
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
        if(stars==1)
        {
            oneStarTexture.gameObject.SetActive(true);
        }
        else if(stars==2)
        {
            oneStarTexture.gameObject.SetActive(true);
            twoStarTexture.gameObject.SetActive(true);
        }
        else if(stars==3)
        {
            oneStarTexture.gameObject.SetActive(true);
            twoStarTexture.gameObject.SetActive(true);
            threeStarTexture.gameObject.SetActive(true);
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
