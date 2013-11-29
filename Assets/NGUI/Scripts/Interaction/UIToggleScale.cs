//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2013 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Simple example script of how a button can be scaled visibly when the mouse hovers over it or it gets pressed.
/// </summary>

[AddComponentMenu("NGUI/Interaction/Toggle Scale")]
public class UIToggleScale : MonoBehaviour
{
	public Transform tweenTarget;
	public Transform tweenTarget2;
	public Vector3 hover = new Vector3(1.1f, 1.1f, 1.1f);
	public Vector3 pressed = new Vector3(1.05f, 1.05f, 1.05f);
	public Vector3 selected = new Vector3(1.5f, 1.5f, 1.5f);
	public Vector3 Default = new Vector3(1.5f, 1.5f, 1.5f);
	public float duration = 0.2f;

	Vector3 mScale;
	bool mStarted = false;
	bool mHighlighted = false;

	void Start ()
	{
		if (!mStarted)
		{
			mStarted = true;
			if (tweenTarget == null) tweenTarget = transform;
			mScale = tweenTarget.localScale;
			UIToggle toggle = GetComponent<UIToggle>();
			if(toggle!=null)
			{
				if(toggle.value == true)
					OnToggleActivate();
				toggle.OnToggleActivate += OnToggleActivate;
				toggle.OnToggleDeactivate += OnToggleDeactivate;
			}
		}
	}
//
//	void OnEnable () { if (mStarted && mHighlighted) OnHover(UICamera.IsHighlighted(gameObject)); }

	void OnDisable ()
	{
		if (mStarted && tweenTarget != null)
		{
			TweenScale tc = tweenTarget.GetComponent<TweenScale>();

			if (tc != null)
			{
				tc.scale = mScale;
				tc.enabled = false;
			}
		}
	}

	void OnToggleActivate()
	{
		if (enabled)
		{
			if (!mStarted) Start();
			TweenScale.Begin(tweenTarget.gameObject, duration, selected).method = UITweener.Method.EaseIn;
			TweenScale.Begin(tweenTarget2.gameObject, duration, selected).method = UITweener.Method.EaseIn;
			
		}
	}
	
	void OnToggleDeactivate()
	{
		if (enabled)
		{
			if (!mStarted) Start();
			TweenScale.Begin(tweenTarget.gameObject, duration, mScale).method = UITweener.Method.BounceOut;
			TweenScale.Begin(tweenTarget2.gameObject, duration, mScale).method = UITweener.Method.BounceOut;
		}
	}
	
//	void OnPress (bool isPressed)
//	{
//		if (enabled)
//		{
//			if (!mStarted) Start();
//			TweenScale.Begin(tweenTarget.gameObject, duration, isPressed ? Vector3.Scale(mScale, pressed) :
//				(UICamera.IsHighlighted(gameObject) ? Vector3.Scale(mScale, hover) : mScale)).method = UITweener.Method.EaseInOut;
//		}
//	}

//	void OnHover (bool isOver)
//	{
//		if (enabled)
//		{
//			if (!mStarted) Start();
//			TweenScale.Begin(tweenTarget.gameObject, duration, isOver ? Vector3.Scale(mScale, hover) : mScale).method = UITweener.Method.EaseInOut;
//			mHighlighted = isOver;
//		}
//	}
}
