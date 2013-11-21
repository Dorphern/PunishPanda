using UnityEngine;
using System.Collections;

public class Lever : TrapActivator {

    [SerializeField] bool isActive = false;
    private string animationName = "lever";

	private Collidable colliderType;

    void Start ()
    {
        if (isActive)
        {
            ActivateTraps();
        }
    }

    protected override void ActivateTraps ()
    {
        base.ActivateTraps();
        StartCoroutine(PlayActivateAnimation());
    }

    protected override void DeactivateTraps ()
    {
        base.DeactivateTraps();
        PlayDeactiveAnimation();
    }
	
	void OnTriggerEnter(Collider collider)
	{
        Debug.Log("enter!");
		colliderType = collider.GetComponent<Collidable>();
		if(colliderType == null) return;
		
		if(colliderType.type == CollidableTypes.Panda)
		{
            if (isActive)
            {
                DeactivateTraps();
            }
            else
            {
                ActivateTraps();
            }

            isActive = !isActive;
		}
    }

    IEnumerator PlayActivateAnimation ()
    {
        Debug.Log("activate anim");
        animation.Play(animationName);
        yield return new WaitForSeconds(0.2f);
        animation[animationName].speed = 0;
    }

    void PlayDeactiveAnimation ()
    {
        Debug.Log("deactivate anim");
        animation[animationName].speed = 1;
    }
}
