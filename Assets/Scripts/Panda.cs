using UnityEngine;
using System.Collections;

public class Panda : MonoBehaviour {

    public void PandaKilled (bool fromTrap, bool perfect)
    {
        InstanceFinder.GameManager.ActiveLevel.OnPandaDeath(fromTrap, perfect);
    }

	void Start () 
	{
	    InstanceFinder.GameManager.ActiveLevel.RegisterPanda();
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
