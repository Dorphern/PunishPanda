using UnityEngine;
using System.Collections;

public class AutoLevelSwitch : MonoBehaviour {
	// Use this for initialization
	void Start () {

        if (IsWideScreen())
            Application.LoadLevel(1);
        else
            Application.LoadLevel(2);
        
	}

    private static bool IsWideScreen()
    {
        if (Screen.width / (float)Screen.height > 1.4)
            return true;
        else
            return false;
    }

}
