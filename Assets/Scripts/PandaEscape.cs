using UnityEngine;
using System.Collections;

public class PandaEscape : MonoBehaviour {

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == 8)
        {
            InstanceFinder.GameManager.ActiveLevel.PandaEscaped();
        }
    }
}
