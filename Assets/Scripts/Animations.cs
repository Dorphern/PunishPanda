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


        if (statePanda == PandaState.HoldingOntoFinger)
            transform.GetComponentInChildren<Transform>().eulerAngles = holdingTargetDirection;

        if (statePanda == PandaState.PushingFinger)
            transform.GetComponentInChildren<Transform>().eulerAngles = pushingTargetDirection;

        if (statePanda == PandaState.FallSplat)
            transform.GetComponentInChildren<Transform>().eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);

        anim.SetBool(statePanda.ToString(), pandaStateBool);

    }
    public void PlayDeathAnimation(TrapType typeTrap, bool hitTrap)
    {
       // anim.SetBool(pandaStateLast.ToString(), false);
       // anim.SetBool(typeTrap.ToString(), hitTrap);
    }

    public void PlaySlappedAnimation(PandaState statePanda, bool pandaStateBool, PandaDirection dir, bool isInFace, PandaState pandaStateLast)
    {

        Vector3 targetDirection = new Vector3(transform.eulerAngles.x, 90, transform.eulerAngles.z);
        transform.eulerAngles = targetDirection;

        anim.SetBool(statePanda.ToString(), pandaStateBool);
        anim.SetBool(dir.ToString(), pandaStateBool);
        anim.SetBool("Face", isInFace);
        
        StartCoroutine(EndSlap(dir, isInFace));

    }
    IEnumerator EndSlap(PandaDirection dir, bool isInFace)
    {

        yield return new WaitForSeconds(0.6f);
        if (isInFace)
            pandaAI.ChangeDirection(null);
        Debug.Log("ChangingDirection Aimation");
        stateManager.ChangeState(PandaState.Walking);
        anim.SetBool(dir.ToString(), false);
        anim.SetBool("Slapped", false);        
        anim.SetBool("Face", false);
        
    }
}
