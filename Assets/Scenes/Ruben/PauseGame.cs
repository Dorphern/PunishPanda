using UnityEngine;
using System.Collections.Generic;

public class PauseGame : MonoBehaviour {
	
	private float savedTimeScale;
	private GameObject inputHandler;
	private InputHandler inputScript;
	
	[SerializeField] [EventHookAttribute("PauseGame")]
	List<AudioEvent> pauseGameEvent;
	
	[SerializeField] [EventHookAttribute("ResumeGame")]
	List<AudioEvent> resumeGameEvent;

	void Start () 
	{
		savedTimeScale = Time.timeScale;
		inputScript = InputHandler.instance;
	}
	

	public void StopTime()
	{
		Time.timeScale = 0;
        InputHandler.instance.PausedGame();
		
		//PAUSE AUDIO ALSO??
		for(int i = 0; i < pauseGameEvent.Count; ++i)
		{
			HDRSystem.PostEvent(gameObject, pauseGameEvent[i]);
		}
	}
	
	public void TutorialPause()
	{
		Time.timeScale = 0;
    
		
		//PAUSE AUDIO ALSO??
		for(int i = 0; i < pauseGameEvent.Count; ++i)
		{
			HDRSystem.PostEvent(gameObject, pauseGameEvent[i]);
		}
	}
	
	public void ResumeGame()
	{
		Time.timeScale = savedTimeScale;
        InputHandler.instance.UnpausedGame();
		Debug.Log("RESUMED");
		
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
