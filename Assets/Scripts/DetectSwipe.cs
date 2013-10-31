using UnityEngine;
using System.Collections;

public class DetectSwipe : MonoBehaviour {

    
    private float startTime;
    public float minDistance;
    public float minAngle;
    public float minSpeed;
    public Vector2 endPos;
    public Vector2 startPos;
    public int pandaLayer = 8;
    public float minSpeedRaycast;
    public float SphereCastRadius;
    public float distanceToLevel;
	// Use this for initialization
	void Start () {
	

	}
	
	// Update is called once per frame
	void Update () {

       if(Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
            startTime = Time.time;
        }
        if(Input.GetMouseButtonUp(0))
        {
            endPos = Input.mousePosition;
            Vector2 deltaPos = endPos - startPos;
            CalcSwipe(deltaPos, endPos, startPos, startTime);
        }


        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            startPos = Input.GetTouch(0).position;
            startTime = Time.time;
            Vector2 deltaPos = Input.GetTouch(0).deltaPosition;
            float deltaTime = Input.GetTouch(0).deltaTime;

            CalcSwipe(deltaPos, endPos, startPos, deltaTime);
        }

  }

    void CalcSwipe(Vector2 delta, Vector2 endPosition, Vector2 startPosition, float timeDelta)
    {
        float distance = delta.magnitude;

        float angle = Vector2.Angle(Vector2.right.normalized, delta.normalized);

        float speed = distance / timeDelta;


        Debug.Log("Distance: " + distance + " Angle: " + angle + " Speed: " + speed + "Duration: " + timeDelta);
        DebugStreamer.message = ("Distance: " + distance + " Angle: " + angle + " Speed: " + speed + "Duration: " + timeDelta);



        if (speed > minSpeedRaycast)
        {
            RaycastHit hit;
            Vector3 origin = new Vector3(startPosition.x, startPosition.y, 0f);
            Vector3 rayDir = new Vector3(startPosition.x, startPosition.y, 10f);

            if(Physics.SphereCast(origin, SphereCastRadius, rayDir, out hit, distanceToLevel))
            {

            }
        }

        // Left-Right Swipe
        if (startPosition.x < endPosition.x)
        {
            if (angle < 0) 
            {
                angle = angle * -1.0f;
                
            }

            if (distance > minDistance && angle < minAngle && speed > minSpeed)
            {
                // Call swipe events here
            }
        }
            // Right-Left swipe
        else if(startPosition.x > endPosition.x)
        {
            if (angle < 0) 
            {
                angle = angle * -1.0f;

            }

            if (distance > minDistance && angle < minAngle && speed > minSpeed)
            {
                // Call swipe events here
            }
        }
    }
}
