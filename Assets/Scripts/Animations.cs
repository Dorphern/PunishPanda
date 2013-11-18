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

        Vector3 targetDirection = new Vector3(transform.eulerAngles.x, 90, transform.eulerAngles.z);
        Vector3 targetDirectionX = new Vector3(180, 90, transform.eulerAngles.z);

        if (statePanda == PandaState.HoldingOntoFinger && currentDirection == PandaDirection.Left)
        {
            transform.GetComponentInChildren<Transform>().eulerAngles = targetDirection;
        }
        else if (statePanda == PandaState.HoldingOntoFinger && currentDirection == PandaDirection.Right)
        {
            transform.GetComponentInChildren<Transform>().eulerAngles = targetDirectionX;
        }

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
        
        StartCoroutine(EndSlap(dir));

    }
    IEnumerator EndSlap(PandaDirection dir)
    {

        yield return new WaitForSeconds(0.6f);

        
        stateManager.ChangeState(PandaState.Walking);
        anim.SetBool(dir.ToString(), false);
        anim.SetBool("Slapped", false);        
        anim.SetBool("Face", false);
        pandaAI.ChangeDirection(null);
    }
}
