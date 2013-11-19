using UnityEngine;
using System.Collections;

public class Animations : MonoBehaviour {

    private Animator anim;
    private PandaStateManager stateManager;
    PandaAI pandaAI;
    private PandaState currentStatePanda;
    private PandaDirection currentDirection;


	// Use this for initialization
	void Start () 
	{
        anim = gameObject.GetComponentInChildren<Animator>();
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
        {
            //    transform.GetComponentInChildren<Transform>().eulerAngles = pushingTargetDirection;
            Vector3 targetChildDirectionVec = new Vector3(0f, 180f, 0f);
            transform.FindChild("WalkExport_2").transform.localEulerAngles += targetChildDirectionVec;
        }
        else if (pandaStateLast == PandaState.PushingFinger)
        {
            Vector3 targetChildDirectionVec = new Vector3(0f, 180f, 0f);
            transform.FindChild("WalkExport_2").transform.localEulerAngles -= targetChildDirectionVec;
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

    }
    public void PlayDeathAnimation(TrapType typeTrap, bool hitTrap)
    {
       // anim.SetBool(pandaStateLast.ToString(), false);
       // anim.SetBool(typeTrap.ToString(), hitTrap);
    }

    public void PlaySlappedAnimation(PandaState statePanda, bool pandaStateBool, PandaDirection dir, bool isInFace, PandaState pandaStateLast)
    {


        //Vector3 targetChildDirectionVec = new Vector3(0f, 180f, 0f);
        //transform.FindChild("WalkExport_2").transform.localEulerAngles += targetChildDirectionVec;

        //Quaternion targetChildDirectionQua = transform.FindChild("WalkExport_2").transform.rotation;


        
        anim.SetBool(statePanda.ToString(), pandaStateBool);
        anim.SetBool(dir.ToString(), pandaStateBool);
        anim.SetBool("Face", isInFace);

       // anim.MatchTarget(transform.position, targetChildDirectionQua, AvatarTarget.Root, new MatchTargetWeightMask (new Vector3(0f, 1f, 0f), 0f), 0f, 0.64f);
        StartCoroutine(EndSlap(dir, isInFace));

    }
    IEnumerator EndSlap(PandaDirection dir, bool isInFace)
    {



        yield return new WaitForSeconds(0.6f);

        //Vector3 targetChildDirection = new Vector3(0f, -180f, 0f);
        //transform.FindChild("WalkExport_2").transform.localEulerAngles += targetChildDirection;

        anim.SetBool(dir.ToString(), false);
        anim.SetBool("Slapped", false);
        anim.SetBool("Face", false);
        if (isInFace)
            pandaAI.ChangeDirection(null);
        stateManager.ChangeState(PandaState.Walking);
 

        
        
        


    }
}
