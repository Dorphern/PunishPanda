//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2013 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Simple example script of how a button can be scaled visibly when the mouse hovers over it or it gets pressed.
/// </summary>

[AddComponentMenu("NGUI/Interaction/Toggle Scale")]
public class UIToggleScale : MonoBehaviour
{
	
	public Transform tweentarget1;
	public Transform tweentarget2;
	public List<Transform> tweenTargets;
//	public Vector3 hover = new Vector3(1.1f, 1.1f, 1.1f);
//	public Vector3 pressed = new Vector3(1.05f, 1.05f, 1.05f);
	public Vector3 selected = new Vector3(1.5f, 1.5f, 1.5f);

	public float duration = 0.2f;

	Vector3 mScale;
	bool mStarted = false;
	bool mHighlighted = false;
	UIToggle toggle;
	
	// using awake since this need to be initialized before start
	void Awake ()
	{
		if (!mStarted)
		{
			mStarted = true;
			if (tweenTargets.Count == 0) tweenTargets.Add(transform);
			mScale = tweenTargets[0].localScale;
			toggle = GetComponent<UIToggle>();
			if(toggle!=null)
			{
//				if(toggle.value == true)
//					OnToggleActivate();
				toggle.OnToggleActivate += OnToggleActivate;
				toggle.OnToggleDeactivate += OnToggleDeactivate;
			}
		}
	}
	
//	void OnEnable ()
//	{
//		if (mStarted && tweenTargets.Count != 0)
//		{
//			for(int i=0; i< tweenTargets.Count; i++)
//			{
//				TweenScale tc = tweenTargets[i].GetComponent<TweenScale>();
//
//				if (tc != null)
//				{
////					if(toggle.value = true)
////						tc.scale = mScale;
////					else
////						tc.scale = mScale;
//					//tc.enabled = true;
//				}
//			}
//		}
//	}
//	
//	void OnDisable ()
//	{
//		if (mStarted && tweenTargets.Count != 0)
//		{
//			for(int i=0; i< tweenTargets.Count; i++)
//			{
//				TweenScale tc = tweenTargets[i].GetComponent<TweenScale>();
//
//				if (tc != null)
//				{
//					tc.scale = mScale;
//					tc.enabled = false;
//				}
//			}
//		}
//	}

	void OnToggleActivate()
	{
		if (enabled)
		{
			if (!mStarted) Awake();
			for(int i=0; i<tweenTargets.Count; i++)
			{
//				if(tweentarget1!=null && tweentarget2!=null)
//				{
//					TweenScale.Begin(tweenTargets[i].gameObject, duration, selected).method = UITweener.Method.EaseIn;
//				}
				
				if(tweenTargets[i]!=null)
					TweenScale.Begin(tweenTargets[i].gameObject, duration, selected).method = UITweener.Method.EaseIn;
			}
		}
	}
	
	void OnToggleDeactivate()
	{
		if (enabled)
		{
			if (!mStarted) Awake();
			for(int i=0; i<tweenTargets.Count; i++)
			{
				if(tweenTargets[i]!=null)
					TweenScale.Begin(tweenTargets[i].gameObject, duration, mScale).method = UITweener.Method.EaseOut;
			}
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
