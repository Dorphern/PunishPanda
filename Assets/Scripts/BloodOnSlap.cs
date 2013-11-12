using UnityEngine;
using System.Collections.Generic;

public enum Version {v1, v2}
public class BloodOnSlap : MonoBehaviour {
	
	/*****************************************************
	 *  This loads the BloodSplat object (in Resources) 
	 *  and instantiates it based on Panda's position. We
	 *  getDirection of Panda to know which direction we
	 *  want to emmit the blood. Function is called on a Slap.
	*********************************************************/
		
	private PandaStateManager pandaStateManager;
	public Version particleVersion;
	private string objectName;
	
	[EventHookAttribute("Slap")]
	[SerializeField] List<AudioEvent> slapAudioEvents = new List<AudioEvent>();

	
	void Start ()
    {
        pandaStateManager = GetComponent<PandaStateManager>();
	}
	
	public void EmmitSlapBlood()
	{
		GameObject BloodSplat;
		
		if(particleVersion == Version.v1)
		{
			objectName = "particle_slap_v1";	
		}
		else
		{
			objectName = "particle_slap_v2";
		}
		
		for(int i = 0; i < slapAudioEvents.Count; ++i)
		{
			HDRSystem.PostEvent(gameObject, slapAudioEvents[i]);
		}
		if(pandaStateManager.GetDirection() == PandaDirection.Right)
		{
			BloodSplat = Instantiate(Resources.Load(objectName),transform.position, Quaternion.identity) as GameObject;		
		}
		else
		{
			BloodSplat = Instantiate(Resources.Load(objectName),transform.position, Quaternion.LookRotation(Vector3.back)) as GameObject;
		}	
	}
}
