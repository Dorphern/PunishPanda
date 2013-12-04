using UnityEngine;
using System.Collections;

public class PissParticles : MonoBehaviour {
	
	public ParticleSystem piss;
	public ParticleSystem subPiss;
	public PissPudle pissPudle;
	public MeshRenderer puddleMesh;
	
	bool peeing = false;
	
	//assuming no puddle is the first state;
	int pissPudleTransitions = 4; 
	
	public void PissFor(float duration)
	{
		if(!peeing)
		{
			peeing = true;
			StartCoroutine("Pee", duration);
		}
	}
	
	public void InterruptPiss()
	{
		if(peeing)
		{
			StopCoroutine("Pee");
			puddleMesh.enabled = false;
		}
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
			
		PissFor(5f);
	}
}
