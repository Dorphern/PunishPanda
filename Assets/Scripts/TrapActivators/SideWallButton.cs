﻿using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class SideWallButton : TrapActivator
{

    public float activationTimeLength = 3f;
    private Collidable colliderType;
    private string animationName = "wallButtonAnimation";
	private bool isActivated = false;

    protected override void ActivateTraps()
    {
		if(this.isActivated == false)
		{
	        base.ActivateTraps();
	        StartCoroutine(PlayActivateAnimation());
	        var buttonSound = GetComponentInChildren<CountDownTime>();
	        if(buttonSound != null)
	            buttonSound.Activate(activationTimeLength);
			
			this.isActivated = true;
		}
    }

    protected override void DeactivateTraps()
    {
        base.DeactivateTraps();
        PlayDeactiveAnimation();
		this.isActivated = false;
    }

    void OnTriggerEnter(Collider collider)
    {
        colliderType = collider.GetComponent<Collidable>();
        if (colliderType == null) return;

        if (colliderType.type == CollidableTypes.Panda)
        {
            ActivateTraps();
            StartCoroutine(ResetState());
        }
    }

    IEnumerator ResetState()
    {
        yield return new WaitForSeconds(activationTimeLength);
        DeactivateTraps();
    }

    IEnumerator PlayActivateAnimation()
    {
		animation[animationName].time = 0;
		animation[animationName].speed = 1;
        animation.Play(animationName);
        yield return new WaitForSeconds(0.2f);
        animation[animationName].speed = 0;
    }

    void PlayDeactiveAnimation()
    {
		animation[animationName].time = animation[animationName].length;
        animation[animationName].speed = -1;
		animation.Play(animationName);
    }
}
