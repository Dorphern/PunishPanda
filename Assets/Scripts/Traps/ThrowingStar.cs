using System.Collections.Generic;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class ThrowingStar : MonoBehaviour 
{
    [SerializeField] protected Texture cleanTexture;
    [SerializeField] protected Texture dirtyTexture;
	[SerializeField] protected ParticleSystem bloodParticles;

	[System.NonSerialized] public StarSpawner starSpawner;
    private bool isActive = true;

    [SerializeField]
    [EventHookAttribute("On Alive Panda Collide")]
    private List<AudioEvent> onAliveEvents = new List<AudioEvent>();

    [SerializeField]
    [EventHookAttribute("On Dead Panda Collide")]
    private List<AudioEvent> onDeadEvents = new List<AudioEvent>();

    [SerializeField]
    [EventHookAttribute("On Spawn Collide")]
    private List<AudioEvent> onSpawnCollision = new List<AudioEvent>();

    [SerializeField]
    [EventHookAttribute("On Finger Collide")]
    private List<AudioEvent> onFingerEvents = new List<AudioEvent>();

    [SerializeField]
    [EventHookAttribute("On Wall Collide")]
    private List<AudioEvent> onWallCollision = new List<AudioEvent>();

    [SerializeField]
    [EventHookAttribute("On Floor Collide")]
    private List<AudioEvent> onFloorCollision = new List<AudioEvent>();

    void Start()
    {
        HDRSystem.PostEvents(gameObject, onSpawnCollision);
    }


    public void SetDirty ()
    {
        if (dirtyTexture != null) renderer.material.mainTexture = dirtyTexture;
    }

    public void SetClean ()
    {
        if (cleanTexture != null) renderer.material.mainTexture = cleanTexture;
    }
	
	public void ShootStar(Vector3 direction, float force, float torque) 
	{
		isActive = true;
		rigidbody.useGravity = false;
		rigidbody.isKinematic = false;
		rigidbody.AddForce(direction * force, ForceMode.Impulse);
		rigidbody.AddTorque( - Vector3.forward * torque, ForceMode.Impulse);
	}
	
	protected void OnTriggerEnter (Collider collider)
    {
		if(collider.tag == "FingerBlockade" && isActive == true)
		{
			this.isActive = false;
			rigidbody.velocity = Vector3.zero;
			rigidbody.useGravity = true;
            HDRSystem.PostEvents(gameObject, onFingerEvents);
		}
		
        Collidable collidable = collider.GetComponent<Collidable>();
		if (collidable == null) return;
		
        if (collidable.type == CollidableTypes.Panda && isActive == true)
        {
            var pandaAi = collider.GetComponent<PandaAI>();
            if(pandaAi.IsAlive())
                HDRSystem.PostEvents(gameObject, onAliveEvents);
            else
                HDRSystem.PostEvents(gameObject, onDeadEvents);
            if (starSpawner.TryPandaKill(pandaAi))
            {
				bloodParticles.transform.localRotation = Quaternion.LookRotation( new Vector3(pandaAi.GetPandaFacingDirection().x, 0f, 0f));
				bloodParticles.Play();
                SetDirty();
            }
        }
		else if(collidable.type == CollidableTypes.Wall)
		{
            HDRSystem.PostEvents(gameObject, onWallCollision);
			this.isActive = false;
			rigidbody.isKinematic = true;
		}
		else if(collidable.type == CollidableTypes.ThrowingStar)
		{
			if(collider.GetComponent<ThrowingStar>().isActive == true)
			{
				// pool objects instead of destroying them
				// remove object in a coroutine after a short amount
				//starSpawner.ReuseThrowingStar(collider.GetComponent<ThrowingStar>());
                
				this.enabled = false;
				renderer.enabled = false;
			}
		}
		else if(collidable.type == CollidableTypes.Floor)
		{
            HDRSystem.PostEvents(gameObject, onFloorCollision);
			this.isActive = false;
			rigidbody.isKinematic = true;
			GameObject emptyObject = new GameObject();
			emptyObject.transform.parent = collider.transform;
			this.transform.parent = emptyObject.transform;
		}
    }
}
