using UnityEngine;
using System.Collections;

public class MainMenuSaws : MonoBehaviour {
	
	public GameObject sawObject;
	SawTrap sawtrap;
	
	
	// Use this for initialization
	void Start () {
		
		sawtrap = sawObject.GetComponent<SawTrap>();
		
	}
	
	public void OnPress(bool isDown)
	{
	    if(isDown)
	    {
		   Debug.Log ("PRESSING");
	       sawtrap.ActivateTrap();
	    }
	 
	    if(!isDown)
	    {
			 Debug.Log ("DEACTIVATE");
	       sawtrap.DeactivateTrap();
	    }
	} 

	
}
