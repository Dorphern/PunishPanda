using System;
using PunishPanda.Game;
using UnityEngine;

[Serializable]
public class LevelData
{
    [SerializeField] public string LevelName = "";
    [SerializeField] public GameModes Mode;

	
	
	public LevelScore score;
	


    [HideInInspector] [SerializeField] 
    public int HighScore;
    [HideInInspector] [SerializeField]
    public bool UnlockedLevel; //1 == unlocked, 0 = locked

    [HideInInspector] [SerializeField] 
    public bool UnlockedFunFact; //1 == unlocked, 0 = locked
}

