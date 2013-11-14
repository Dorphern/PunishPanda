using UnityEngine;
using System.Collections;

public class SideWallButton : TrapActivator {

	public float activationTimeLength = 3f;
	private Collidable colliderType;
	
	void OnTriggerEnter(Collider collider)
	{
		colliderType = collider.GetComponent<Collidable>();
		if(colliderType == null) return;
		
		if(colliderType.type == CollidableTypes.Panda)
		{
            ActivateTraps();
			StartCoroutine(DeactivateTrap());
		}
	}
	
	IEnumerator DeactivateTrap()
	{
		yield return new WaitForSeconds(activationTimeLength);
        DeactivateTrap();
	}
}
