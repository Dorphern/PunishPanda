using UnityEngine;
using System.Collections;

public class Animations : MonoBehaviour {

    private Animator anim;
    private PandaState currentStatePanda;
    private PandaState lastPandState;

	// Use this for initialization
	void Start () {
        anim = gameObject.GetComponentInChildren<Animator>();
        lastPandState = gameObject.GetComponent<PandaStateManager>().GetState(); ;
        
	}
	
	// Update is called once per frame
	void Update () {

        currentStatePanda = gameObject.GetComponent<PandaStateManager>().GetState();
        if(lastPandState != currentStatePanda)
        {
            if(currentStatePanda == PandaState.Died)
            {
                // call death animation send trap touched
               // playDeathAnimation(, true);
            }
            else
            {
                playAnimation(currentStatePanda, true, lastPandState);
            }
            
            lastPandState = currentStatePanda;
        }
        
	}

    public void playAnimation(PandaState statePanda, bool pandaStateBool, PandaState pandaStateLast)
    {
        Debug.Log(statePanda);
        anim.SetBool(pandaStateLast.ToString(), false);
        anim.SetBool(statePanda.ToString(), pandaStateBool);

    }
    public void playDeathAnimation(TrapType typeTrap, bool hitTrap, PandaState pandaStateLast)
    {
        anim.SetBool(pandaStateLast.ToString(), false);
        anim.SetBool(typeTrap.ToString(), hitTrap);
    }
}
