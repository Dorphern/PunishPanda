//----------------------------------------------
//            MeshBaker
// Copyright © 2011-2012 Ian Deane
//----------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Specialized;
using System;
using System.Collections.Generic;
using System.Text;

#if UNITY_EDITOR
	using UnityEditor;
#endif 

public delegate void ProgressUpdateDelegate(string msg, float progress);

/// <summary>
/// Maps a list of source materials to a combined material. Included in MB2_TextureBakeResults
/// </summary>
[System.Serializable]
public class MB_MultiMaterial{
	public Material combinedMaterial;
	public List<Material> sourceMaterials = new List<Material>();
}

public enum MB_RenderType{
	meshRenderer,
	skinnedMeshRenderer
}

public enum MB2_OutputOptions{
	bakeIntoSceneObject,
	bakeMeshAssetsInPlace,
	bakeIntoPrefab
}

public enum MB2_LightmapOptions{
	preserve_current_lightmapping,
	ignore_UV2,
	copy_UV2_unchanged,
	generate_new_UV2_layout
}

/// <summary>
/// Abstract root of the mesh combining classes
/// </summary>
public abstract class MB2_MeshBakerCommon : MB2_MeshBakerRoot {	
	
	public List<GameObject> objsToMesh;	
	public bool useObjsToMeshFromTexBaker = true;
	
	[HideInInspector] public GameObject resultPrefab;
	[HideInInspector] public GameObject resultSceneObject;
	
	[HideInInspector] public MB_RenderType renderType = MB_RenderType.meshRenderer;
	[HideInInspector] public MB2_OutputOptions outputOption = MB2_OutputOptions.bakeIntoSceneObject;
	
	[HideInInspector] public MB2_LightmapOptions lightmapOption = MB2_LightmapOptions.ignore_UV2; //todo can't change after we have started adding
	
	[HideInInspector] public bool doNorm = true;
	[HideInInspector] public bool doTan = true;
	[HideInInspector] public bool doCol = false;	
	[HideInInspector] public bool doUV = true;
	[HideInInspector] public bool doUV1 = false;
		
	public override List<GameObject> GetObjectsToCombine(){
		if (objsToMesh == null) objsToMesh = new List<GameObject>();
		return objsToMesh;
	}

	public void EnableDisableSourceObjectRenderers(bool show){
		for (int i = 0; i < objsToMesh.Count; i++){
			GameObject go = objsToMesh[i];
			if (go != null){
				Renderer mr = MB_Utility.GetRenderer(go);
				if (mr != null){
					mr.enabled = show;
				}
			}
		}
	}

/// <summary>
///  Clears the meshs and mesh related data but does not destroy it.
/// </summary>
	public abstract void ClearMesh();

/// <summary>
///  Clears and desroys the mesh. Clears mesh related data.
/// </summary>	
	public abstract void DestroyMesh();

	public Mesh AddDeleteGameObjects(GameObject[] gos, GameObject[] deleteGOs){
		if (textureBakeResults == null){
			Debug.LogError("Material Bake Results is not set.");
			return null;
		}
		return AddDeleteGameObjects(gos,deleteGOs,true,textureBakeResults.fixOutOfBoundsUVs);
	}
	
	public Mesh AddDeleteGameObjects(GameObject[] gos, GameObject[] deleteGOs, bool disableRendererInSource){		
		if (textureBakeResults == null){
			Debug.LogError("Material Bake Results is not set.");
			return null;
		}
		return AddDeleteGameObjects(gos,deleteGOs,disableRendererInSource,textureBakeResults.fixOutOfBoundsUVs);
	}

/// <summary>
/// Adds and deletes objects from the combined mesh. gos and deleteGOs can be null. 
/// You need to call Apply or ApplyAll to see the changes. 
/// objects in gos must not include objects already in the combined mesh.
/// objects in gos and deleteGOs must be the game objects with a Renderer component
/// This method is slow, so should be called as infrequently as possible.
/// </summary>
/// <returns>
/// The first generated combined mesh
/// </returns>
/// <param name='gos'>
/// gos. Array of objects to add to the combined mesh. Array can be null. Must not include objects
/// already in the combined mesh. Array must contain game objects with a render component.
/// </param>
/// <param name='deleteGOs'>
/// deleteGOs. Array of objects to delete from the combined mesh. Array can be null.
/// </param>
/// <param name='disableRendererInSource'>
/// Disable renderer component on objects in gos after they have been added to the combined mesh.
/// </param>
/// <param name='fixOutOfBoundUVs'>
/// Whether to fix out of bounds UVs in meshes as they are being added. This paramater should be set to the same as the combined material.
/// </param>
/// </summary>
	public abstract Mesh AddDeleteGameObjects(GameObject[] gos, GameObject[] deleteGOs, bool disableRendererInSource, bool fixOutOfBoundUVs);

/// <summary>
/// Returns true if go is in the combined mesh.	
/// </summary>	
	public abstract bool CombinedMeshContains(GameObject go);

/// <summary>
/// Updates vertices, normals and tangents from gos of an object in the combined mesh. Use this if an object has moved, rotated, been scaled or been deformed. It can’t be used if the number of vertices has changed. You need to call Apply to see the changes.	
/// </summary>
	public abstract void UpdateGameObjects(GameObject[] gos, bool recalcBounds = true);

/// <summary>
/// Apply changes to the mesh. All channels set in this instance will be set in the combined mesh.
/// </summary>
	public abstract void Apply();

/// <summary>	
/// Applys the changes to flagged properties of the mesh. This method is slow, and should only be called once per frame. The speed is directly proportional to the number of flags that are true. Only apply necessary properties.	
/// </summary>
	public abstract void Apply(bool triangles,
					  bool vertices,
					  bool normals,
					  bool tangents,
					  bool uvs,
					  bool colors,
					  bool uv1,
					  bool uv2,
					  bool bones=false);
	
	public abstract void SaveMeshsToAssetDatabase(string folderPath,string newFileNameBase);
	
	public abstract void RebuildPrefab();
}
