using UnityEngine;
using System.Collections;

public class Intro : MonoBehaviour
{
    public string Path = "Intro.mp4";
	// Use this for initialization
	void Start ()
	{
	    Handheld.PlayFullScreenMovie(Path, Color.black, FullScreenMovieControlMode.Minimal, FullScreenMovieScalingMode.None);
        InstanceFinder.LevelManager.LoadLevelByWorldIndex(0);
	}
}
