using UnityEngine;
using System.Collections;

public class SlapEvent : MonoBehaviour
{

    public float directionSpeed;
    public float hitDirection;
    Transform myTransform;

    // Use this for initialization
    void Start()
    {
        directionSpeed = 2;
        myTransform = gameObject.transform;

    }

    // Update is called once per frame
    void FixedUpdate() {
       // rigidbody.velocity = new Vector3(directionSpeed, 0, 0);

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Slap(hitDirection);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Slap(-hitDirection);
        }

        myTransform.Translate((directionSpeed, 0, 0));

	}

    public void Slap(float hit)
    {
        if (hit < 0)
        {
            rigidbody.AddForce(new Vector3(hit, 0, 0), ForceMode.Impulse);
            directionSpeed = -2;
        }
        else if (hit > 0)
        {
            rigidbody.AddForce(new Vector3(hit, 0, 0), ForceMode.Impulse);
            directionSpeed = 2;
        }
    }
}

