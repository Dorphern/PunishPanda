using UnityEngine;
using System.Collections;

public class Animations : MonoBehaviour {

    private Animator anim;
    private CollidableTypes collisionTypes;
    private PandaState currentStatePanda;
    private PandaState lastPandState;

    private int walkingState = Animator.StringToHash("Base Layer.Walking");
    private int fallingState = Animator.StringToHash("Base Layer.Falling");

	// Use this for initialization
	void Start () {
        anim = gameObject.GetComponentInChildren<Animator>();
        collisionTypes = GetComponent<Collidable>().type;
        lastPandState = gameObject.GetComponent<PandaStateManager>().GetState(); ;

	}
	
	// Update is called once per frame
	void Update () {

        currentStatePanda = gameObject.GetComponent<PandaStateManager>().GetState();
        if(lastPandState != currentStatePanda)
        {
            playAnimation(currentStatePanda, true, lastPandState);
            lastPandState = currentStatePanda;
        }
        
	}

    public void playAnimation(PandaState statePanda, bool pandaStateBool, PandaState pandaStateLast)
    {
        Debug.Log(statePanda);
        anim.SetBool(pandaStateLast.ToString(), false);
       if(statePanda == PandaState.Died)
       {
           //anim.SetBool(Traptypes.ToString(), pandaStateBool);
       }
       else
       {
           anim.SetBool(statePanda.ToString(), pandaStateBool);
       }
        

    }
}
