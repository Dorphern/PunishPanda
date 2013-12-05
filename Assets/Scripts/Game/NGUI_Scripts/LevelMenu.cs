using UnityEngine;
using System.Collections;

public class LevelMenu : MonoBehaviour {
	
	
	
	public void LoadLevel1()
	{
#if UNITY_EDITOR
        Debug.Log("Skipping intro scene in editor#");
        InstanceFinder.LevelManager.LoadLevelByWorldIndex(0);
#elif UNITY_ANDROID || UNITY_IPHONE || UNITY_WP8
        InstanceFinder.LevelManager.LoadLevel("IntroScene");
#endif

    }
	
	public void LoadLevel2()
	{
		InstanceFinder.LevelManager.LoadLevelByWorldIndex(1);
	}
	
	public void LoadLevel3()
	{
		InstanceFinder.LevelManager.LoadLevelByWorldIndex(2);
	}
	
	public void LoadLevel4()
	{
		InstanceFinder.LevelManager.LoadLevelByWorldIndex(3);
	}
	
	public void LoadLevel5()
	{
		InstanceFinder.LevelManager.LoadLevelByWorldIndex(4);
	}
	
	public void LoadLevel6()
	{
		InstanceFinder.LevelManager.LoadLevelByWorldIndex(5);
	}
	
	public void LoadLevel7()
	{
		InstanceFinder.LevelManager.LoadLevelByWorldIndex(6);
	}
	
	public void LoadLevel8()
	{
		InstanceFinder.LevelManager.LoadLevelByWorldIndex(7);
	}
	
	public void LoadLevel9()
	{
		InstanceFinder.LevelManager.LoadLevelByWorldIndex(8);
	}
	
	public void LoadLevel10()
	{
		InstanceFinder.LevelManager.LoadLevelByWorldIndex(9);
	}
	
	public void LoadLevel11()
	{
		InstanceFinder.LevelManager.LoadLevelByWorldIndex(10);
	}
	
	public void LoadLevel12()
	{
		InstanceFinder.LevelManager.LoadLevelByWorldIndex(11);
	}
	
}
