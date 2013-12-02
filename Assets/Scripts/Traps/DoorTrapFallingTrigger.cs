using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DoorTrapFallingTrigger : MonoBehaviour {
	
	private List<PandaAI> pandasOnTrap;
	
	void Start () 
	{
		pandasOnTrap = new List<PandaAI>();
	}
	
	public void PandasFalling()
	{
		for(int i = 0; i < pandasOnTrap.Count; i++)
		{
			pandasOnTrap[i].Falling();
		}
	}
	
	private void OnTriggerEnter (Collider collider)
    {
        Collidable collidable = collider.GetComponent<Collidable>();

        if (collidable != null && collidable.type == CollidableTypes.Panda)
        {
            pandasOnTrap.Add(collider.GetComponent<PandaAI>());
        }
	}
	
	private void OnTriggerExit (Collider collider)
    {
        Collidable collidable = collider.GetComponent<Collidable>();

        if (collidable != null && collidable.type == CollidableTypes.Panda)
        {
            pandasOnTrap.Remove(collider.GetComponent<PandaAI>());
        }
	}
}
