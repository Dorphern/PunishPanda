using System.Collections.Generic;
using UnityEngine;
using System.Collections;

[AddComponentMenu("NGUI/PunishPanda/Panda Button Click Sound")]
public class UIButtonClickSound : MonoBehaviour {
    [EventHookAttribute("On Click Events")]
    public List<AudioEvent> clickEvents = new List<AudioEvent>();

    void OnClick()
    {
        HDRSystem.PostEvents(InAudioInstanceFinder.DataManager.gameObject, clickEvents);
    }
}
