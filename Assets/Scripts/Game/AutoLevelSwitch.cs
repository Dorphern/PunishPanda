using UnityEngine;
using System.Collections;

public class AutoLevelSwitch : MonoBehaviour {
	// Use this for initialization
	void Start () {

        if (WidescreenCheck.IsWideScreen())
            Application.LoadLevel(1);
        else
            Application.LoadLevel(2);
        
	}

}
