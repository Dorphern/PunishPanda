using UnityEngine;
using System.Collections;

public class DoorTrap : TrapBase {
	
	public string animationName;
	private AnimationState trapClip;
	
	void Start()
	{
		trapClip = this.animation[animationName];	
	}
	
	public override void ActivateTrap ()
	{
		base.ActivateTrap ();
		trapClip.speed = 1f;
		if(animation.isPlaying == false)
		{
			trapClip.time = 0f;
		}
		this.animation.Play();
	}
	
	public override void DeactivateTrap ()
	{
		base.DeactivateTrap ();
		trapClip.speed = -1f;
		if(animation.isPlaying == false)
		{
			trapClip.time = trapClip.length;	
		}
		this.animation.Play();
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
