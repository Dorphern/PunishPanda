﻿using UnityEngine;
using System.Collections;

public class Animations : MonoBehaviour {

    private Animator anim;
    private PandaStateManager stateManager;
    PandaAI pandaAI;
    private PandaState currentStatePanda;
    private PandaDirection currentDirection;

    # region Private Methods
    // Use this for initialization
    void Start ()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
        stateManager = gameObject.GetComponent<PandaStateManager>();
        pandaAI = gameObject.GetComponent<PandaAI>();
    }

    IEnumerator SetNewPandaState (PandaState state)
    {
        anim.SetBool("NewState", true);
        anim.SetInteger("PandaState", (int) state);
        yield return new WaitForEndOfFrame();
        anim.SetBool("NewState", false);
    }

    IEnumerator EndSlap (PandaDirection dir, bool isInFace)
    {
        yield return new WaitForSeconds(0.37f);

        anim.SetBool(dir.ToString(), false);
        anim.SetBool("Slapped", false);
        anim.SetBool("Face", false);

        stateManager.ChangeState(PandaState.Walking);
    }

    IEnumerator CheckAnimationState (AnimatorStateInfo animStateInfo)
    {
        yield return new WaitForSeconds(animStateInfo.length);
        pandaAI.stuckOnSpikes = false;
    }
    # endregion

    # region Public Methods
    public void ChangePandaState (PandaState state)
    {
        StartCoroutine(SetNewPandaState(state));
    }

    public void PlayAnimation(PandaState statePanda, bool pandaStateBool, PandaState pandaStateLast, PandaDirection currentDirection)
    {

        anim.SetBool(pandaStateLast.ToString(), false);

        Vector3 holdingTargetDirection = new Vector3(transform.eulerAngles.x, 60f, transform.eulerAngles.z);
        Vector3 pushingTargetDirection = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 180f, transform.eulerAngles.z);

        if (statePanda == PandaState.PushingFinger)
        {
            //    transform.GetComponentInChildren<Transform>().eulerAngles = pushingTargetDirection;
            //Vector3 targetChildDirectionVec = new Vector3(0f, 180f, 0f);
            //transform.FindChild("WalkExport_2").transform.localEulerAngles += targetChildDirectionVec;
        }
        else if (pandaStateLast == PandaState.PushingFinger)
        {
            //Vector3 targetChildDirectionVec = new Vector3(0f, 180f, 0f);
            //transform.FindChild("WalkExport_2").transform.localEulerAngles -= targetChildDirectionVec;
        }


        if (currentDirection == PandaDirection.Left)
        {
            anim.SetBool(currentDirection.ToString(), pandaStateBool);
            anim.SetBool("Right", false);
        }
        else
        {
            anim.SetBool(currentDirection.ToString(), pandaStateBool);
            anim.SetBool("Left", false);
        }

        anim.SetBool(statePanda.ToString(), pandaStateBool);
        StartCoroutine(CheckAnimationState(anim.GetCurrentAnimatorStateInfo(0)));

    }
    public void PlayDeathAnimation(TrapType typeTrap, bool hitTrap, PandaState pandaStateLast)
    {
        if (typeTrap == TrapType.StaticSpikes)
            pandaAI.stuckOnSpikes = true;
        anim.SetBool(pandaStateLast.ToString(), false);
        anim.SetBool(typeTrap.ToString(), hitTrap);

    }

    public void PlaySlappedAnimation(PandaDirection dir, bool isInFace, PandaState pandaStateLast)
    {
        anim.SetBool(pandaStateLast.ToString(), false);
        anim.SetBool("Slapped", true);

        bool leftDir = dir == PandaDirection.Left;
        anim.SetBool("Left", leftDir);
        anim.SetBool("Right", !leftDir);
        anim.SetBool("Face", isInFace);

        pandaAI.ChangeDirection(null);
        StartCoroutine(EndSlap(dir, isInFace));
    }
    # endregion
}
