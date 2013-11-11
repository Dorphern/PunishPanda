using UnityEngine;
using System.Collections;

public class SideWallButton : MonoBehaviour {

	public TrapBase trap;
	public float activationTimeLength = 3f;
	
	void Start () 
	{
	}
	
	void Update () 
	{
		
	}
	
	void OnTriggerEnter(Collider collider)
	{
		if(collider.GetComponent<Collidable>().type == CollidableTypes.Panda)
		{
			if(trap.IsActive() == false)
			{
				trap.ActivateTrap();
				StartCoroutine(DeactivateTrap());
			}
		}
	}
	
	IEnumerator DeactivateTrap()
	{
		yield return new WaitForSeconds(activationTimeLength);
		
		trap.DeactivateTrap();
	}
}
