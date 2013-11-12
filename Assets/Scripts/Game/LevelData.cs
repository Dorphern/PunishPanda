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
	
}
