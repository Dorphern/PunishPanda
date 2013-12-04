using UnityEngine;
using System.Collections;

public class SpikeTrapTrigger : MonoBehaviour {

    [SerializeField] TriggeredImpalerSpikeTrap spikeTrap;

    void OnTriggerEnter (Collider collider)
    {
		Collidable collidable = collider.GetComponent<Collidable>();
		if(collidable != null && collidable.type == CollidableTypes.Panda)
		{
    		spikeTrap.Fire();
		}
    }

}
