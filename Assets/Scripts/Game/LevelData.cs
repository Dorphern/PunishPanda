using System;
using PunishPanda.Game;
using UnityEngine;

[Serializable]
public class LevelData
{
    [SerializeField] public string LevelName = "";
    [SerializeField] public GameModes Mode;

	
	public Texture2D funFactPicture;
	public Texture2D objectiveScreen;
	public string funFact;
	public LevelScore score;
	


    [HideInInspector] [SerializeField] 
    public int HighScore;
    [HideInInspector] [SerializeField]
    public bool Unlocked; //1 == unlocked, 0 = locked
}

