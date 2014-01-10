using UnityEngine;
using System.Collections;

public class GUIObjectFetcher : MonoBehaviour {
    GameObject guiObject;

	void OnEnable()
	{
        if (guiObject == null)
        {
            guiObject = Object.Instantiate(InstanceFinder.LevelManager.CurrentLevel.GUIObject) as GameObject;
        }
        guiObject.transform.parent = gameObject.transform;
        guiObject.transform.localScale = Vector3.one;
        guiObject.transform.localPosition = Vector3.one;
	}
}
