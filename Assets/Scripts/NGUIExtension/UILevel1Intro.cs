using System;
using UnityEngine;

/// <summary>
/// Handles swipe control 
/// </summary>

[AddComponentMenu("PunishPanda/Level 1 Intro")]
public class UILevel1Intro : MonoBehaviour
{
    [SerializeField] private UISwipeControl swipeControl;

    [SerializeField] private GameObject StartButton;
    [SerializeField] private GameObject SkipButton;
    [SerializeField] private GameObject NextButton;

    public void StartLevelOne()
    {
        InstanceFinder.LevelManager.LoadLevelByWorldIndex(0);
    }

    void Update()
    {
        if (swipeControl.HasReachedRightEnd)
        {
            StartButton.SetActive(true);
            SkipButton.SetActive(false);
            NextButton.SetActive(false);
        }
        else
        {
            StartButton.SetActive(false);
            SkipButton.SetActive(true);
            NextButton.SetActive(true);
        }
    }
}