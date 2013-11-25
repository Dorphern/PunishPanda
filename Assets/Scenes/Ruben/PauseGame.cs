using UnityEngine;
using System.Collections.Generic;

public class PauseGame : MonoBehaviour {
	
	private float savedTimeScale;	
	
	[SerializeField] [EventHookAttribute("PauseGame")]
	List<AudioEvent> pauseGameEvent;
	
	[SerializeField] [EventHookAttribute("ResumeGame")]
	List<AudioEvent> resumeGameEvent;

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
		for(int i = 0; i < pauseGameEvent.Count; ++i)
		{
			HDRSystem.PostEvent(gameObject, pauseGameEvent[i]);
		}
	}
	
	public void ResumeGame()
	{
		//Debug.Log ("ResumeGame()" + gameObject);
		Time.timeScale = savedTimeScale;
		for(int i = 0; i < resumeGameEvent.Count; ++i)
		{
			HDRSystem.PostEvent(gameObject, resumeGameEvent[i]);
		}
	}
	
	public void RestartLevel()
	{
		InstanceFinder.LevelManager.Reload();
	}
}
