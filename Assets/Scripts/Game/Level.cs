using System.Collections;
using PunishPanda;
using UnityEngine;
using System.Collections.Generic;
using PunishPanda.Game;


public class Level : MonoBehaviour
{
    private float elapsedTime;
    private bool paused;
    [SerializeField] private float LoseFadeTime = 4.0f;

    /*private int totalPandaCount;
    private int alivePandas;
    private int normalPandaKills;
    private int perfectPandaKills;*/
	private bool onLevelCompleteFlag = false;
	[System.NonSerializedAttribute]
	public List<PandaAI> pandas = new List<PandaAI>();  
	
	public delegate void levelCompleteDelegate();
	public event levelCompleteDelegate onLevelComplete;

    public delegate void LevelLostDelegate();
    public event LevelLostDelegate onLevelLost;

    # region Public Methods

    public void Pause()
    {
        paused = true;
    }

    public void PandaEscaped()
    {
        if (onLevelLost != null)
        {
            StartCoroutine(WaitForLoseFade());
        }
    }

    private IEnumerator WaitForLoseFade()
    {
        yield return new WaitForSeconds(LoseFadeTime);
        onLevelLost();
        Time.timeScale = 0;
    }

    public void Continue()
    { 
        paused = false;
    }

    public float LevelTime
    {
        get
        {
            return elapsedTime;
        }
    }
	
	public int GetScore()
	{
        return ScoreCalculator.Score(InstanceFinder.LevelManager.CurrentLevel.LevelScore, InstanceFinder.ComboSystem.LevelDeaths, elapsedTime);	
	}
	
	public int Stars()
	{
	    return ScoreCalculator.Stars(InstanceFinder.LevelManager.CurrentLevel.LevelScore, GetScore());	
	}
	
    # endregion

    # region Private Methods
    private void OnEnable()
    {
        Time.timeScale = 1.0f;
        //This code only exists to enable that the game will work correctly when working in the editor and loading a random map
        if (!GetComponent<InstanceFinder>().SetupIfMissing())
        {
            InstanceFinder.GameManager.ActiveLevel = this;
        }
        InstanceFinder.GameManager.ActiveLevel = this;
    }

    private void Update()
    {
        if (!paused && InstanceFinder.ComboSystem.AlivePandas > 0)
        {
            elapsedTime += PandaTime.deltaTime;
        }

        if (InstanceFinder.ComboSystem.AlivePandas <= 0 && onLevelCompleteFlag == false && onLevelComplete != null)
		{
			onLevelCompleteFlag = true;
            if(onLevelComplete != null)
			    onLevelComplete();
		}	
    }
	
	// save stat info on level completion
	private void SaveData()
	{
		InstanceFinder.StatsManager.Save();	
	}

    void OnDestroy()
    { Time.timeScale = 1.0f; }
	
    # endregion
}