using System.Collections.Generic;
using UnityEngine;
using System.Collections;

[AddComponentMenu("NGUI/PunishPanda/Panda Button Click Sound")]
public class UIButtonClickSound : MonoBehaviour {
    [EventHookAttribute("On Click Events")]
    public List<AudioEvent> clickEvents = new List<AudioEvent>();

    [EventHookAttribute("On Press Events")]
    public List<AudioEvent> pressEvents = new List<AudioEvent>();

    void Start()
    {
        GetComponent<UIButton>().OnPressedButton += button => HDRSystem.PostEvents(gameObject, pressEvents);
    }

    void OnClick()
    {
        HDRSystem.PostEvents(InAudioInstanceFinder.DataManager.gameObject, clickEvents);
    }
}
