using UnityEngine;
using System.Collections.Generic;

public enum Version {v1, v2}
public class BloodOnSlap : MonoBehaviour {
	
	/*****************************************************
	 *  This loads the BloodSplat object (in Resources) 
	 *  and instantiates it based on Panda's position.
	 *  Function is called on a Slap (in PandaAI).
	*********************************************************/
		
	private PandaStateManager pandaStateManager;
	public Version particleVersion;
	private string objectName;
	
	// 3D vector controlling the direction of the blood particle object
	private Vector3 projectionDirection = Vector3.right;

	[EventHookAttribute("Slap")]
	[SerializeField] List<AudioEvent> slapAudioEvents = new List<AudioEvent>();
	
	void Start ()
    {
        pandaStateManager = GetComponent<PandaStateManager>();			
		if(particleVersion == Version.v1)
		{
			objectName = "particle_slap_v1";	
		}
		else
		{
			objectName = "particle_slap_v2";
		}
	}
	
	
	//Method for emmiting blood in the same direction(and angle) as slap.
	public void EmmitSlapBloodWithAngle(Vector2 slapDirection)
	{
		for(int i = 0; i < slapAudioEvents.Count; ++i)
		{
			
			HDRSystem.PostEvent(gameObject, slapAudioEvents[i]);
		}
		
		// set the angle of the splat to be the same in both XY and XZ planes
		projectionDirection.x = slapDirection.x;
		projectionDirection.y = slapDirection.y;
		projectionDirection.z = Mathf.Abs(slapDirection.x);
		
		//Object instantiates facing the Z-axis direction	
		Instantiate(Resources.Load(objectName),transform.position, Quaternion.LookRotation(projectionDirection));
		

	} 
}
