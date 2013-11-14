using UnityEngine;
using System;
using System.Collections;

public class PandaAIMainMenu : MonoBehaviour {
	
	public float slapStateLength = 1f;
	
	PandaStateManager pandaStateManager;
	BloodOnSlap bloodOnSlap;
	
	
	#region Public Methods
    public void PandaSlapped(Vector2 slapDirection, float force)
	{
		// we can slap the panda only in tanding state
		if(pandaStateManager.GetState() != PandaState.Standing)
			return;
		
		// project blood and change state to standing
		StartCoroutine(PlaySlap(slapStateLength, slapDirection));
		
		float dot = Vector2.Dot(slapDirection.normalized, - Vector2.right);
		if(dot > 0f)
		{
			// panda gets slapped from the left
			// play animation
		}
		else
		{
			// panda gets slapped from the right
			// play animation
		}
		// blood particles
		bloodOnSlap.EmmitSlapBloodWithAngle(slapDirection.normalized);
	}
	#endregion
	
    # region Private Methods
	// Use this for initialization
	void Start()
	{
		pandaStateManager = GetComponent<PandaStateManager>();
		bloodOnSlap = GetComponent<BloodOnSlap>();
	}
	
	// Update is called once per frame
	void FixedUpdate() 
	{	
		
	}

	IEnumerator PlaySlap(float waitForSeconds, Vector2 slapDirection)
	{
		BloodSplatter.Instance.ProjectBlood(transform.position, slapDirection.normalized);
		
		yield return new WaitForSeconds(waitForSeconds);
		
		pandaStateManager.ChangeState(PandaState.Standing);
		
		// play standing (idle) animation
	}

	# endregion
		
}
