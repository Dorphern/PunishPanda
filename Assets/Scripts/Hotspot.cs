using UnityEngine;
using System.Collections;

/**
 * Controls the hotspot, as well as activating it by fingure presure
 * and bouncing off the panda when they land on it.
 **/

public class Hotspot : MonoBehaviour {

    [SerializeField] private float bounceForce = 1f;
    [SerializeField] private float bounceDirection = 1f;

    private bool hotspotActive = false;

    void OnMouseDown ()
    {
        SetIsActive(true);
    }

    void OnMouseExit ()
    {
        SetIsActive(true);
    }

    void OnMouseUp ()
    {
        SetIsActive(false);
    }

    void SetIsActive (bool isDown)
    {
        hotspotActive = IsActive() && isDown;
    }

    void OnTriggerEnter (Collider collider)
    {
        CheckPandaHit(collider);
    }

    void OnTriggerStay (Collider collider)
    {
        CheckPandaHit(collider);
    }

    bool IsActive () {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        if (hit.collider == null) return false; 
        return hit.collider.gameObject == gameObject;
    }

    void CheckPandaHit (Collider collider)
    {
        PandaStateManager pandaStateManager = collider.GetComponent<PandaStateManager>();
        PandaState pandaState = pandaStateManager.GetState();
        if ((pandaState == PandaState.Walking
            /*|| pandaState == PandaState.Falling*/)
            /*&& hotspotActive*/)
        {
            collider.GetComponent<PandaAI>().Jump(bounceForce, bounceDirection);
        }
        
    }
}
