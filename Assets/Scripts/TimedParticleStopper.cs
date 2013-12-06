using UnityEngine;
using System.Collections;

public class TimedParticleStopper : MonoBehaviour {
	public float time;
	// Use this for initialization
	void Start () {
		Invoke("StopParticles", time);
	}
	
	void StopParticles()
	{
		this.particleSystem.Stop();
	}
}
