using UnityEngine;
using System.Collections;

/**
 * Controls the hotspot, as well as activating it by fingure presure
 * and bouncing off the panda when they land on it.
 **/

public class Hotspot : MonoBehaviour {

    /*
     * @todo add arrow when active
     */

    [SerializeField] private float bounceForce = 1f;
    [SerializeField] private float bounceDirection = 1f;

    private bool hotspotActive = false;

    void OnTriggerEnter (Collider collider)
    {
        CheckPandaHit(collider);
    }

    void OnTriggerStay (Collider collider)
    {
        CheckPandaHit(collider);
    }

    void CheckPandaHit (Collider collider)
    {
        PandaStateManager pandaStateManager = collider.GetComponent<PandaStateManager>();
        PandaState pandaState = pandaStateManager.GetState();
        if (pandaState == PandaState.Walking && hotspotActive)
        {
            collider.GetComponent<PandaAI>().Jump(bounceForce, bounceDirection);
        }
        
    }

    public void ActivateHotspot ()
    {
        hotspotActive = true;
		//renderer.material.color = Color.green;
    }

    public void DeactivateHotspot ()
    {
        hotspotActive = false;
		//renderer.material.color = Color.gray;
    }
}
