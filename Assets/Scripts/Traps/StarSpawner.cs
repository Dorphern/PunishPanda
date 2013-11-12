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
	
	private Queue<ThrowingStar> starsPool;
	
	void Start () 
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
	
	public void TryPandaKill(PandaAI pandaAI)
	{
		bool isPerfect = (pandaKillCount++ < maxPerfectPandaKills || maxPerfectPandaKills == -1) && isPerfectTrap;
        bool successful = PandaAttemptKill(pandaAI, isPerfect);
        if (successful) 
        {
            GivePointsForKill(isPerfect);
        }
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
		while(this.isActivated)
		{
			if(starsPool.Count < maxStarCount)
			{
				GameObject star = Instantiate(starPrefab) as GameObject;
				ThrowingStar throwingStar = star.GetComponent<ThrowingStar>();
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
		star.ShootStar(transform.forward, force, torque);
	}
}
