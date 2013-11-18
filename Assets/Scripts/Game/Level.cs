using PunishPanda;
using UnityEngine;
using System.Collections.Generic;
using PunishPanda.Game;


public class Level : MonoBehaviour
{
    [SerializeField] private LevelScore levelScore = new LevelScore();

    private float elapsedTime;
    private bool paused;
    private int totalPandaCount;
    private int alivePandas;
    private int normalPandaKills;
    private int perfectPandaKills;
	private bool onLevelCompleteFlag = false;
	
	public delegate void levelCompleteDelegate();
	public event levelCompleteDelegate onLevelComplete;

    # region Public Methods

    public void Pause()
    {
        paused = true;
    }

    public void Continue()
    { 
        paused = false;
    }

    public void RegisterPanda()
    {
        totalPandaCount += 1;
        alivePandas += 1;
    }

    public void OnPandaDeath(bool fromTrap, bool perfect)
    {
        alivePandas -= 1;
        if (perfect)
        {
            perfectPandaKills += 1;
        }
        else
        {
            normalPandaKills += 1;
        }
		InstanceFinder.StatsManager.PandasKilled++;
    }


    public float LevelTime
    {
        get
        {
            return elapsedTime;
        }
    }

    # endregion

    # region Private Methods
    private void OnEnable()
    {
        //This code only exists to enable that the game will work correctly when working in the editor and loading a random map
        if (!GetComponent<InstanceFinder>().SetupIfMissing())
        {
            InstanceFinder.GameManager.ActiveLevel = this;
        }
        InstanceFinder.GameManager.ActiveLevel = this;
    }

    private void Update()
    {
        if (!paused && alivePandas > 0)
        {
            elapsedTime += PandaTime.deltaTime;
        }
		
		if(alivePandas <= 0 && onLevelCompleteFlag==false)
		{
			onLevelCompleteFlag = true;
			onLevelComplete();
		}	
    }
	
	// save stat info on level completion
	private void SaveData()
	{
		InstanceFinder.StatsManager.Save();	
	}

    # endregion

//    private void OnGUI()
//    {
//        return;
//        var levelManger = InstanceFinder.LevelManager;
//
//        if (alivePandas > 0)
//        {
//            if (levelManger != null)
//            {
//                Rect nextRect = new Rect(Screen.width - 150, 50, 100, 80);
//                string nextLevelString = LanguageManager.Instance.GetTextValue("Game.NextLevel");
//                if (GUI.Button(nextRect, nextLevelString))
//                {
//                    levelManger.LoadNextLevel();
//                }
//
//                nextRect = new Rect(Screen.width - 300, 50, 100, 80);
//                string mainMenuString = LanguageManager.Instance.GetTextValue("Game.MainMenu");
//                if (GUI.Button(nextRect, mainMenuString))
//                {
//                    levelManger.LoadMainMenu();
//                }
//
//            }
//
//            Rect killButton = new Rect(50, 50, 100, 80);
//            if (GUI.Button(killButton, "(Fake) Kill Panda"))
//            {
//                OnPandaDeath(false, false);
//            }
//        }
//        else
//        {
//            int kills = normalPandaKills + perfectPandaKills;
//            int heightOffset = 30; 
//            Rect nextRect = new Rect(Screen.width/2 - 75, 50, 100, 40);
//            int score = ScoreCalculator.Score(levelScore, perfectPandaKills, normalPandaKills, elapsedTime);
//            string scoreString = LanguageManager.Instance.GetTextValue("Score.Score");
//            GUI.Label(nextRect, scoreString + score);
//            nextRect.y += heightOffset;
//            string pandaKillsString = LanguageManager.Instance.GetTextValue("Score.PandaKills");
//            GUI.Label(nextRect, pandaKillsString + kills);
//            nextRect.y += heightOffset;
//
//            string timeScoreString = LanguageManager.Instance.GetTextValue("Score.TimeScore");
//            GUI.Label(nextRect, timeScoreString + ScoreCalculator.TimeScore(levelScore, elapsedTime));
//            nextRect.y += heightOffset;
//
//            string starsString = LanguageManager.Instance.GetTextValue("Score.Stars");
//            GUI.Label(nextRect, starsString + ScoreCalculator.Stars(levelScore, score));
//            if (alivePandas == 0)
//            {
//                nextRect.y += heightOffset;
//                nextRect.width += 20;
//                string perfectPandaKill = LanguageManager.Instance.GetTextValue("Score.PerfectKill");
//                GUI.Label(nextRect, perfectPandaKill);
//                nextRect.width -= 20;
//            }
//            nextRect.y += heightOffset*2;
//
//            string nextLevelString = LanguageManager.Instance.GetTextValue("Game.NextLevel");
//            if (GUI.Button(nextRect, nextLevelString))
//            {
//                levelManger.LoadNextLevel();
//            }
//
//            string replayString = LanguageManager.Instance.GetTextValue("Game.Replay");
//            nextRect.y += heightOffset * 2;
//            if (GUI.Button(nextRect, replayString))
//            {
//                levelManger.Reload();
//            }
//            
//            string mainMenuString = LanguageManager.Instance.GetTextValue("Game.MainMenu");
//            nextRect.y += heightOffset * 2;
//            if (GUI.Button(nextRect, mainMenuString))
//            {
//                levelManger.LoadMainMenu();
//            }
//        }
//    }
//
}