﻿using UnityEngine;
using System.Collections;

public class Intro : MonoBehaviour
{
    public string Path = "Intro.mp4";
	// Use this for initialization
	void Start ()
	{
#if UNITY_ANDROID
	    Handheld.PlayFullScreenMovie(Path, Color.black, FullScreenMovieControlMode.Minimal, FullScreenMovieScalingMode.None);
#endif
		InstanceFinder.LevelManager.LoadLevelByWorldIndex(0);

	}
}