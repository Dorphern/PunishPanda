using UnityEngine;
using System.Collections;

public class PissParticles : MonoBehaviour {
	
	public ParticleSystem piss;
	public ParticleSystem subPiss;
	public PissPudle pissPudle;
	public MeshRenderer puddleMesh;
	
	bool peeing = false;
	
	public PandaStateManager pandaStateScript;
	private Quaternion rightEmission;
	private Quaternion leftEmission;
	
	
	//TO DO: 
	//Get pandastate so we adjust rotation of pissMachine
	// when panda is idle facing LEFT.
	
	//assuming no puddle is the first state;
	int pissPudleTransitions = 4; 
	
	public void PissFor(float duration)
	{
		//adjust direction based on state:
		if(pandaStateScript.GetDirection() == PandaDirection.Left)
		{
			Debug.Log ("facingleft");
			transform.rotation = leftEmission;
		}
		else
		{
			transform.rotation = rightEmission;
		}

		
		if(!peeing)
		{
			peeing = true;
			StartCoroutine("Pee", duration);
			
			while(peeing == true)
			{
				if(pandaStateScript.GetState() != PandaState.Idle)
				{
					Debug.Log ("INTERRUPTED PISSinWhile");
					InterruptPiss();
				}
			}
		}
	}
	
	public void InterruptPiss()
	{
		StopCoroutine("Pee");
		piss.Stop ();
		puddleMesh.enabled = false;
		peeing = false;
		gameObject.SetActive(false);
			
		
		
//		if(peeing)
//		{
//			Debug.Log ("stoppingCourutine?");
//			StopCoroutine("Pee");
//			puddleMesh.enabled = false;
//		}
	}
	
	IEnumerator Pee(float duration)
	{
		
		
		if(piss!=null)
			piss.Play();
		

		
		for(int i=0; i<pissPudleTransitions; i++)
		{

			
			yield return new WaitForSeconds(duration/pissPudleTransitions);
			pissPudle.SetSpriteCell(i);
			puddleMesh.enabled = true;
		}
		
		yield return new WaitForSeconds(duration/pissPudleTransitions);
		
		
		
		if(piss!=null)
			piss.Stop();
		
		yield return new WaitForSeconds(1f);
		
		//add extra delay for puddle
		puddleMesh.enabled = false;
		peeing = false;
	}
	
	void Start()
	{
		rightEmission = transform.rotation;
		leftEmission = Quaternion.Euler(0, 135, 0);	

		
	}
}
