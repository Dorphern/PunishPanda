using UnityEngine;
using System.Collections;

public class LevelMenu : MonoBehaviour {

	public void LoadLevel1()
	{
		InstanceFinder.LevelManager.LoadLevelByWorldIndex(0);
	}
}
