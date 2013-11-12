using UnityEngine;
using System.Collections;

public class Panda : MonoBehaviour {

    public void PandaKilled (bool fromTrap, bool perfect)
    {
        InstanceFinder.GameManager.ActiveLevel.OnPandaDeath(fromTrap, perfect);
    }

	void Start () {
	    InstanceFinder.GameManager.ActiveLevel.RegisterPanda();
	}
}
