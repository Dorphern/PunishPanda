﻿using UnityEngine;
using System.Collections;


/*
 * Controls swiping
 *
 * Important note!
 *
 * Using multiple colliders on the panda causes RaycastAll to register  
 * Double collisions with the same panda! Thus, I am forced to perform an additional
 * Loop over the entire hits list to ensure that we perform a slap event on every panda 
 * only once!
 * 
 */

public class SwipeController : MonoBehaviour {
	
    private Vector3 endPos;
	private Vector3 thisPos;
    int count = 0;
	
	public void InitSwipe(Vector3 initPos)
	{
		
		initPos = TranslateScreenToWorldPos(initPos);
		endPos = initPos;
	}
	
	public void Swipe(Vector3 currPos)
    {
		currPos = TranslateScreenToWorldPos(currPos);
        Vector3 direction = endPos - currPos;
        Vector2 direction2D = new Vector2(direction.x, direction.y);
        float scrVecX = (direction2D.x * 10) / Screen.width;
        float scrVecY = (direction2D.y * 10) / Screen.height;

        Vector2 scrVec = new Vector2(scrVecX, scrVecY);

        
        float dist = scrVec.magnitude;
        float speed = dist / Time.deltaTime;
		
        Ray ray = new Ray(currPos, direction);

        Debug.DrawLine(currPos, endPos, Color.red, 3f);
		
		// reycast all for multiple panda hits
        /*RaycastHit hit;
        if (Physics.Raycast(ray, out hit, direction.magnitude + 0.01f, 1 << 8))
        {
			var collidable = hit.collider.GetComponent<Collidable>();
            if (collidable != null && collidable.type == CollidableTypes.Panda)
                count++;
                //Debug.Log("Hit Panda" + count);
            hit.collider.GetComponent<PandaAI>().PandaSlapped(-direction2D, speed);
        }/**/
		
		RaycastHit[] hits = Physics.RaycastAll(ray, direction.magnitude + 0.01f, 1 << 8);
		for(int i=0; i<hits.Length;i ++)
		{
			bool doubleCollisionFlag = false;
			// we need to check for multiple collisions of the same panda
			for(int j=0; j<i; j++)
			{
				if(hits[i].collider.gameObject.GetInstanceID() == hits[i].collider.gameObject.GetInstanceID())
				{
					doubleCollisionFlag = true;
					break;
				}
			}
			if(!doubleCollisionFlag)
			{
				var collidable = hits[i].collider.GetComponent<Collidable>();
				if (collidable != null && collidable.type == CollidableTypes.Panda)
		            count++;
		            Debug.Log("Hit Panda" + count);
		        hits[i].collider.GetComponent<PandaAI>().PandaSlapped(-direction2D, speed);
			}
		}/**/
		
        endPos = currPos;
             
    }
	
	Vector3 TranslateScreenToWorldPos(Vector3 mousePos)
	{
		float cameraOffset = Camera.main.transform.position.z; 
		mousePos.z = Mathf.Abs(cameraOffset);
		
	    Vector3 pos = Camera.main.ScreenToWorldPoint(mousePos);
        pos.z = 0;
        return pos;	
	}
}