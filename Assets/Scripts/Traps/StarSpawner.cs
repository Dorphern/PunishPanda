using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StarSpawner : TrapBase
{
	public float force = 10f;
	public float torque = 10f;
	public float spawnInterval = 2f;
	public int maxStarCount = 5;
	public GameObject starPrefab;
    [SerializeField] float angle;

    public bool SlicePandaInHalf;

    public float Angle
    {
        get { return angle; }
    }
	
	private Queue<ThrowingStar> starsPool;
	
	void Awake () 
	{
		starsPool = new Queue<ThrowingStar>();
	}
	
	public override TrapType GetTrapType ()
	{
		return  TrapType.ThrowingStars;
	}
	
	protected override bool PandaAttemptKill (PandaAI pandaAI, bool isPerfect)
	{
        return pandaAI.AttemptDeathTrapKill(this, isPerfect);	
	}
	
	public override void ActivateTrap ()
	{
		base.ActivateTrap ();
		StartCoroutine(SpawnStars());
	}
	
	public override void DeactivateTrap()
	{
		base.DeactivateTrap();
	}
	
	IEnumerator SpawnStars()
	{
		while(collider.enabled)
		{
			if(starsPool.Count < maxStarCount)
			{
				GameObject star = Instantiate(starPrefab) as GameObject;
				ThrowingStar throwingStar = star.GetComponent<ThrowingStar>();
			    throwingStar.SlicePandaInHalf = SlicePandaInHalf;
				ActivateThrowingStar(throwingStar);	
				starsPool.Enqueue(throwingStar);
			}
			else
			{
				ThrowingStar throwingStar = starsPool.Dequeue();
				ActivateThrowingStar(throwingStar);
				starsPool.Enqueue(throwingStar);
			}
			yield return new WaitForSeconds(spawnInterval);
		}
	}
	
	private void ActivateThrowingStar(ThrowingStar star)
	{
		star.enabled = true;
		star.renderer.enabled = true;
		star.transform.position = transform.position;
		star.starSpawner = this;
        Vector3 dir = new Vector3(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad), 
            0
        );
        star.Activated();
        star.ShootStar(dir, force, torque);
	}
}
