using UnityEngine;
using System.Collections;

public class RemoveAfterTime : MonoBehaviour {

	
	public float EventLength = 2f;
	
	void Start () 
	{
		if(this.gameObject.name == "BloodSplat(Clone)")
		{
			Destroy(gameObject, EventLength);
			//Debug.Log ("Destroyed BloodSplat object");
		}
	}
	
	
}
