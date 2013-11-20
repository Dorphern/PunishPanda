using UnityEngine;
using System.Collections;

public class SpikeTrapTrigger : MonoBehaviour {

    [SerializeField] TriggeredImpalerSpikeTrap spikeTrap;

    void OnTriggerEnter (Collider collider)
    {
        spikeTrap.Fire();
    }

}
