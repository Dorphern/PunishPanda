using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class ThrowingStar : MonoBehaviour 
{
    [SerializeField] protected Texture cleanTexture;
    [SerializeField] protected Texture dirtyTexture;

	[System.NonSerialized] public StarSpawner starSpawner;
    private bool isActive = true;

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
		}
		
        Collidable collidable = collider.GetComponent<Collidable>();
		if (collidable == null) return;
		
        if (collidable.type == CollidableTypes.Panda && isActive == true)
        {
            if (starSpawner.TryPandaKill(collider.GetComponent<PandaAI>()))
            {
                SetDirty();
            }
        }
		else if(collidable.type == CollidableTypes.Wall)
		{
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
			this.isActive = false;
			rigidbody.isKinematic = true;
			GameObject emptyObject = new GameObject();
			emptyObject.transform.parent = collider.transform;
			this.transform.parent = emptyObject.transform;
		}
    }
}
