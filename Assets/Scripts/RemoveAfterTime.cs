using UnityEngine;
using System.Collections;

public class RemoveAfterTime : MonoBehaviour {

	
	public float EventLength = 2f;
	
	void Start () 
	{
		if(this.gameObject.name == "particle_slap_v1(Clone)"  || 
			this.gameObject.name == "particle_slap_v2(Clone)" ||
			this.gameObject.name == "Menu_DismemberedRigidBody(Clone)" ||
			this.gameObject.name == "Menu_Dismembered(Clone)" ||
			this.gameObject.name == "blood_spray" )
		{
			Destroy(gameObject, EventLength);
		}
	}
	
	
}
