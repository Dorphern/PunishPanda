using UnityEngine;
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
		
		// star calculation
		int stars = level.Stars();
		if(stars==1)
		{
			oneStarTexture.enabled = true;	
		}
		else if(stars==1)
		{
			oneStarTexture.enabled = true;	
			twoStarTexture.enabled = true;	
		}
		else if(stars==1)
		{
			oneStarTexture.enabled = true;	
			twoStarTexture.enabled = true;	
			threeStarTexture.enabled = true;
		}*/
		UnLockLevels();
		StartCoroutine(WaitWinScreen());
		
	}
			
	private IEnumerator WaitWinScreen()
	{
		yield return new WaitForSeconds(2.0f);
		winScreen.SetActive(true);	
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
        InstanceFinder.StatsManager.Save();
    }
}
