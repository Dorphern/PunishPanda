//
// using Edelweiss.DecalSystem by
//   Andreas Suter (andy@edelweissinteractive.com)

using UnityEngine;
//using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Edelweiss.DecalSystem;

public enum HierarchyTransform
{
	Base,
	Parent,
	ParentParent,
	Root
}

public class BloodSplatter : MonoBehaviour {
	
	public static BloodSplatter Instance;
		// The prefab which contains the DS_Decals script with already set material and
		// uv rectangles.
	public GameObject decalsPrefab;
	public HierarchyTransform combinedMeshTransform = HierarchyTransform.Root;
	public bool mainMenuSlap = false;
	public float platformsLevelScale = 1f;
	
	public int maxSpaltCount = 100;
	public float fadingSpeed = 0.5f;
	
		// The raycast hits a collider at a certain position. This value indicated how far we need to
		// go back from that hit point along the ray of the raycast to place the new decal projector. Set
		// this value to 0.0f to see why this is needed.
	public float decalProjectorOffset = 0.5f;
		// The size of new decal projectors.
	//public Vector3 decalProjectorScale = new Vector3 (0.2f, 2.0f, 0.2f);
	
	public float slapDelay = 0.1f;
	public float decalOffsetAngle = 45f;
	
	public float slapMinScale = 1.8f;
	public float slapMaxScale = 5f;
	
	public float hitMinScale = 1.8f;
	public float hitMaxScale = 5f;
	
	public int slapUVmin;
	public int slapUVmax;
	public int hitUVmin;
	public int hitUVmax;
	
	private bool levelHasPlatforms = false;
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
	private int slapCount = 0;
	
		// Move on to the next uv rectangle index.
	private void NextSlapUV () 
	{
		m_UVRectangleIndex = Random.Range(slapUVmin, slapUVmax + 1);
		
//		m_UVRectangleIndex = m_UVRectangleIndex + 1;
//		if (m_UVRectangleIndex >= m_Decals.CurrentUvRectangles.Length) 
//		{
//			m_UVRectangleIndex = 0;
//		}
	}
	
	private void NextHitUV () 
	{
		m_UVRectangleIndex = Random.Range(hitUVmin, hitUVmax + 1);
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
		if(InstanceFinder.LevelManager.CurrentLevelIndex != -1)
			levelHasPlatforms = InstanceFinder.LevelManager.CurrentLevel.hasPlatforms;
		else
			levelHasPlatforms= true;
		if(levelHasPlatforms)
		{
			slapMaxScale = platformsLevelScale;
			slapMinScale = platformsLevelScale;
		}
		
			// Instantiate the prefab and get its decals instance.
		GameObject l_Instance = Instantiate (decalsPrefab) as GameObject;
		m_Decals = l_Instance.GetComponentInChildren <DS_Decals> ();
		m_Decals.UseVertexColors = true;
		
		if (m_Decals == null) 
		{
			Debug.LogError ("The 'decalsPrefab' does not contain a 'DS_Decals' instance!");
		} 
		else 
		{
				// Create the decals mesh (intermediate mesh data) for our decals instance.
				// Further we need a decals mesh cutter instance and the world to decals matrix.
			m_DecalsMesh = new DecalsMesh (m_Decals);
			m_DecalsMesh.PreserveVertexColorArrays = true;
			m_DecalsMeshCutter = new DecalsMeshCutter ();
		}
		
		layerMask = ( 1 << LayerMask.NameToLayer("Panda") );
		layerMask |= ( 1 << LayerMask.NameToLayer("FingerBlockade") );
		layerMask |= ( 1 << LayerMask.NameToLayer("Ignore Raycast") );
		layerMask |= ( 1 << LayerMask.NameToLayer("PandaBodyParts") );
		layerMask = ~layerMask;
		
		// We instantiate a blood splatter in order to avoid a huge spike in performance caused by the first slap
		ProjectHit(transform.position ,Vector2.zero);
		
		if(m_DecalProjectors.Count > 0)
			ClearProjectors();
		else
		{
			Debug.LogException(new System.Exception("Failed to project a texture from " + gameObject.name + " because there was no mesh to project onto!" +
				"\n<b> The object's Z axis must be facing an object within " + rayDistance + " meters! </b>"
				+ " \n The mesh needs to have a collider!"
				+ " \n The combineChildren script need to execue before the bloodSplatter script!"), this.gameObject);
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
		float scale = Random.Range(hitMinScale, hitMaxScale);
		
		rayStart.y -= 0.5f;
		
		projectionDirection.x = slapDirection.x;
		projectionDirection.y = slapDirection.y;
		projectionDirection.z = 1f;
		
		float angle = GetProjectionAngle();
		
		NextHitUV();
		ProjectBlood(rayStart, angle, scale, slapForce);
	}
	
	public void ProjectSlap(Vector3 rayStart ,Vector2 slapDirection, float slapForce = 2)
	{
		ProjectWithDelay(rayStart, slapDirection, slapForce, false);
		slapCount++;
	}
	
	public void ProjectFloorHit(Vector3 rayStart ,Vector2 slapDirection, float slapForce = 2)
	{
		ProjectWithDelay(rayStart, slapDirection, slapForce, true);
		
	}
	
	void ProjectWithDelay(Vector3 rayStart ,Vector2 slapDirection, float slapForce, bool floorHit)
	{
		if(mainMenuSlap)
		{
			if(slapDirection.x > 0f)
				rayStart.x = rayStart.x - 0.5f + slapForce * 3f;
		}
		
		Vector3 rotatedDirection;
		rotatedDirection = Quaternion.AngleAxis( Random.Range(- decalOffsetAngle, decalOffsetAngle), Vector3.forward) * new Vector3(slapDirection.x, slapDirection.y) ;
		
		// set the angle of the splat to be the same in both XY and XZ planes
		projectionDirection.x = rotatedDirection.x;
		projectionDirection.y = rotatedDirection.y;
		if(mainMenuSlap)
		{
			projectionDirection.z = 1f;
		}
		else
		{
			if(projectionDirection.y < 0f)
				projectionDirection.z = Mathf.Abs(projectionDirection.x);
			else
				projectionDirection.z = 1f;
		}
		
		float angle = GetProjectionAngle();
		
		float scale = Random.Range(slapMinScale, slapMaxScale);
		
		// First 4 slaps will be huge
		if(levelHasPlatforms == false)
		{
			if(slapCount < 5f)
			{
				scale = slapMaxScale;	
			}
		}
		
		float angleToFloor = Mathf.Abs(angle - 90f);
		if(angleToFloor < 60f && angleToFloor > 30f)
		{
			if(levelHasPlatforms)
			{
				if(scale > 0.65f * slapMaxScale)
					scale = 0.65f * slapMaxScale;
			}
			else
			{
				if(scale > 0.3f * slapMaxScale)
					scale = 0.3f * slapMaxScale;
			}
		}
		
		if(angleToFloor < 30f || floorHit)
		{
			if(levelHasPlatforms)
				scale = slapMaxScale * 0.86f;	
			else
				scale = slapMaxScale * 0.56f;	
		}
		
		
		if(floorHit == false)
		{
			NextSlapUV ();
		}
		else
		{
			NextHitUV();	
		}
		ProjectBlood(rayStart, angle, scale, slapForce);
	}
	
	private float GetProjectionAngle()
	{
		Vector2 projectionDirection2D = new Vector2(projectionDirection.x, projectionDirection.y).normalized;
			
		float angle = Vector2.Angle(Vector2.right, projectionDirection2D);
		
		if(Vector2.Dot(Vector2.up, projectionDirection2D) > 0f)
		{
			angle = 360 - angle;
		}
		
		return angle;
	}
	
	public void ProjectBlood(Vector3 rayStart , float angle, float scale = 1.8f, float slapForce = 2f)
	{	
		
		//Debug.DrawLine(rayStart, rayStart + projectionDirection * rayDistance, Color.blue, 100f);
		if(Physics.Raycast (rayStart + rayStartYOffset, projectionDirection, out hitInfo, rayDistance, layerMask) )
		{
			// Collider hit.
			RecycleDecalProjectors();	
		
			Quaternion projectorRotation = ProjectorRotationUtility.ProjectorRotation ( projectionDirection, Vector3.up);
			
			//Debug.DrawRay(hitInfo.point,  - projectionDirection, Color.green, 2000f);
			
			float angleToFloor = Mathf.Abs(angle - 90f);
			if(angleToFloor < 30f)
				decalProjectorOffset = scale * 0.75f;
			else
				decalProjectorOffset = scale * 0.5f;
				
			Vector3 projectorPosition = hitInfo.point - (decalProjectorOffset * projectionDirection.normalized);
			
			Quaternion slapRotation = Quaternion.Euler (0.0f, angle , 0.0f);
			projectorRotation = projectorRotation * slapRotation;	
			
			ProjectDecal(projectorPosition, projectorRotation, scale);	
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
			StartCoroutine(FadeOutDecal());
		}	
	}
	
	private void ProjectDecal(Vector3 l_ProjectorPosition, Quaternion l_ProjectorRotation, float scale)
	{
		// We hit a collider. Next we have to find the mesh that belongs to the collider.
				// That step depends on how you set up your mesh filters and collider relative to
				// each other in the game objects. It is important to have a consistent way in order
				// to have a simpler implementation.
		
			Transform combinedMeshTransform = GetCombinedMeshTransform(hitInfo.transform);	
		
			MeshFilter l_MeshFilter = combinedMeshTransform.GetComponent <MeshFilter> ();
			
			if (l_MeshFilter != null) 
			{
				Mesh l_Mesh = null; 
				if (l_MeshFilter != null) 
				{
					// Otherwise take the data from the shared mesh.
					l_Mesh = l_MeshFilter.sharedMesh;
				}
				
				if (l_Mesh != null) 
				{
						// Create the decal projector.
					DecalProjector l_DecalProjector = new DecalProjector (l_ProjectorPosition, l_ProjectorRotation, 
						new Vector3(scale, scale, scale), cullingAngle, meshOffset, m_UVRectangleIndex, m_UVRectangleIndex, Color.white, 0.0f);
					
					
						// Add the projector to our list and the decals mesh, such that both are
						// synchronized. All the mesh data that is now added to the decals mesh
						// will belong to this projector.
					m_DecalProjectors.Add (l_DecalProjector);
					m_DecalsMesh.AddProjector (l_DecalProjector);
					
						// Get the required matrices.
					Matrix4x4 l_WorldToMeshMatrix = combinedMeshTransform.renderer.transform.worldToLocalMatrix;
					Matrix4x4 l_MeshToWorldMatrix = combinedMeshTransform.renderer.transform.localToWorldMatrix;
					
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
	
	private IEnumerator FadeOutDecal()
	{
		DecalProjector l_DecalProjector = m_DecalProjectors [0];
		m_DecalProjectors.RemoveAt (0);
		
		while(l_DecalProjector.vertexColor.a >= 0)
		{
			l_DecalProjector.vertexColor.a -= Time.deltaTime * fadingSpeed;
			m_DecalsMesh.UpdateVertexColors (l_DecalProjector);
			m_Decals.UpdateVertexColors (m_DecalsMesh);
			yield return null;
		}
		
		m_DecalsMesh.RemoveProjector (l_DecalProjector);	
		m_Decals.UpdateDecalsMeshes (m_DecalsMesh);
		yield return null;
	}
	
	private Transform GetCombinedMeshTransform(Transform baseTransform)
	{
		switch(combinedMeshTransform)
		{
			case HierarchyTransform.Base:	
				return baseTransform;
			case HierarchyTransform.Root:	
				return baseTransform.root;
			case HierarchyTransform.Parent:
				return baseTransform.parent;
			case HierarchyTransform.ParentParent:
				return baseTransform.parent.parent;
			default:
				return baseTransform.root;
		}
	}
}
