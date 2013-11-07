using UnityEngine;
using System.Collections;

public class BloodInstantiation : MonoBehaviour {
	
	public int count = 50;
	
	private GameObject newBloodProjector;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	[ContextMenu("InstantiateBlood")]
	public void InstantiateBlood()
	{
		for(int i = 0; i < count; i++)
		{
			newBloodProjector = new GameObject("Projector" + i);
			newBloodProjector.AddComponent<DS_DecalProjector>();
			//newBloodProjector.GetComponent<DS_DecalProjector>().GetDecals().dec
			newBloodProjector.transform.parent = this.transform;
			newBloodProjector.transform.localPosition = new Vector3(i/2, 0, i%7);
		}
	}
	
	[ContextMenu("ClearProjectors")]
	public void ClearProjectors()
	{
		DS_DecalProjector[] projectors = this.GetComponentsInChildren<DS_DecalProjector>();
		
		for(int i = 0; i < projectors.Length; i++)
		{
			DestroyImmediate(projectors[i].gameObject);
		}
	}
}
