using UnityEngine;
using System.Collections;

public class RemoveAfterTime : MonoBehaviour {

	
	public float EventLength = 2f;
	
	void Start () 
	{
		if(this.gameObject.name == "particle_slap_v1(Clone)"  || 
			this.gameObject.name == "particle_slap_v2(Clone)" ||
			this.gameObject.name == "Menu_DismemberedRigidBody(Clone)")
		{
			Destroy(gameObject, EventLength);
		}
	}
	
	
}
