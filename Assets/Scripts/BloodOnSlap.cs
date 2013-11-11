using UnityEngine;
using System.Collections;

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
	
	//Method for emmiting blood simply -- left or right.
	public void EmmitSlapBlood()
	{
		GameObject BloodSplat;
				
		if(pandaStateManager.GetDirection() == PandaDirection.Right)
		{
			BloodSplat = Instantiate(Resources.Load(objectName),transform.position, Quaternion.identity) as GameObject;		
		}
		else
		{
			BloodSplat = Instantiate(Resources.Load(objectName),transform.position, Quaternion.LookRotation(Vector3.back)) as GameObject;
		}	
	}
	
	//Method for emmiting blood in the same direction as slap.
	public void EmmitSlapBloodWithAngle(Vector2 slapDirection)
	{
		// set the angle of the splat to be the same in both XY and XZ planes
		projectionDirection.x = slapDirection.x;
		projectionDirection.y = slapDirection.y;
		projectionDirection.z = Mathf.Abs(slapDirection.x);
		
		GameObject BloodSplat;
		//Object instantiates facing the Z-axis direction
		BloodSplat = Instantiate(Resources.Load(objectName),transform.position, Quaternion.LookRotation(projectionDirection)) as GameObject;	
		

	} 
}
