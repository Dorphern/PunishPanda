using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class TestScript : MonoBehaviour
{
    public AudioBankLink link;
    public AudioEvent audioEvent;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Application.LoadLevel(1);
        }
        
        if (Input.GetKeyDown(KeyCode.U))
        {
            //RuntimeEventWorker.PlayAttachedTo(gameObject, audioEvent);
        }
    }
}

