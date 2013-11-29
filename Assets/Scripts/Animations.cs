using UnityEngine;
using System.Collections;

public class Animations : MonoBehaviour {

    private Animator anim;
    private PandaStateManager stateManager;
    private CharacterController characterController;
    private PandaMovementController pandaMovementController;
    private Collidable collidable;
    PandaAI pandaAI;
    private PandaState currentStatePanda;
    private PandaDirection currentDirection;

    static int staticSpikes = Animator.StringToHash("Base.StaticSpikes");
    static int SpikedDeathFAll = Animator.StringToHash("Base.SpikedDeathFAll");
    static int deathSpikeImpact = Animator.StringToHash("Base.DeathSpikeImpact");
    static int spikedDeathFall = Animator.StringToHash("Base.SpikedDeathFall");
    static int jumping = Animator.StringToHash("Base.Jumping");
    static int walking = Animator.StringToHash("Base.Walking");

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
        yield return new WaitForEndOfFrame();

        anim.SetBool("Slapped", false);
        anim.SetBool("Face", false);
    }

    IEnumerator CheckAnimationState (AnimatorStateInfo animStateInfo, PandaState statePanda)
    {
        yield return new WaitForSeconds(animStateInfo.length);
        pandaAI.stuckOnSpikes = false;
        anim.SetBool("LandingHard", false);
        if(statePanda == PandaState.Escape)
        {
            pandaMovementController.PandaEscapeAway();
        }
    }
    # endregion

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

        anim.SetBool(statePanda.ToString(), pandaStateBool);
        anim.SetBool("Grounded", characterController.isGrounded);
        anim.SetBool("LandingHard", pandaAI.landingHard);
        StartCoroutine(CheckAnimationState(anim.GetCurrentAnimatorStateInfo(0), statePanda));

    }
    public void PlayDeathAnimation(TrapBase trap, bool hitTrap, PandaDirection pandaDirection, PandaState pandaStateLast)
    {
        anim.SetBool(pandaStateLast.ToString(), false);
        anim.SetInteger("TrapPosition", (int) trap.GetTrapPosition());
        anim.SetInteger("TrapType", (int)trap.GetTrapType());
        anim.SetInteger("Direction", (int) pandaDirection);
    }

    public void SetSlapped(bool front)
    {
        anim.SetBool("Front", front);
        anim.SetBool("Slapped", true);

        StartCoroutine(ResetSlap());
    }

    public void PlayTriggerAnimations(PandaDirection pandaDirection, CollidableTypes collidableType)
    {
        anim.SetInteger("CollidableType", (int)collidableType);
        anim.SetInteger("Direction", (int)pandaDirection);
    }
    # endregion
}
