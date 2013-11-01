using UnityEngine;
using System.Collections;

public class FingerBlocking : MonoBehaviour {

	//cameraOffset determines where to place blockade on Z-axis
	private float cameraOffset;
    public float minSpeed = 30;
    public float firstMaxSpeed = 200;
    private Vector3 lastPosition;
    public Vector3 currentVel;
	
	void Start () {

		collider.enabled = false;
        lastPosition = transform.position;

	}
	
	void Update () {
	} 
	
	
	public void ActivateBlockade (Vector3 mousePos)
	{
		collider.enabled = true;
        
        StartCoroutine(DeltaPosition(lastPosition));
		cameraOffset = Camera.main.transform.position.z; 
		mousePos.z = Mathf.Abs(cameraOffset);	
		
	    Vector3 pos = Camera.main.ScreenToWorldPoint(mousePos);
        pos.z = 0;
        transform.position = pos;
        

	}
	
	public void DeactivateBlockade()
	{
		collider.enabled = false;
		//DestroyImmediate(this.gameObject);
	}

   IEnumerator DeltaPosition(Vector3 lastPos)
    {
        yield return new WaitForEndOfFrame();

        Vector3 delta = transform.position - lastPos;
        float dist = delta.magnitude;
        float speed = dist / Time.deltaTime;
        if (speed > firstMaxSpeed)
            yield break;

        delta.z = 0;        
        float angle = Vector3.Angle(Vector3.right, delta);

        lastPosition = transform.position;
        Debug.Log(currentVel.magnitude);
        DebugStreamer.message = ("speed " + speed.ToString() + "Angle " + angle.ToString() + "isTrigger " + collider.isTrigger);
        
        if (speed > minSpeed)
        {            
            collider.isTrigger = true;
            // send angle && speed to bloodSplatter
            
        }
    }
}
