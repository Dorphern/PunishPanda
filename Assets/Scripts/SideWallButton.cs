using UnityEngine;
using System.Collections;

public class SideWallButton : MonoBehaviour {

	public TrapBase trap;
	public float activationTimeLength = 3f;
	private Collidable colliderType;
	
	void Start () 
	{
	}
	
	void Update () 
	{
		
	}
	
	void OnTriggerEnter(Collider collider)
	{
		colliderType = collider.GetComponent<Collidable>();
		if(colliderType == null) return;
		
		if(colliderType.type == CollidableTypes.Panda)
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
