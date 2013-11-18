#pragma strict

function OnGUI () {
	// Make a background box
	GUI.Box (Rect (10,10,100,390), "Animations");

	// Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
	if (GUI.Button (Rect (20,40,80,20), "Walk")) {
		animation.wrapMode = WrapMode.Loop;
		animation.Play("Momo_Walk");
//		animation.Play("walk", PlayMode.StopAll);
	}

		if (Input.GetKey("1")) {
		animation.wrapMode = WrapMode.Loop;
		animation.Play("Momo_Walk");
	}
	
	// Make the second button.
		if (GUI.Button (Rect (20,70,80,20), "Kick")) {
		animation.wrapMode = WrapMode.Loop;
		animation.Play("Momo_Kick");
	}
		
		if (Input.GetKey("2")) {
		animation.wrapMode = WrapMode.Loop;
		animation.Play("Momo_Kick");
	}
	
		if (GUI.Button (Rect (20,100,80,20), "Run")) {
		animation.wrapMode = WrapMode.Loop;
		animation.Play("Momo_Run");
	}
	
		if (Input.GetKey("3")) {
		animation.wrapMode = WrapMode.Loop;
		animation.Play("Momo_Run");
	}
	
		if (GUI.Button (Rect (20,130,80,20), "Death")) {
		animation.wrapMode = WrapMode.Loop;
		animation.Play("Momo_Death");
	}
			if (Input.GetKey("4")) {
		animation.wrapMode = WrapMode.Loop;
		animation.Play("Momo_Death");
	}
	
		if (GUI.Button (Rect (20,160,80,20), "TailWhip")) {
		animation.wrapMode = WrapMode.Loop;
		animation.Play("Momo_TailWhip");
	}
	
		if (Input.GetKey("5")) {
		animation.wrapMode = WrapMode.Loop;
		animation.Play("Momo_TailWhip");
	}
	
		if (GUI.Button (Rect (20,190,80,20), "Idle")) {
		animation.wrapMode = WrapMode.Loop;
		animation.Play("Momo_IdleNasty");
	}
	
		if (Input.GetKey("6")) {
		animation.wrapMode = WrapMode.Loop;
		animation.Play("Momo_IdleNasty");
	}
	
		if (GUI.Button (Rect (20,220,80,20), "Throw")) {
		animation.wrapMode = WrapMode.Loop;
		animation.Play("Momo_Throw1");
	}
	
		if (Input.GetKey("7")) {
		animation.wrapMode = WrapMode.Loop;
		animation.Play("Momo_Throw1");
	}
	
		if (GUI.Button (Rect (20,250,80,20), "Hit")) {
		animation.wrapMode = WrapMode.Loop;
		animation.Play("Momo_Hit_Lightly");
	}
	
		if (Input.GetKey("8")) {
		animation.wrapMode = WrapMode.Loop;
		animation.Play("Momo_Hit_Lightly");
	}
	
		if (GUI.Button (Rect (20,280,80,20), "Revive")) {
		animation.wrapMode = WrapMode.Loop;
		animation.Play("Momo_Revive");
	}
	
		if (Input.GetKey("9")) {
		animation.wrapMode = WrapMode.Loop;
		animation.Play("Momo_Revive");
	}
	
		if (GUI.Button (Rect (20,310,80,20), "WallFlip")) {
		animation.wrapMode = WrapMode.Loop;
		animation.Play("Momo_WallFlip");
	}
	
		if (Input.GetKey("0")) {
		animation.wrapMode = WrapMode.Loop;
		animation.Play("Momo_WallFlip");
	}
	
		if (GUI.Button (Rect (20,340,80,20), "Drowning")) {
		animation.wrapMode = WrapMode.Loop;
		animation.Play("Momo_Drowning");
	}
	
		if (Input.GetKey("q")) {
		animation.wrapMode = WrapMode.Loop;
		animation.Play("Momo_Drowning");
	}
	
		if (GUI.Button (Rect (20,370,80,20), "Jump")) {
		animation.wrapMode = WrapMode.Loop;
		animation.Play("Momo_Jump");
	}
	
		if (Input.GetKey("w")) {
		animation.wrapMode = WrapMode.Loop;
		animation.Play("Momo_Jump");
	}
	
						if (GUI.Button (Rect (20,420,80,20), "STOP")) {
		animation.wrapMode = WrapMode.Loop;
		animation.Stop();
	}
}

