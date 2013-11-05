using System;
using PunishPanda.Game;
using UnityEngine;

[Serializable]
public class LevelData
{
    [SerializeField] public string LevelName = "";
    [SerializeField] public GameModes Mode;
}
