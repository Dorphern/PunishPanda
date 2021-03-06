﻿using UnityEngine;
using System.Collections;

public class Panda : MonoBehaviour {

    public void PandaKilled (bool fromTrap, bool perfect)
    {
        InstanceFinder.ComboSystem.OnPandaDeath(perfect);
		InstanceFinder.GameManager.ActiveLevel.RemovePandaAIRef(GetComponent<PandaAI>());
		if(InstanceFinder.StatsManager!=null)
		{
			InstanceFinder.StatsManager.PandasKilled++;
			if(perfect == true)
			{
				InstanceFinder.StatsManager.PandasKilledPerfect++;
				InstanceFinder.StatsManager.LiterBlood += PandaRandom.RandomBlood(1f);
			}
			else
			{
				InstanceFinder.StatsManager.LiterBlood += PandaRandom.RandomBlood(0.7f);
			}
		}
        // Track panda death
        GA.API.Design.NewEvent("panda:died", transform.position);
    }

	void Start () 
	{
        InstanceFinder.ComboSystem.RegisterPanda();
		InstanceFinder.GameManager.ActiveLevel.AddPandaAIRef(GetComponent<PandaAI>());
	}

    public void EnableColliders (bool enable)
    {
        Collider[] colliders = GetComponents<Collider>();
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = enable;
        }
    }
}
