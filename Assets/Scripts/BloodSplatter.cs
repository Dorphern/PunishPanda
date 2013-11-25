//
// Author:
//   Andreas Suter (andy@edelweissinteractive.com)
//
// Copyright (C) 2012-2013 Edelweiss Interactive (http://www.edelweissinteractive.com)
//

using UnityEngine;
//using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Edelweiss.DecalSystem;

public class BloodSplatter : MonoBehaviour {
	
	public static BloodSplatter Instance;
		// The prefab which contains the DS_Decals script with already set material and
		// uv rectangles.
	public GameObject decalsPrefab;
	
	public int maxSpaltCount = 100;
	
		// The raycast hits a collider at a certain position. This value indicated how far we need to
		// go back from that hit point along the ray of the raycast to place the new decal projector. Set
		// this value to 0.0f to see why this is needed.
	public float decalProjectorOffset = 0.5f;
		// The size of new decal projectors.
	public Vector3 decalProjectorScale = new Vector3 (0.2f, 2.0f, 0.2f);
	
	public float slapDelay = 0.1f;
	
	public int slapUVmin;
	public int slapUVmax;
	public int hitUVmin;
	public int hitUVmax;
	
		// The reference to the instantiated prefab's DS_Decals instance.
	private DS_Decals m_Decals;
	
		// All the projectors that were created at runtime.
	[System.NonSerializedAttribute]
	public List <DecalProjector> m_DecalProjectors = new List <DecalProjector> ();
	
		// Intermediate mesh data. Mesh data is added to that one for a specific projector
		// in order to perform the cutting.
	private DecalsMesh m_DecalsMesh;
	private DecalsMeshCutter m_DecalsMeshCutter;
	
	private float cullingAngle = 90.0f;
	private float meshOffset = 0.003f;
	
		// We iterate through all the defined uv rectangles. This one inicates which index we are using at
		// the moment.
	private int m_UVRectangleIndex = 0;
	
	private RaycastHit hitInfo;
	private int layerMask;
	// 3D vector controlling the direction of the splat
	private Vector3 projectionDirection = Vector3.right;
	
	public Vector3 rayStartYOffset = new Vector3(0,1,0);
	private float rayDistance = 10f;
	
		// Move on to the next uv rectangle index.
	private void NextSlapUV () 
	{
		m_UVRectangleIndex = Random.Range(slapUVmin, slapUVmax);
		
//		m_UVRectangleIndex = m_UVRectangleIndex + 1;
//		if (m_UVRectangleIndex >= m_Decals.CurrentUvRectangles.Length) 
//		{
//			m_UVRectangleIndex = 0;
//		}
	}
	
	private void NextHitUV () 
	{
		m_UVRectangleIndex = Random.Range(hitUVmin, hitUVmax);
	}
	
	private void Awake()
	{
		if(Instance != null && Instance != this)
        {
            // If that is the case, we destroy other instances
            Destroy(gameObject);
        }
 
        // Here we save our singleton instance
        Instance = this;	
	}
	
	private void Start () 
	{
			// Instantiate the prefab and get its decals instance.
		GameObject l_Instance = Instantiate (decalsPrefab) as GameObject;
		m_Decals = l_Instance.GetComponentInChildren <DS_Decals> ();
		
		if (m_Decals == null) 
		{
			Debug.LogError ("The 'decalsPrefab' does not contain a 'DS_Decals' instance!");
		} 
		else 
		{
				// Create the decals mesh (intermediate mesh data) for our decals instance.
				// Further we need a decals mesh cutter instance and the world to decals matrix.
			m_DecalsMesh = new DecalsMesh (m_Decals);
			m_DecalsMeshCutter = new DecalsMeshCutter ();
		}
		
		layerMask = ( 1 << LayerMask.NameToLayer("Panda") );
		layerMask |= ( 1 << LayerMask.NameToLayer("FingerBlockade") );
		layerMask |= ( 1 << LayerMask.NameToLayer("Ignore Raycast") );
		layerMask |= ( 1 << LayerMask.NameToLayer("PandaBodyParts") );
		layerMask = ~layerMask;
		
		// We instantiate a blood splatter in order to avoid a huge spike in performance caused by the first slap
		ProjectBlood(transform.position ,Vector2.right, 2f);
		if(m_DecalProjectors.Count > 0)
			ClearProjectors();
		else
		{
			Debug.LogException(new System.Exception("Failed to project a texture from " + gameObject.name + " because there was no mesh to project onto!" +
				"\n<b> The object's Z axis must be facing an object within " + rayDistance + " meters! </b>"), this.gameObject);
		}
	}
	
	private void Update () 
	{
		if (Input.GetKeyDown (KeyCode.C)) 
		{
			ClearProjectors();
		}
	}
		
	private void ClearProjectors()
	{
		// Remove all projectors.
		while (m_DecalProjectors.Count > 0) 
		{
			DecalProjector l_Projector = m_DecalProjectors [m_DecalProjectors.Count - 1];
			m_DecalsMesh.RemoveProjector (l_Projector);
			m_DecalProjectors.RemoveAt (m_DecalProjectors.Count - 1);
		}
		m_Decals.UpdateDecalsMeshes (m_DecalsMesh);		
	}
	
	public void ProjectHit(Vector3 rayStart ,Vector2 slapDirection, float slapForce = 2)
	{
		ProjectBlood(rayStart, slapDirection, slapForce);
		NextHitUV();
	}
	
	public void ProjectSlap(Vector3 rayStart ,Vector2 slapDirection, float slapForce = 2)
	{
		StartCoroutine(ProjectWithDelay(rayStart, slapDirection, slapForce));
		//Util.Draw.Arrow(rayStart, rayStart + rotatedDirection * 1f, Color.blue, 100f);
	}
	
	IEnumerator ProjectWithDelay(Vector3 rayStart ,Vector2 slapDirection, float slapForce)
	{
		ProjectBlood(rayStart, slapDirection, slapForce);
		NextSlapUV ();
		
		yield return new WaitForSeconds(slapDelay);
		
		Vector3 rotatedDirection;		
		rotatedDirection = Quaternion.AngleAxis( -45, Vector3.forward) * new Vector3(slapDirection.x, slapDirection.y) ;
		ProjectBlood(rayStart, rotatedDirection, slapForce);
		NextSlapUV ();
		
		yield return new WaitForSeconds(slapDelay);
		
		rotatedDirection = Quaternion.AngleAxis( 45, Vector3.forward) * new Vector3(slapDirection.x, slapDirection.y) ;
		ProjectBlood(rayStart, rotatedDirection, slapForce);
		NextSlapUV ();
	}
	
	public void ProjectBlood(Vector3 rayStart ,Vector2 slapDirection, float slapForce = 2f)
	{
		// set the angle of the splat to be the same in both XY and XZ planes
		projectionDirection.x = slapDirection.x;
		projectionDirection.y = slapDirection.y;
		if(projectionDirection.y < 0f)
			projectionDirection.z = Mathf.Abs(projectionDirection.x);
		else
			projectionDirection.z = 1f;
		
		if(Physics.Raycast (rayStart + rayStartYOffset, projectionDirection, out hitInfo, rayDistance, layerMask) )
		{
			// Collider hit.
			RecycleDecalProjectors();
			
			
			Vector2 projectionDirection2D = new Vector2(projectionDirection.x, projectionDirection.y).normalized;
			
			float angle = Vector2.Angle(Vector2.right, projectionDirection2D);
			
			if(Vector2.Dot(Vector2.up, projectionDirection2D) > 0f)
			{
				angle = 360 - angle;
			}
		
			Quaternion projectorRotation = ProjectorRotationUtility.ProjectorRotation ( projectionDirection, Vector3.up);
			
			
			//Vector3 upVector = l_ProjectorRotation * Vector3.up;
			Vector3 forwardVector = projectorRotation * hitInfo.normal;
			
			//Debug.DrawRay(hitInfo.point, upVector, Color.red, 2000f);
			//Debug.DrawRay(hitInfo.point,  - projectionDirection, Color.green, 2000f);
			
//			if(projectionDirection.y < - 0.7f)
//			{
//				Vector3 hitVector = hitInfo.point - new Vector3(hitInfo.point.x, edge.position.y, edge.position.z);	
//				//Debug.DrawLine(hitInfo.point, new Vector3(hitInfo.point.x, edge.position.y, edge.position.z), Color.white, 2000f);
//				
//				decalProjectorOffset = 1.9f - Vector3.Dot(hitVector, hitInfo.normal == - Vector3.forward ? -forwardVector : forwardVector);
//				//textMesh.GetComponent<TextMesh>().text = projectionDirection.ToString();
//			}
//			else
//			{
//				decalProjectorOffset = 1f;	
//			}
			Vector3 projectorPosition = hitInfo.point - (decalProjectorOffset * projectionDirection.normalized);
			
			//Util.Draw.Arrow(projectorPosition, projectorPosition + projectionDirection * 3f, Color.green, 1000f);
			
			
			Quaternion slapRotation = Quaternion.Euler (0.0f, angle , 0.0f);
			projectorRotation = projectorRotation * slapRotation;			
			ProjectDecal(projectorPosition, projectorRotation);	
		}
	}
	
	private void RecycleDecalProjectors()
	{
		// Make sure there are not too many projectors.
		if (m_DecalProjectors.Count >= maxSpaltCount) 
		{
				// If there are more than maxSpatCount projectors, we remove the first one from
				// our list and certainly from the decals mesh (the intermediate mesh
				// format). All the mesh data that belongs to this projector will
				// be removed.
			DecalProjector l_DecalProjector = m_DecalProjectors [0];
			m_DecalProjectors.RemoveAt (0);
			m_DecalsMesh.RemoveProjector (l_DecalProjector);
		}	
	}
	
	private void ProjectDecal(Vector3 l_ProjectorPosition, Quaternion l_ProjectorRotation)
	{
		// We hit a collider. Next we have to find the mesh that belongs to the collider.
				// That step depends on how you set up your mesh filters and collider relative to
				// each other in the game objects. It is important to have a consistent way in order
				// to have a simpler implementation.
			
			MeshCollider l_MeshCollider = hitInfo.transform.root.GetComponent <MeshCollider> ();
			MeshFilter l_MeshFilter = hitInfo.transform.root.GetComponent <MeshFilter> ();
			
			if (l_MeshCollider != null || l_MeshFilter != null) 
			{
				Mesh l_Mesh = null;
				if (l_MeshCollider != null) 
				{
					// Mesh collider was hit. Just use the mesh data from that one.
					l_Mesh = l_MeshCollider.sharedMesh;
				} 
				else if (l_MeshFilter != null) 
				{
					// Otherwise take the data from the shared mesh.
					l_Mesh = l_MeshFilter.sharedMesh;
				}
				
				if (l_Mesh != null) 
				{
						// Create the decal projector.
					DecalProjector l_DecalProjector = new DecalProjector (l_ProjectorPosition, l_ProjectorRotation, decalProjectorScale, cullingAngle, meshOffset, m_UVRectangleIndex, m_UVRectangleIndex);
					
					
						// Add the projector to our list and the decals mesh, such that both are
						// synchronized. All the mesh data that is now added to the decals mesh
						// will belong to this projector.
					m_DecalProjectors.Add (l_DecalProjector);
					m_DecalsMesh.AddProjector (l_DecalProjector);
					
						// Get the required matrices.
					Matrix4x4 l_WorldToMeshMatrix = hitInfo.transform.root.renderer.transform.worldToLocalMatrix;
					Matrix4x4 l_MeshToWorldMatrix = hitInfo.transform.root.renderer.transform.localToWorldMatrix;
					
						// Add the mesh data to the decals mesh, cut and offset it before we pass it
						// to the decals instance to be displayed.
					m_DecalsMesh.Add (l_Mesh, l_WorldToMeshMatrix, l_MeshToWorldMatrix);						
					m_DecalsMeshCutter.CutDecalsPlanes (m_DecalsMesh);
					m_DecalsMesh.OffsetActiveProjectorVertices ();
					m_Decals.UpdateDecalsMeshes (m_DecalsMesh);
					
						// For the next hit, use a new uv rectangle. Usually, you would select the uv rectangle
						// based on the surface you have hit.
				}
			}	
	}
}
