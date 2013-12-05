using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DoorTrap : TrapBase {
	
	public string animationName;
	private AnimationState trapClip;
	private List<PandaAI> pandasOnTrap;
	private DoorTrapFallingTrigger fallingTrigger;
	
	void Awake()
	{
		pandasOnTrap = new List<PandaAI>();
		fallingTrigger = transform.parent.GetComponent<DoorTrapFallingTrigger>();
		trapClip = this.animation[animationName];	
	}
	
	public override void ActivateTrap (bool playAnimation = true)
	{
		base.ActivateTrap ();
		fallingTrigger.PandasFalling();
		if(playAnimation)
		{
			trapClip.speed = 1f;
			if(animation.isPlaying == false)
			{
				trapClip.time = 0f;
			}
			this.animation.Play();
		}
		else
		{
			trapClip.speed = 1f;
			trapClip.time = trapClip.length;
			this.animation.Play();
		}
	}
	
	public override void DeactivateTrap (bool playAnimation = true)
	{
		base.DeactivateTrap ();
		if(playAnimation)
		{
			trapClip.speed = -1f;
			if(animation.isPlaying == false)
			{
				trapClip.time = trapClip.length;	
			}
			this.animation.Play();
		}
		else
		{
			trapClip.speed = -1f;
			trapClip.time = 0f;
			this.animation.Play();
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
