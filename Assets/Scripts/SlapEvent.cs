using UnityEngine;
using System.Collections;

public class SlapEvent : MonoBehaviour
{
    public float hitSpeed;

    Transform myTransform;

    public float timeBoost;
    public float currentSpeed;
    public float originalSpeed;

    private Vector3 swipe;
    private PandaDirection swipeDirection;

    # region public

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

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            StartCoroutine(Slap(timeBoost, hitSpeed, swipeDirection));
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            StartCoroutine(Slap(timeBoost, -hitSpeed, swipeDirection));
        }

        myTransform.Translate((new Vector3(currentSpeed, 0 , 0)) * Time.deltaTime);
    }

    #region private
    IEnumerator Slap(float waitForSeconds, float boostSpeed, PandaDirection direction)
    {
        if (gameObject.GetComponent<PandaStateManager>().GetDirection() == direction)
        {
            // inrease movementSpeed game.GetComponent<Movement>().speed = speedBoost;
        }
        else if (gameObject.GetComponent<PandaStateManager>().GetDirection() != direction)
        {
            gameObject.GetComponent<PandaStateManager>().ChangeDirection(direction);
        }
        currentSpeed = boostSpeed;

        gameObject.renderer.material.color = Color.red;


        // Call hitAnimation (direction)


        // Call bloodSpatter with (direction/angle)


        yield return new WaitForSeconds(waitForSeconds);


        // Call walkAnimation after slap i.e. with blood etc. 
        
        // decrease speed / end speedbost
        gameObject.renderer.material.color = Color.white;
        if (boostSpeed < 0)
        {
            currentSpeed = -originalSpeed;
        }
        else if (boostSpeed > 0)
        {
            currentSpeed = originalSpeed;
        }
    }
    #endregion


}

