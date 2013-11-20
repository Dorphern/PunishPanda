using UnityEngine;
using System.Collections;

public class Animations : MonoBehaviour {

    private Animator anim;
    private PandaStateManager stateManager;
    PandaAI pandaAI;
    private PandaState currentStatePanda;
    private PandaState lastPandaState;
    private PandaDirection currentDirection;
    private float rotationSpeed = 50f;
    public AvatarTarget bodyPart;

	// Use this for initialization
	void Start () {
        anim = gameObject.GetComponentInChildren<Animator>();
        lastPandaState = gameObject.GetComponent<PandaStateManager>().GetState();
        stateManager = gameObject.GetComponent<PandaStateManager>();
        pandaAI = gameObject.GetComponent<PandaAI>();

        
	}


    public void PlayAnimation(PandaState statePanda, bool pandaStateBool, PandaState pandaStateLast, PandaDirection currentDirection)
    {

        anim.SetBool(pandaStateLast.ToString(), false);

        Vector3 holdingTargetDirection = new Vector3(transform.eulerAngles.x, 60f, transform.eulerAngles.z);

        Vector3 pushingTargetDirection = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 180f, transform.eulerAngles.z);


        if (statePanda == PandaState.HoldingOntoFinger && currentDirection == PandaDirection.Right)
            transform.GetComponentInChildren<Transform>().eulerAngles = new Vector3(transform.eulerAngles.x, 60f, transform.eulerAngles.z);
        else if (statePanda == PandaState.HoldingOntoFinger && currentDirection == PandaDirection.Left)
            transform.GetComponentInChildren<Transform>().eulerAngles = new Vector3(transform.eulerAngles.x, 120f, transform.eulerAngles.z);

        if (statePanda == PandaState.Falling && currentDirection == PandaDirection.Right)
            transform.GetComponentInChildren<Transform>().eulerAngles = new Vector3(transform.eulerAngles.x, 60f, transform.eulerAngles.z);
        else if (statePanda == PandaState.Falling && currentDirection == PandaDirection.Left)
            transform.GetComponentInChildren<Transform>().eulerAngles = new Vector3(transform.eulerAngles.x, 120f, transform.eulerAngles.z);

        if (statePanda == PandaState.PushingFinger)
            transform.GetComponentInChildren<Transform>().eulerAngles = pushingTargetDirection;

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

    }

    public void PlayDeathAnimation(TrapType typeTrap, bool hitTrap)
    {
       // anim.SetBool(pandaStateLast.ToString(), false);
       // anim.SetBool(typeTrap.ToString(), hitTrap);
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


    IEnumerator EndSlap(PandaDirection dir, bool isInFace)
    {
        yield return new WaitForSeconds(0.37f);

        anim.SetBool(dir.ToString(), false);
        anim.SetBool("Slapped", false);
        anim.SetBool("Face", false);  

        stateManager.ChangeState(PandaState.Walking);
    }
}
