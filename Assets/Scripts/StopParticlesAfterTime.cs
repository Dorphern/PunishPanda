using UnityEngine;
using System.Collections;

public class StopParticlesAfterTime : MonoBehaviour 
{
	public float timeToWait = 3f;
	
	IEnumerator Start () 
	{
		yield return new WaitForSeconds(timeToWait);
		
		foreach(ParticleEmitter emitter in gameObject.GetComponentsInChildren<ParticleEmitter>())
		{
			emitter.emit = false;	
		}
		
		yield return null;
	}
	
}
