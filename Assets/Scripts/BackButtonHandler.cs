using UnityEngine;
using System.Collections;

public class BackButtonHandler : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
        }
    }
}
