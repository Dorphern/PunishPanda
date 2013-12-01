using UnityEngine;
using System.Collections.Generic;

public enum Version {v1, v2}
public class BloodOnSlap : MonoBehaviour {
	
	/*****************************************************
	 *  This loads the BloodSplat object (in Resources) 
	 *  and instantiates it based on Panda's position.
	 *  Function is called on a Slap (in PandaAI).
	*********************************************************/
		
	public GameObject PandaMouthFront; //use for start position of emmision
	public GameObject PandaMouthBack; //use for turn-slap
	public Version particleVersion;
	private string objectName;
	private float timeStamp1;
	private float timeStamp2;
	private GameObject slapParticles;
	// 3D vector controlling the direction of the blood particle object
	private Vector3 projectionDirection = Vector3.right;

	[EventHookAttribute("Slap")]
	[SerializeField] List<AudioEvent> slapAudioEvents = new List<AudioEvent>();
	
	void Start ()
    {	
		if(particleVersion == Version.v1)
		{
			objectName = "particle_slap_v1";	
		}
		else
		{
			objectName = "particle_slap_v2";
		}
		
		slapParticles = Instantiate(Resources.Load(objectName)) as GameObject;
	}
	
	
	//Method for emmiting blood in the same direction(and angle) as slap.
	//NOTE: we use this when panda is slapped in the BACK.
	public void EmmitSlapBlood(Vector2 slapDirection)
	{
		float diff;
		
		for(int i = 0; i < slapAudioEvents.Count; ++i)
		{
			
			HDRSystem.PostEvent(gameObject, slapAudioEvents[i]);
		}
		
		// set the angle of the splat to be the same in both XY and XZ planes
		projectionDirection.x = slapDirection.x;
		projectionDirection.y = slapDirection.y;
		projectionDirection.z = Mathf.Abs(slapDirection.x);
		
		
		timeStamp1 = Time.realtimeSinceStartup;
		//Debug.Log (timeStamp1);
		
		diff = timeStamp1 - timeStamp2;
		diff = Mathf.Abs (diff);
		//Debug.Log ("diff:"+diff);
		
		if(diff > 0.1)
		{
			//Object instantiates facing the Z-axis direction
			Instantiate(slapParticles ,PandaMouthFront.transform.position, Quaternion.LookRotation(projectionDirection));
		}


	} 
	
	//NOTE: use this when panda is slapped from its FRONT (and does a turn)
	public void EmmitSlapBloodOnTurn(Vector2 slapDirection)
	{
		float diff;
		
		for(int i = 0; i < slapAudioEvents.Count; ++i)
		{
			
			HDRSystem.PostEvent(gameObject, slapAudioEvents[i]);
		}
		
		// set the angle of the splat to be the same in both XY and XZ planes
		projectionDirection.x = slapDirection.x;
		projectionDirection.y = slapDirection.y;
		projectionDirection.z = Mathf.Abs(slapDirection.x);
		
		timeStamp2 = Time.realtimeSinceStartup;
		//Debug.Log (timeStamp2);
		
		diff = timeStamp1 - timeStamp2;
		diff = Mathf.Abs (diff);
		//Debug.Log ("diff:"+diff);
		//diff registered at 0.018 sec away...
		
		
		if(diff > 0.1)
		{
			//Object instantiates facing the Z-axis direction	
			Instantiate(Resources.Load(objectName),PandaMouthBack.transform.position, Quaternion.LookRotation(projectionDirection));
		}


	} 
}
