using System.Collections;
using PunishPanda;
using UnityEngine;
using System.Collections.Generic;
using PunishPanda.Game;


public class Level : MonoBehaviour
{
    private float elapsedTime;
    private bool paused;

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

    [SerializeField]
    [EventHookAttribute("On Level Win")]
    private List<AudioEvent> onLevelWin = new List<AudioEvent>();

    [SerializeField]
    [EventHookAttribute("On Level Lost")]
    private List<AudioEvent> onLose = new List<AudioEvent>();

    # region Public Methods

    public void Pause()
    {
        paused = true;
    }
	
	public void AddPandaAIRef(PandaAI panda)
	{
		pandas.Add(panda);
	}
	
	public void RemovePandaAIRef(PandaAI panda)
	{
		pandas.Remove(panda);
	}

    public void PandaEscaped()
    {
		if( InstanceFinder.StatsManager != null)
		{
			InstanceFinder.StatsManager.PandasEscaped++;
		}
        if (onLevelLost != null)
        {
            HDRSystem.PostEvents(gameObject, onLose);
            onLevelLost();
        }
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
        return ScoreCalculator.Score(InstanceFinder.LevelManager.CurrentLevel, InstanceFinder.ComboSystem.LevelDeaths, elapsedTime);	
	}
	
	public int GetTimeScore()
	{
        return ScoreCalculator.TimeScore(InstanceFinder.LevelManager.CurrentLevel, elapsedTime);	
	}
	
	public int Stars()
	{
	    return ScoreCalculator.Stars(InstanceFinder.LevelManager.CurrentLevel, GetScore());	
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
        if (!paused && InstanceFinder.ComboSystem != null && InstanceFinder.ComboSystem.AlivePandas > 0)
        {
            elapsedTime += PandaTime.deltaTime;
        }

        if (InstanceFinder.ComboSystem && InstanceFinder.ComboSystem.AlivePandas <= 0 && onLevelCompleteFlag == false && onLevelComplete != null)
		{
			onLevelCompleteFlag = true;
		    if (onLevelComplete != null)
		    {
		        onLevelComplete();
                HDRSystem.PostEvents(gameObject, onLevelWin);
		    }
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