using System.Collections.Generic;
using UnityEngine;
using System.Collections;

[AddComponentMenu("HDR Audio/Manual Event Poster")]
public class EventTester : MonoBehaviour {

	[SerializeField]
    public List<AudioEvent> Events = new List<AudioEvent>();
}
