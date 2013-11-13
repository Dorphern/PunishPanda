using PunishPanda;
using UnityEngine;
using System.Collections.Generic;
using PunishPanda.Game;


public class Level : MonoBehaviour
{

    

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
	
	[SerializeField] private LevelScore levelScore = new LevelScore();
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
        }
    }

}