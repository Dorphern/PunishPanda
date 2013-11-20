﻿using UnityEngine;
using System.Collections;

public class Panda : MonoBehaviour {

    public void PandaKilled (bool fromTrap, bool perfect)
    {
       
        if (InstanceFinder.ComboSystem != null)
        {
            InstanceFinder.ComboSystem.OnPandaDeath(perfect);
        }
    }

	void Start () 
	{
        InstanceFinder.ComboSystem.RegisterPanda();
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
