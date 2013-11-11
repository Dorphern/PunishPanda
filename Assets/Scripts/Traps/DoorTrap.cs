using UnityEngine;
using System.Collections;

public class DoorTrap : TrapBase {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	protected override bool PandaAttemptKill (PandaAI pandaAI, bool isPerfect)
	{
		return true;	
	}
}
