using UnityEngine;
using System.Collections;

public class FingerBlocking : MonoBehaviour {

	//cameraOffset determines where to place blockade on Z-axis
	//private float cameraOffset;
    //private Vector3 firstPos;
    //private Vector3 endPos;
    //private Vector2 direction;
    //private float speed;
	private Collider [] childColliders;
	
	
	void Start () {

		collider.enabled = false;
		childColliders = GetComponentsInChildren<Collider>();
	}
	
	public bool IsEnabled()
	{
		return collider.enabled;
	}
	
	public void ActivateBlockade (Vector3 mousePos)
	{

		float cameraOffset = Camera.main.transform.position.z; 
		mousePos.z = Mathf.Abs(cameraOffset);	
		
	    Vector3 pos = Camera.main.ScreenToWorldPoint(mousePos);
        pos.z = 0;
        transform.position = pos;

//        firstPos = transform.position;
//        if (collider.enabled == false)
//        {
//            endPos = firstPos;
//        }
//        Swipe();

        collider.enabled = true;
        ChildCollidersEnabled(true);
	}
	
	public void DeactivateBlockade()
	{
		collider.enabled = false;
        ChildCollidersEnabled(false);
		collider.isTrigger = false;
	}
	
	void ChildCollidersEnabled(bool val)
	{
		if(childColliders!=null)
		{
			for(int i=0; i<childColliders.Length; i++)
				childColliders[i].enabled = val;
		}
	}

//    void Swipe()
//    {
//        Vector3 direction = endPos - firstPos;
//        Vector2 direction2D = new Vector2(direction.x, direction.y);
//        Debug.Log("firstPos" + firstPos);
//        Debug.Log("endPos" + endPos);
//        float scrVecX = (direction2D.x * 10) / Screen.width;
//        float scrVecY = (direction2D.y * 10) / Screen.height;
//
//        Vector2 scrVec = new Vector2(scrVecX, scrVecY);
//
//        
//        float dist = scrVec.magnitude;
//        float speed = dist / Time.deltaTime;
//
//        //if (speed > 10f)
//        //{
//        //    return;
//        //}
//        Ray ray = new Ray(firstPos, direction);
//
//        Debug.DrawLine(firstPos, endPos, Color.red, 3f);
//		
//		// reycast all for multiple panda hits
//        RaycastHit hit;
//        if (Physics.Raycast(ray, out hit, direction.magnitude + 0.01f, 1 << 8))
//        {
//            if (hit.collider.GetComponent<Collidable>().type != null && hit.collider.GetComponent<Collidable>().type == CollidableTypes.Panda)
//                count++;
//                Debug.Log("Hit Panda" + count);
//            hit.collider.GetComponent<PandaAI>().PandaSlapped(-direction2D, speed);
//        }
//            endPos = firstPos;
//             
//    }
	
	
}
