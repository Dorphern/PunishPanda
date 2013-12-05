using UnityEngine;
using System.Collections;

public class PissParticles : MonoBehaviour {
	
	public ParticleSystem piss;
	public ParticleSystem subPiss;
//	public PissPudle pissPudle;
//	public MeshRenderer puddleMesh;
	
	bool peeing = false;
	private Quaternion rightEmission;
	private Quaternion leftEmission;
	float initGravity;
    float initLife;

	
	void Start()
	{
		initGravity = piss.gravityModifier;
        initLife = piss.startLifetime;
	}
	
	public void PissFor(float duration)
	{
		//adjust direction based on state:

		
		if(!peeing)
		{
			peeing = true;
			StartCoroutine("Pee", duration);
		}
	}
	
	public void InterruptPiss()
	{
		StopCoroutine("Pee");
		piss.Stop ();
		//puddleMesh.enabled = false;
		peeing = false;
		piss.gravityModifier = initGravity;
		//gameObject.SetActive(false);
	}
	
	IEnumerator Pee(float duration)
	{
		
		
		if(piss!=null)
			piss.Play();
		
		
		yield return new WaitForSeconds(2.8f);
		
		float d = 1f;
        int steps = 10;
        float gravTarget = 0.8f / steps;
        float lifeTarget = 0.5f / steps;

        for (int i = 0; i < steps; i++)
		{
            piss.gravityModifier += gravTarget;
            piss.startLifetime -= lifeTarget;
            yield return new WaitForSeconds(d / steps);
		}
		
		piss.gravityModifier = -1.5f;
		yield return new WaitForSeconds(0.2f);
		
		if(piss!=null)
		{
			piss.Stop();
			piss.gravityModifier = initGravity;
            piss.startLifetime = initLife;
		}
		peeing = false;
	}

}
