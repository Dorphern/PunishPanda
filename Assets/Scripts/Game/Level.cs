using PunishPanda;
using UnityEngine;
using System.Collections.Generic;
using PunishPanda.Game;


public class Level : MonoBehaviour
{
    [SerializeField] private LevelScore levelScore = new LevelScore();

    public void Pause()
    {
        paused = true;
    }

    public void Continue()
    { 
        paused = false;
    }

    public void RegisterPanda(Panda panda)
    {
        totalPandaCount += 1;
        alivePandas += 1;
    }

    public void OnPandaDeath(Panda panda)
    {
        alivePandas -= 1;
        pandaKills += 1;
    }


    public float LevelTime
    {
        get
        {
            return elapsedTime;
        }
    }

    private float elapsedTime;
    private bool paused;
    private int totalPandaCount;
    private int alivePandas;
    private int pandaKills;

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
            Debug.Log(elapsedTime);
        }
        

    }

    private void OnGUI()
    {
        var levelManger = InstanceFinder.LevelManager;

        if (alivePandas > 0)
        {
            if (levelManger != null)
            {
                Rect nextRect = new Rect(Screen.width - 150, 50, 100, 80);
                if (GUI.Button(nextRect, "Next level"))
                {
                    levelManger.LoadNextLevel();
                }

                nextRect = new Rect(Screen.width - 300, 50, 100, 80);
                if (GUI.Button(nextRect, "Main Menu"))
                {
                    levelManger.LoadMainMenu();
                }

            }

            Rect killButton = new Rect(50, 50, 100, 80);
            if (GUI.Button(killButton, "(Fake) Kill Panda"))
            {
                OnPandaDeath(null);
            }
        }
        else
        {
            int heightOffset = 30;
            Rect nextRect = new Rect(Screen.width/2 - 75, 50, 100, 40);
            int score = ScoreCalculator.Score(levelScore, pandaKills, alivePandas, elapsedTime);
            GUI.Label(nextRect, "Score: " + score);
            nextRect.y += heightOffset;
            GUI.Label(nextRect, "Panda Kills: " + pandaKills);
            nextRect.y += heightOffset;

            GUI.Label(nextRect, "Time Score: " + ScoreCalculator.TimeScore(levelScore, elapsedTime));
            nextRect.y += heightOffset;
            GUI.Label(nextRect, "Stars: " + ScoreCalculator.Stars(levelScore, score));
            if (alivePandas == 0)
            {
                nextRect.y += heightOffset;
                nextRect.width += 20;
                GUI.Label(nextRect, "Perfect Panda Kill!");
                nextRect.width -= 20;
            }
            nextRect.y += heightOffset*2;
            if (GUI.Button(nextRect, "Next Level"))
            {
                levelManger.LoadNextLevel();
            }

            nextRect.y += heightOffset * 2 ;
            if (GUI.Button(nextRect, "Replay"))
            {
                levelManger.Reload();
            }
            nextRect.y += heightOffset * 2;
            if (GUI.Button(nextRect, "Main Menu"))
            {
                levelManger.LoadMainMenu();
            }
        }
    }
}