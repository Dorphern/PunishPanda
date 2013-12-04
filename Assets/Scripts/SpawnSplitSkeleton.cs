using UnityEngine;
using System.Collections;

public class SpawnSplitSkeleton : MonoBehaviour {
	[SerializeField] protected GameObject splitSkeletonPanda;
	public float timeBeforeSpawn = 2f;
	// Use this for initialization
	void Start () {
		StartCoroutine(SpawnSkeleton());
	}
	
	IEnumerator SpawnSkeleton()
	{
		yield return new WaitForSeconds(timeBeforeSpawn);
		Instantiate(splitSkeletonPanda, transform.position + new Vector3(0, 0f, 0f), Quaternion.Euler(new Vector3(0f, 180f, 0f)));
		Destroy(this.gameObject);
	}
}
