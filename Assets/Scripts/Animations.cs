using UnityEngine;
using System.Collections;

public class Animations : MonoBehaviour {

    [SerializeField] float randomMinWait = 2f;
    [SerializeField] float randomMaxWait = 6f;

    private Animator anim;
    private PandaStateManager stateManager;
    private CharacterController characterController;
    private PandaMovementController pandaMovementController;
    private Collidable collidable;
    PandaAI pandaAI;
    private PandaState currentStatePanda;
    private PandaDirection currentDirection;

    private string escapeUpAnimation = "escapeUp";
    private string escapeDownAnimation = "escapeDown";

    # region Public Methods
    public void ChangePandaState (PandaState state)
    {
        StartCoroutine(SetNewPandaState(state));
    }

    public void ChangePandaDirection (PandaDirection direction)
    {
        anim.SetInteger("Direction", (int) direction);
    }

    public void PlayAnimation(PandaState statePanda, bool pandaStateBool, PandaState pandaStateLast, PandaDirection currentDirection)
    {
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

        anim.SetBool(statePanda.ToString(), pandaStateBool);
        anim.SetBool("LandingHard", pandaAI.landingHard);
        StartCoroutine(CheckAnimationState(anim.GetCurrentAnimatorStateInfo(0), statePanda));

    }
    public void PlayDeathAnimation(TrapBase trap, PandaDirection pandaDirection)
    {
        anim.SetInteger("TrapPosition", (int) trap.GetTrapPosition());
        anim.SetInteger("TrapType", (int) trap.GetTrapType());
        anim.SetInteger("Direction", (int) pandaDirection);
        if (trap.GetTrapType() == TrapType.EscapeBamboo)
        {
            StartCoroutine(PandaEscapeMove(trap.GetTrapPosition()));
        }

        bool front = (trap.GetTrapPosition() == TrapPosition.WallRight && pandaDirection == PandaDirection.Right)
            || (trap.GetTrapPosition() == TrapPosition.WallLeft && pandaDirection == PandaDirection.Left);
        anim.SetBool("Front", front);
    }

    public void SetSlapped(bool front)
    {
        anim.SetBool("Front", front);
        anim.SetBool("Slapped", true);

        StartCoroutine(ResetSlap());
    }

    public void PlayTriggerAnimations(PandaDirection pandaDirection, CollidableTypes collidableType)
    {
        StartCoroutine(ChangeCollidableType(collidableType));
        anim.SetInteger("Direction", (int)pandaDirection);
    }

    public void SpikePullOut()
    {
        anim.SetBool("PullOutSpikes", true);
        StartCoroutine(ChangeSpikesPullOut());
    }

    public void SetGrounded()
    {
        anim.SetBool("Grounded", characterController.isGrounded);
    }

    # endregion

    # region Private Methods
    // Use this for initialization
    void Start ()
    {
        anim = GetComponentInChildren<Animator>();
        stateManager = GetComponent<PandaStateManager>();
        pandaAI = GetComponent<PandaAI>();
        characterController = GetComponent<CharacterController>();
        pandaMovementController = GetComponent<PandaMovementController>();

        anim.SetInteger("Direction", (int) stateManager.initDirection);

        collidable = GetComponent<Collidable>();

        StartCoroutine(RandomNumberUpdater());
    }

    IEnumerator SetNewPandaState (PandaState state)
    {
        anim.SetBool("NewState", true);
        anim.SetInteger("PandaState", (int) state);
        yield return new WaitForEndOfFrame();
        anim.SetBool("NewState", false);
    }

    IEnumerator ResetSlap ()
    {
        anim.SetInteger("CollidableType", -1);
        yield return new WaitForEndOfFrame();
        anim.SetBool("Slapped", false);
        anim.SetBool("Face", false);
    }

    IEnumerator CheckAnimationState (AnimatorStateInfo animStateInfo, PandaState statePanda)
    {
        yield return new WaitForSeconds(animStateInfo.length);
        pandaAI.stuckOnSpikes = false;
        anim.SetBool("LandingHard", false);
    }

    IEnumerator PandaEscapeMove (TrapPosition position)
    {
        if (position == TrapPosition.Ceiling)
        {
            yield return new WaitForSeconds(1.1f);
            animation.Play(escapeUpAnimation);
        }
        else if (position == TrapPosition.Ground)
        {
            yield return new WaitForSeconds(2f);
            animation.Play(escapeDownAnimation);
        }
    }

    IEnumerator ChangeCollidableType (CollidableTypes collidableType)
    {
        anim.SetInteger("CollidableType", (int) collidableType);
        anim.SetBool("NewCollidableType", true);
        yield return new WaitForEndOfFrame();
        anim.SetBool("NewCollidableType", false);
    }

    IEnumerator RandomNumberUpdater ()
    {
        yield return new WaitForSeconds(PandaRandom.NextFloat(0f, randomMaxWait));
        while (stateManager.GetState() != PandaState.Died)
        {
            anim.SetInteger("Random", PandaRandom.NextInt(0, 101));
            anim.SetBool("NewRandom", true);
            yield return new WaitForEndOfFrame();
            anim.SetBool("NewRandom", false);
            yield return new WaitForSeconds(PandaRandom.NextFloat(randomMinWait, randomMaxWait));
        }
    }

    IEnumerator ChangeSpikesPullOut ()
    {
        yield return new WaitForSeconds(0.05f);
        anim.SetBool("PullOutSpikes", false);
    }
    # endregion
}
