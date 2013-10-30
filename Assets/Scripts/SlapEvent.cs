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
    private Vector3 swipeVector;

    # region public
    public void Slap(float _hitSpeed, Vector3 swipe)
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

        // SWIPE
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 tempMouStart = Input.mousePosition;
            mousePosStart = new Vector3(tempMouStart.x, tempMouStart.y, 10);
        }
        if(Input.GetMouseButtonUp(0))
        {
            Vector3 tempMouEnd = Input.mousePosition;
            mousePosEnd = new Vector3(tempMouEnd.x, tempMouEnd.y, 0);
            mouseVector = (mousePosEnd - mousePosStart);
           
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Slap(hitSpeed,swipeVector);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Slap(-hitSpeed, swipeVector);
        }


        myTransform.Translate((new Vector3(currentSpeed, 0 , 0)) * Time.deltaTime);
    }

    #region private
    IEnumerator BoostSpeed(float waitForSeconds, float boostSpeed)
    {
        currentSpeed = boostSpeed;
        gameObject.renderer.material.color = Color.red;


        // Call hitAnimation (direction)

        // Call bloodSpatter with (direction/angle)


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

