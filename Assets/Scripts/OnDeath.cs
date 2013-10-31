using UnityEngine;
using System.Collections;

public class OnDeath : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        gameObject.GetComponent<PandaStateManager>().ChangeState(PandaState.Died);

	}
}
