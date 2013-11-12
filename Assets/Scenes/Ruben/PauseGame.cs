using UnityEngine;
using System.Collections;

public class PauseGame : MonoBehaviour {
	
	private float savedTimeScale;	

	void Start () 
	{
		savedTimeScale = Time.timeScale;
	}
	

	public void StopTime()
	{
		Time.timeScale = 0;
		//we also may need to pause Audio here..
	}
	
	public void ResumeGame()
	{
		Time.timeScale = savedTimeScale;
	}
	
	public void RestartLevel()
	{
	}
}
