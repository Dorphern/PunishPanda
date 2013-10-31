using UnityEngine;
using System.Collections;

public class SlapEvent : MonoBehaviour
{
    Transform myTransform;
	
	public float slapLength = 2f;
	
    public float timeBoost;
    public float currentSpeed;
    public float originalSpeed;

    # region public
    public void BoostPanda(float hitSpeed, Vector3 swipe)
    {
    	StartCoroutine(BoostSpeed(timeBoost, hitSpeed));
    }
    #endregion

    // Use this for initialization
    void Start()
    {
        currentSpeed = 2;
        myTransform = gameObject.transform;
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

