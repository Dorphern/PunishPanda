using UnityEngine;
using System.Collections;

public class ObjectDestroyer : MonoBehaviour {

	void OnCollisionEnter(Collision collision)
	{
		Destroy(collision.gameObject);	
	}
}
