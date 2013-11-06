using UnityEngine;
using System.Collections;

public class FingerBlocking : MonoBehaviour {

	//cameraOffset determines where to place blockade on Z-axis
	private float cameraOffset;
    private float minSpeed = 0.03f;
    private float maxSpeed = 1;
    private Vector3 lastPosition;
    private Vector2 direction;
    private float speed;
	
	void Start () {

		collider.enabled = false;

	}
	
	public void ActivateBlockade (Vector3 mousePos)
	{
        if(collider.enabled == true)
        {
            StartCoroutine(DeltaPosition(lastPosition));
        }

		collider.enabled = true;        
        
		cameraOffset = Camera.main.transform.position.z; 
		mousePos.z = Mathf.Abs(cameraOffset);	
		
	    Vector3 pos = Camera.main.ScreenToWorldPoint(mousePos);
        pos.z = 0;
        transform.position = pos;
	}
	
	public void DeactivateBlockade()
	{
		collider.enabled = false;
        collider.isTrigger = false;
		//DestroyImmediate(this.gameObject);
	}

   IEnumerator DeltaPosition(Vector3 lastPos)
    {
        
        yield return new WaitForEndOfFrame();

        Vector3 delta = transform.position - lastPos;
        direction = new Vector2(delta.x, delta.y);
        float dist = delta.sqrMagnitude;
        
        float distPrScreenWidth = dist / Screen.width;

        float speed = distPrScreenWidth / Time.deltaTime;
        
        //if(speed > maxSpeed)
        //{
        //    DebugStreamer.message = "Break";
        //    yield break;
        //}
            
        delta.z = 0;        
        float angle = Vector3.Angle(Vector3.right, delta);
        
        lastPosition = transform.position;
        
        if (speed > minSpeed)
        {
            DebugStreamer.message = speed.ToString();
            collider.isTrigger = true;
        }
        else
        {
            collider.isTrigger = false;
        }
    }
    void OnTriggerEnter(Collider c)
    {
        if (c.GetComponent<Collidable>().type != null && c.GetComponent<Collidable>().type == CollidableTypes.Panda)
           c.gameObject.renderer.material.color = Color.red;
        c.GetComponent<PandaAI>().PandaSlapped(direction, speed);
    }
}
