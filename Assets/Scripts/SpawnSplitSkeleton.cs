﻿using UnityEngine;
using System.Collections;

public class SpawnSplitSkeleton : MonoBehaviour {
	[SerializeField] protected GameObject splitSkeletonPanda;
	public float timeBeforeSpawn = 2f;
	public float teethDisableTime = 1.6f;
	public float ashSilhouetteTime = 0.8f;
	public Renderer teethOutline;
	public Renderer ashSilhouette;
	// Use this for initialization
	void Start () {
		StartCoroutine(SpawnSkeleton());
	}
	
	IEnumerator SpawnSkeleton()
	{
		yield return new WaitForSeconds(ashSilhouetteTime);
		ashSilhouette.enabled = true;
		yield return new WaitForSeconds(teethDisableTime - ashSilhouetteTime);
		teethOutline.enabled = false;
		yield return new WaitForSeconds(timeBeforeSpawn - teethDisableTime);
		//ashSilhouette.enabled = false;
		Instantiate(splitSkeletonPanda, transform.position + new Vector3(0, 0f, 0f), Quaternion.Euler(new Vector3(0f, 180f, 0f)));
		Destroy(this.gameObject);
	}
}
