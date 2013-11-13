using UnityEngine;
using System.Collections;

public class UIDisableCollider : MonoBehaviour {
	
	//public Component[] ButtonColliders;
	
	
	public void DisableCollider()
	{
		(gameObject.GetComponent(typeof(Collider)) as Collider).enabled = false;
		
		//ButtonColliders =(gameObject.GetComponentsInChildren(typeof(Collider)) as Collider);
		//foreach(Collider box in ButtonColliders)
		//{
		//	box.enabled = false;
		//}
	}
	
	public void ActivateCollider()
	{
		(gameObject.GetComponent(typeof(Collider)) as Collider).enabled = true;
	}
}
