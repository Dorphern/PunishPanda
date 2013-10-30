using UnityEngine;
using System.Collections;

public class SlapEvent : MonoBehaviour
{


    public float hitSpeed;

    Transform myTransform;

    public float timeBoost;
    public float currentSpeed;
    public float originalSpeed;

    private Vector3 mousePosStart;
    private Vector3 mousePosEnd;
    private Vector3 mouseVector;

    # region public
    public void Slap(float _hitSpeed)
    {
            StartCoroutine(BoostSpeed(timeBoost, _hitSpeed));
            
    }
    #endregion

    // Use this for initialization
    void Start()
    {
        currentSpeed = 2;
        myTransform = gameObject.transform;

    }

    // Update is called once per frame
    void FixedUpdate() {

        if(Input.GetMouseButtonDown(0))
        {
            Vector3 tempMouStart = Input.mousePosition;
            mousePosStart = new Vector3(tempMouStart.x, tempMouStart.y, 10);
        }
        if(Input.GetMouseButtonUp(0))
        {
            mousePosEnd = Input.mousePosition;
            mouseVector = (mousePosEnd - mousePosStart);
            Debug.DrawRay(mousePosStart, mouseVector);
        }
        Debug.DrawRay(mousePosStart, mouseVector);
        Debug.Log("MouseVEctor " + mouseVector);
        Debug.Log("MouseEnd " + mousePosEnd);
        Debug.Log("MouseSTa " + mousePosStart);

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Slap(hitSpeed);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Slap(-hitSpeed);
        }

        myTransform.Translate((new Vector3(currentSpeed, 0 , 0)) * Time.deltaTime);
    }

    #region private
    IEnumerator BoostSpeed(float waitForSeconds, float boostSpeed)
    {
        currentSpeed = boostSpeed;
        gameObject.renderer.material.color = Color.red;


        // Call hitAnimation


        yield return new WaitForSeconds(waitForSeconds);


        // Call walkAnimation


        gameObject.renderer.material.color = Color.white;
        if(boostSpeed < 0)
        {
            currentSpeed = -originalSpeed;
        }
        else if (boostSpeed > 0)
        {
            currentSpeed = originalSpeed;
        }
        
       // timeBoost = 0;
    }
    #endregion


}

