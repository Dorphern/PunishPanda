using PunishPanda;
using UnityEngine;
using System.Collections.Generic;
using PunishPanda.Game;


public class Level : MonoBehaviour
{
    [SerializeField] private LevelScore levelScore = new LevelScore();
	public Texture2D FunFactsTexture;
	public string    FunFactsText;

    private float elapsedTime;
    private bool paused;
    private int totalPandaCount;
    private int alivePandas;
    private int normalPandaKills;
    private int perfectPandaKills;
	private bool onLevelCompleteFlag = false;
	[System.NonSerializedAttribute]
	public List<PandaAI> pandas = new List<PandaAI>();  
	
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

    public void RegisterPanda(PandaAI panda)
    {
        totalPandaCount += 1;
        alivePandas += 1;
		pandas.Add(panda);
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
	
	public int GetScore()
	{
		return ScoreCalculator.Score(levelScore, perfectPandaKills, normalPandaKills, elapsedTime);	
	}
	
	public int Stars()
	{
		return ScoreCalculator.Stars(levelScore, GetScore());	
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
		
		if(alivePandas <= 0 && onLevelCompleteFlag==false && onLevelComplete != null)
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
	
    # endregion
}