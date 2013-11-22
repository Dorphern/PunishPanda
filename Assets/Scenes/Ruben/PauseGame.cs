using UnityEngine;
using System.Collections;

public class PauseGame : MonoBehaviour {
	
	private float savedTimeScale;	

	void Start () 
	{
		savedTimeScale = Time.timeScale;
		//Debug.Log ("savingTimeScale " + gameObject);
	}
	

	public void StopTime()
	{
		Time.timeScale = 0;
		//Debug.Log ("StopTime()" + gameObject);
		//PAUSE AUDIO ALSO??
	}
	
	public void ResumeGame()
	{
		//Debug.Log ("ResumeGame()" + gameObject);
		Time.timeScale = savedTimeScale;
	}
	
	public void RestartLevel()
	{
		InstanceFinder.LevelManager.Reload();
	}
}
