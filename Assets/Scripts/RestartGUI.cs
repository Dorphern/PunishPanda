using UnityEngine;
using System.Collections;

public class RestartGUI : MonoBehaviour {

    private float restartLevelX = 0.01f;
    private float restartLevelY = 0.01f;
    private float restartLevelWidth = 0.2f;
    private float restartLevelHeight = 0.06f;

    void Start()
    {


    }
    void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width * restartLevelX, Screen.height * restartLevelY, Screen.width * restartLevelWidth, Screen.height * restartLevelHeight), "RESTART"))
        {
            
            Application.LoadLevel(0);
        }
    }
}
