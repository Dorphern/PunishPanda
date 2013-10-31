using UnityEngine;
using System.Collections;

public class FingerBlocking : MonoBehaviour {
	

	//cameraOffset determines where to place blockade on Z-axis
	private float cameraOffset;
    private Transform boxTransform;
    public float minVelocity;
    private Vector3 lastPosition;
    public Vector3 currentVel;
	
	void Start () {
		collider.enabled = false;
        boxTransform = gameObject.transform;
        lastPosition = Vector3.zero;
	}

	
	void Update () {

        lastPosition = boxTransform.position;
        StartCoroutine(DeltaPosition(lastPosition));
	} 
	
	
	public void ActivateBlockade (Vector3 mousePos)
	{
		cameraOffset = Camera.main.transform.position.z;
		cameraOffset = Mathf.Abs(cameraOffset);
			
		mousePos.z = cameraOffset;
			
 
        Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);

        collider.isTrigger = false;	
		collider.enabled = true;
       	transform.position = objectPos;
		
	}
	
	public void DeactivateBlockade()
	{
		collider.enabled = false;
	}

   IEnumerator DeltaPosition(Vector3 lastPos)
    {
       
        yield return new WaitForEndOfFrame();
        currentVel = (lastPos - boxTransform.position) / Time.deltaTime;
        Debug.Log(currentVel.magnitude);

        if (currentVel.magnitude > minVelocity)
        {
            collider.isTrigger = true;
        }
    }
}
