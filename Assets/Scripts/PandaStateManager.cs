﻿using UnityEngine;
using System.Collections;


/**
 * PandaStateManager
 * Manages the current state, doesn't set it, just broardcasts events
 * when entering and exiting states. For the pandastate itself it also calls onUpdate.
 **/


public enum PandaState
{
    Idle,           /* Is standing still */
    Walking,            /* Is walking in some direction */
    PushingFinger,      /* Is Pushing a finger (not moving) */
    Died,               /* The panda is DEAD! (hahahah) */
	Jumping,
    Falling,
    FallSplat,
    Slapped,
	FallTransition,
	Boosting
}


public enum PandaDirection
{
    Left,
    Right
}

public class PandaStateManager : MonoBehaviour {

    // Event Handlers
    public delegate void StateHandler(PandaState state);
    public delegate void DirectionHandler(PandaDirection dir);

    public event StateHandler onStateExit;
    public event StateHandler onStateEnter;
    public event StateHandler onStateUpdate;

    public event DirectionHandler onDirectionExit;
    public event DirectionHandler onDirectionEnter;

    [SerializeField] private PandaState initState = PandaState.Idle;
	[SerializeField] private PandaDirection initDirection = PandaDirection.Left;
    private PandaState currentState;
    private PandaDirection currentDirection;
    private int slapCount = 0;

    # region Public Methods
	
	public void SwapDirection(PandaDirection dir)
	{
		if(dir == PandaDirection.Left)
		{
			ChangeDirection(PandaDirection.Right);
		}
		else
		{
			ChangeDirection(PandaDirection.Left);
		}	
	}
	
    // Change the state of the panda
    public void ChangeState (PandaState state)
    {
		// Debug
		//this.GetComponentInChildren<TextMesh>().text = state.ToString();
		
        if (state == currentState)
        {
            return;
        }
        if (onStateExit != null) onStateExit(currentState);
        currentState = state;
        if (onStateEnter != null) onStateEnter(currentState);
    }

    public PandaState GetState ()
    {
        return currentState;
    }

    public void ChangeDirection (PandaDirection direction)
    {
        if (direction == currentDirection)
        {
            return;
        }
        if (onDirectionExit != null) onDirectionExit(currentDirection);
        currentDirection = direction;
        if (onDirectionEnter != null) onDirectionEnter(currentDirection);
    }

    public PandaDirection GetDirection ()
    {
        return currentDirection;
    }

    public int GetSlapCount ()
    {
        return slapCount;
    }


    public void IncrementSlapCount ()
    {
        slapCount++;
    }

    # endregion

    # region Private Methods
    void Start () 
    {
        currentState = initState;
		currentDirection = initDirection;
	}
	
	void Update () 
    {
        if (onStateUpdate != null) onStateUpdate(currentState);
    }

    # endregion
}
