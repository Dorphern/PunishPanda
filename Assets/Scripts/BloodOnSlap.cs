using UnityEngine;
using System.Collections;

public class BloodOnSlap : MonoBehaviour {
	
	/*****************************************************
	 *  This loads the BloodSplat object (in Resources) 
	 *  and instantiates it based on Panda's position. We
	 *  getDirection of Panda to know which direction we
	 *  want to emmit the blood. Function is called on a Slap.
	*********************************************************/
		
	private PandaStateManager pandaStateManager;

	
	void Start ()
    {
        pandaStateManager = GetComponent<PandaStateManager>();
	}
	
	public void EmmitSlapBlood()
	{
		GameObject BloodSplat = Instantiate(Resources.Load("BloodSplat")) as GameObject;
		if(pandaStateManager.GetDirection() == PandaDirection.Right)
			{
				//Debug.Log ("gave position");
				BloodSplat.transform.position = transform.position;
			}
			else
			{
				BloodSplat.transform.position = transform.position;
				BloodSplat.transform.rotation = Quaternion.LookRotation(Vector3.back);
			}	
	}
}
