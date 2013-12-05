using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DoorTrap : TrapBase {
	
	public string activationClip;
	public string deactivationClip;
	private AnimationState activationState;
	private AnimationState deactivationState;
	private DoorTrapFallingTrigger fallingTrigger;
	
	void Awake()
	{
		fallingTrigger = transform.parent.GetComponent<DoorTrapFallingTrigger>();
		activationState = this.animation[activationClip];	
		deactivationState = this.animation[deactivationClip];	
	}
	
	public override void ActivateTrap (bool playAnimation = true)
	{
		base.ActivateTrap ();
		fallingTrigger.PandasFalling();
		
		if(playAnimation)
		{
			if(animation.isPlaying == false)
			{
				activationState.time = 0f;
			}
			else
			{
				this.animation.Stop();
				activationState.time = deactivationState.time;
			}
			this.animation.Play(activationClip);
		}
		else
		{
			activationState.time = activationState.length;
			this.animation.Play(activationClip);
		}
	}
	
	public override void DeactivateTrap (bool playAnimation = true)
	{
		base.DeactivateTrap ();
		if(playAnimation)
		{
			if(animation.isPlaying == false)
			{
				deactivationState.time = 0f;	
			}
			else
			{
				this.animation.Stop();
				deactivationState.time = activationState.time;
			}
			this.animation.Play(deactivationClip);
		}
		else
		{
			deactivationState.time = deactivationState.length;
			this.animation.Play(deactivationClip);
		}
	}
	
	public override TrapType GetTrapType ()
	{
		return TrapType.DoorTrap;
	}
	
	protected override bool PandaAttemptKill (PandaAI pandaAI, bool isPerfect)
	{
		return true;	
	}
}
