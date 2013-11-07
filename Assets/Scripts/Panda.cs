using UnityEngine;
using System.Collections;

public class Panda : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    InstanceFinder.GameManager.ActiveLevel.RegisterPanda(this);
	}

    void OnDestroy()
    {
        InstanceFinder.GameManager.ActiveLevel.OnPandaDeath(this);
    }
}
