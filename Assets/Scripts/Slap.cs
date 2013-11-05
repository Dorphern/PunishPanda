using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Slap : MonoBehaviour {

    private float minDist = 200f;
    private float maxSpeed = 1;
    private Vector3 lastPosition;
    private Vector2 direction2D;
    private float speed;
    public float radius = 1f;
    public GameObject testPoint;
    Quaternion testPointQua;



	// Use this for initialization
	void Start () {
	
        List<InputPointsList> inputPointsList = new List<InputPointsList>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void DeltaPosition(Vector3 lastPos)
    {




        //yield return new WaitForEndOfFrame();
        Vector3 direction = transform.position - lastPos;
        direction2D = new Vector2(direction.x, direction.y);

        float scrVecX = (direction2D.x * 1000) / Screen.width;
        float scrVecY = (direction2D.y * 1000) / Screen.height;

        Vector2 scrVec = new Vector2(scrVecX, scrVecY);

        float dist = scrVec.magnitude;

        float speed = dist / Time.deltaTime;

        if (dist > Screen.width / minDist)
        {

            // Instantiate(testPoint, lastPos, testPointQua);

            Debug.DrawLine(transform.position, lastPos, Color.blue, 3f, false);

            Ray ray = new Ray(lastPos, transform.position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, dist + 0.01f, 1 << 8))
            {

                if (hit.collider.GetComponent<Collidable>().type != null && hit.collider.GetComponent<Collidable>().type == CollidableTypes.Panda)
                {
                    Debug.DrawLine(transform.position, lastPos, Color.green, 3f, false);

                    Debug.Log("Hit Panda" + dist);
                    hit.collider.renderer.material.color = Color.red;
                    hit.collider.GetComponent<PandaAI>().PandaSlapped(direction2D, speed);
                }
            }
            lastPosition = transform.position;
        }




        //Debug.Log("Ray Length  " + direction2D.magnitude);

    }


}
