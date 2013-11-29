using UnityEngine;
using System.Collections;

public enum BladeDirection {
	None,
	Clockwise,
	CounterClockwise
}

public class PandaDismemberment : MonoBehaviour 
{

	
	public float minHorizontalForce = -5f;
	public float maxHorizontalForce = 5f;
	public float minVerticalForce = 1f;
	public float maxVerticalForce = 3f;
	public float minTorque = 1f;
	public float maxTorque = 3f;
	
	public BladeDirection SpinDirection;
	public int splatCount = 3;
	
	private Vector3 explosionForce = Vector3.zero;
	private Vector2 splatDirection = Vector2.right;

	public void Initialize(BladeDirection spinDirection)
	{
		SpinDirection = spinDirection;
	}
		
	void Start ()
	{
		for(int i = 0; i < transform.childCount; i++)
		{
			explosionForce.x = Random.Range(minHorizontalForce, maxHorizontalForce);
			if(SpinDirection == BladeDirection.Clockwise)
			{
				explosionForce.x *= - 1f;
			}
			else if(SpinDirection == BladeDirection.CounterClockwise)
			{
				explosionForce.x *= 1f;
			}
			else
			{
				explosionForce.x *= Random.Range(0, 2) * 2 - 1;
			}
			explosionForce.y = Random.Range(minVerticalForce, maxVerticalForce);
			explosionForce.z = - Random.Range(minVerticalForce, maxVerticalForce);
			
        	transform.GetChild(i).GetComponentInChildren<Rigidbody>().AddForce(explosionForce, ForceMode.Impulse);
			
			explosionForce.x = Random.Range(minTorque, maxTorque);
			explosionForce.y = Random.Range(minTorque, maxTorque);
			explosionForce.z = Random.Range(minTorque, maxTorque);
			transform.GetChild(i).GetComponentInChildren<Rigidbody>().AddTorque(explosionForce, ForceMode.Impulse);	
		}
		
		for(int i = 0; i < splatCount; i++)
		{
			float angle =  Random.Range(-150f, 150f);
			angle += (angle < 0) ? -15f : 15f;
			splatDirection =  new Vector2(Mathf.Sin(Mathf.Deg2Rad * angle), Mathf.Cos(Mathf.Deg2Rad * angle));
		
			BloodSplatter.Instance.ProjectHit(transform.position, splatDirection.normalized);
		}
	}
}
