using UnityEngine;
using System.Collections;

// Require a character controller to be attached to the same game object
[RequireComponent(typeof(CharacterController))]
public class PandaMovementController : MonoBehaviour {

    [SerializeField] Transform spawnPoint; // The character will spawn here
    [SerializeField] Movement movement;
    [SerializeField] Boosting boosting;
    [SerializeField] JumpingOff jumpOff;
    [SerializeField] Falling falling;
    [SerializeField] Escape escape;
    [SerializeField] float hangingOffSet = 30f;
    [SerializeField] float pushingForce = 15f;
	private float currentPushingMagnitude = 0f;
	
	private CharacterController controller;
	private PandaAI pandaAI;
    private Animations animations;
    private PandaStateManager pandaStateManager;
	Vector3 lastPos;
    Vector3 dampedVelocity;
    private float escapeSlideSpeed = 0f;
	
	bool withinRange = false;

    [SerializeField] float velocityDampingSpeed = 0.1f;
    [SerializeField] float velocityRotation = 3f;
 
    #region SerializedClasses
	[System.Serializable]
	class Boosting
	{
		public float boostSpeed = 5f;
		public float rollOffSpeed = 1.5f;
	}
	
	[System.Serializable]
	class JumpingOff
	{
		public float jumpOffSpeed = 3f;
		public float jumpOffDir = 45f;	
	}
	
	[System.Serializable]
	class Movement 
	{	 
	    public float gravity = 20;
	    public float jumpHeight = 8;
		public float walkSpeed = 3;
		[System.NonSerializedAttribute]
	    public float currentSpeed;
	    // The character's current movement offset (for Jumping / Falling)
	    [System.NonSerialized]
	    public Vector3 offset;
		public float notMovingThreshold = 0.01f;
	}

    [System.Serializable]
    class Falling
    {
        public float velocityThreshold = 1f; /* At what speed are we actually falling ? */
        public float hardLandingThreshold = 10f;
    }

    [System.Serializable]
    public class Escape
    {
        public bool pandaEscapedSlide;
        public bool pandaEscapeCrawl;
        public CollidableTypes bambooDirection;
        public float bambooSlideAngle = 4f;
        public bool pandaJumpToBambooDown;
        public bool pandaJumpToBambooUp;
        public Transform bambooPosition;
        public float slideDistance;
        public Vector3 yVelocity = Vector3.zero;
        public float smoothTime = 0.1f;
        public Vector3 targetJumpPos;

    }
	

	#endregion

    # region Public Methods
	
	public bool IsNotMoving ()
	{
		Vector3 diff = lastPos - transform.position;
		if(diff.magnitude < movement.notMovingThreshold)
			return true;
		return false;	
	}
	
	public void JumpOff ()
	{
		ApplyJump(jumpOff.jumpOffSpeed, jumpOff.jumpOffDir);	
	}
	
	public void ResetGravity()
	{
		movement.offset.y = 0;
	}

    public void ApplyJump (float force, float direction)
    {
        float radDir = Mathf.Deg2Rad * direction;
        movement.offset.y = Mathf.Sin(radDir) * force;
        movement.offset.x = Mathf.Cos(radDir) * force;
    }

    public void SetVelocity (float x, float y)
    {
        movement.offset.x = x;
        movement.offset.y = y;
    }

    public bool IsGrounded ()
    {
        return controller.isGrounded;
    }

    public void PandaEscapeJump (Collider c, CollidableTypes bambooDirection)
    {
       escape.bambooDirection = bambooDirection;
       escape.bambooPosition = c.transform;
       StartCoroutine(JumpToBamboo(0.7f, 0.3f, escape.bambooPosition, bambooDirection));
       escape.targetJumpPos = new Vector3(escape.bambooPosition.position.x + 0.3f, transform.position.y + 1f, -1f);
        
    }
    
    public void PandaEscapeAway ()
    {
        Debug.Log(escape.bambooDirection);
        if (escape.bambooDirection == CollidableTypes.BambooEscapeDown)
            escape.pandaEscapedSlide = true;
        else
            escape.pandaEscapeCrawl = true;
    }

    public void SetDirection (PandaDirection dir)
    {
        Vector3 curr = transform.eulerAngles;
        float goalY = (dir == PandaDirection.Right ? 0 : 180);
        curr.y = goalY;
        transform.eulerAngles = curr;
    }
    # endregion

    # region Private Methods
    void Start()
	{
	    controller = GetComponent<CharacterController>();
		pandaAI = GetComponent<PandaAI>();
        animations = GetComponent<Animations>();
        pandaStateManager = GetComponent<PandaStateManager>();
		
		movement.currentSpeed = movement.walkSpeed;
		
		if(pandaAI != null)
		{
			pandaAI.ApplyWalkingMovement += WalkingMovement;
			pandaAI.PushingMovement += PushingMovement;
            pandaAI.BoostingMovement += BoostedMovement;
			pandaAI.SetBoostSpeed += SetBoostSpeed;
	        pandaAI.ApplyJump += ApplyJump;
	        pandaAI.ApplyGravity += ApplyGravity;
			pandaAI.ApplyIdle += Idle;
		}
	}
	 
	void FixedUpdate ()
	{
	    // Make sure the character stays in the 2D plane
        //if (pandaStateManager.GetState() != PandaState.Escape)
        //{
        //    transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
        //}
        //else
        if (pandaStateManager.GetState() != PandaState.Escape)
        {
            return;
            transform.position = new Vector3(transform.position.x, transform.position.y, -1f);
        }

        dampedVelocity = dampedVelocity * 0.9f + controller.velocity * 0.1f;
       
        if (IsGrounded() == false && dampedVelocity.y < - falling.velocityThreshold)
        {
            pandaAI.landingHard = dampedVelocity.y < -falling.hardLandingThreshold;
            pandaAI.Falling();
        }

        // Store the last position of the character;
        lastPos = transform.position;


                if (escape.pandaEscapedSlide)
        {
            escape.pandaJumpToBambooDown = false;
         //   transform.position = Vector3.SmoothDamp(transform.position, new Vector3(escape.bambooPosition.position.x + 0.3f, transform.position.y - 0.3f, -1), ref escape.yVelocity, escape.smoothTime, 2f, Time.deltaTime);
        }
        else if(escape.pandaEscapeCrawl)
        {
            escape.pandaJumpToBambooUp = false;
           // transform.position = Vector3.SmoothDamp(transform.position, new Vector3(transform.position.x - 0.4f, transform.position.y + 0.3f, -1f), ref escape.yVelocity, escape.smoothTime, 3f, Time.deltaTime);
        }

        if (escape.pandaJumpToBambooDown)
        {
            if (pandaStateManager.GetDirection() == PandaDirection.Left)
            {                
                transform.position = Vector3.Lerp(transform.position, new Vector3(escape.bambooPosition.position.x, transform.position.y, -1f), 10f * Time.deltaTime);
            }                
            else                
            {
                //transform.position = Vector3.Lerp(transform.position, escape.targetJumpPos, 5f * Time.deltaTime);
                transform.position = Vector3.Lerp(transform.position, new Vector3(escape.bambooPosition.position.x, transform.position.y, -0.5f), 10f * Time.deltaTime);
                //transform.position = Vector3.SmoothDamp(transform.position, new Vector3(escape.bambooPosition.position.x - 0.3f, escape.bambooPosition.position.y - 0.3f, -1f), ref escape.yVelocity, escape.smoothTime, 3f, Time.deltaTime);
            }
        }
        if (escape.pandaJumpToBambooUp)
        {
            if (pandaStateManager.GetDirection() == PandaDirection.Left)
            {
               // transform.position = Vector3.Lerp(transform.position, new Vector3(escape.bambooPosition.position.x, transform.position.y + 0.3f, -1f), 8f * Time.deltaTime);
            }  
            else
            {
                //transform.position = Vector3.Lerp(transform.position, new Vector3(escape.bambooPosition.position.x, transform.position.y + 0.3f, -1f), 5f * Time.deltaTime);
            }

        }
	}

    void Update ()
    {


    }
	
	void PushingMovement(PandaDirection direction, float pushingMagnitude, float lastMag)
	{
		currentPushingMagnitude = Mathf.Lerp(lastMag, pushingMagnitude, Time.fixedDeltaTime * 100);
		
		if(controller.isGrounded)
		{
			movement.offset = Vector3.zero;
		}
		// in order for the isGrounded flag to work we always need to apply gravity
		movement.offset.y -= movement.gravity * Time.fixedDeltaTime;
		
		if(direction == PandaDirection.Right)
		{	
			movement.offset.x = movement.currentSpeed * currentPushingMagnitude * pushingForce;
			transform.rotation = Quaternion.LookRotation(Vector3.forward);
		}
		
		if(direction == PandaDirection.Left)
		{
			movement.offset.x = - movement.currentSpeed * currentPushingMagnitude * pushingForce;
			transform.rotation = Quaternion.LookRotation(Vector3.back);
		}
		
		controller.Move(movement.offset * Time.fixedDeltaTime);
	}
	
	void Idle()
	{
		if(controller.isGrounded)
		{
			movement.offset = Vector3.zero;
		}
		// in order for the isGrounded flag to work we always need to apply gravity
		movement.offset.y -= movement.gravity * Time.fixedDeltaTime;	
		controller.Move(movement.offset * Time.fixedDeltaTime);
	}
	
	// Move the character using Unity's CharacterController.Move function
    void WalkingMovement (PandaDirection dir)
	{
		if(controller.isGrounded)
		{
			movement.offset = Vector3.zero;
		}
		// in order for the isGrounded flag to work we always need to apply gravity
		movement.offset.y -= movement.gravity * Time.fixedDeltaTime;
		
        UpdateDirection(dir);

        // Determine movement direction based on movement dir
        movement.offset.x = movement.currentSpeed * (dir == PandaDirection.Left ? -1 : 1);
		
		// CharacterController.Move() should only be called once per frame
		controller.Move(movement.offset * Time.fixedDeltaTime);
	}
	
	public void UpdateDirection (PandaDirection dir) {
		Vector3 curr = transform.eulerAngles;
        float goalY = (dir == PandaDirection.Right ? 0 : 180);
        if ((int) curr.y != (int) goalY)
        {
            curr.y += (Time.fixedDeltaTime / pandaAI.turnSpeed) * 180 * (goalY < curr.y ? -1 : 1);
            curr.y = Mathf.Clamp(curr.y, 0f, 180f);
            transform.eulerAngles = curr;
        }	
	}

	#region JumpingCode (NOT IN USE)
	void  HandleJump ()
	{
	    switch (controller.isGrounded) 
		{
	        // The character is on the ground
	        case true:
	            if (Input.GetButtonDown("Jump"))
				{ 
					//ApplyJump();
				}
				else
				{
					movement.offset = Vector3.zero;
				}
	        break;
	 
	        // The character is midair
	        case false:
	            //DO NOTHING (i.e. dont keep applying jump)
	        break;
	    }
	 
	}
	
    #endregion
    
    void ApplyJump ()
    {
        movement.offset.y = movement.jumpHeight;
    }
	
	void Spawn()
	{
	    transform.position = spawnPoint.position;
	}
	
	void  ApplyGravity()
	{
	    movement.offset.y -= movement.gravity * Time.fixedDeltaTime;
		//USING "OFFSET" FOR APPLYING GRAVITY
		controller.Move(movement.offset * Time.fixedDeltaTime);
	}

	void BoostedMovement(PandaDirection dir)
	{
		//movement.currentSpeed = Mathf.Lerp(movement.currentSpeed, movement.walkSpeed, Time.fixedDeltaTime * boosting.rollOffSpeed);
		if(controller.isGrounded)
		{
			movement.offset = Vector3.zero;
		}
		// in order for the isGrounded flag to work we always need to apply gravity
		movement.offset.y -= movement.gravity * Time.fixedDeltaTime;
		
        Vector3 curr = transform.eulerAngles;
        float goalY = (dir == PandaDirection.Right ? 0 : 180);
        if ((int) curr.y != (int) goalY)
        {
            curr.y += (Time.fixedDeltaTime / pandaAI.turnSpeed) * 180 * (goalY < curr.y ? -1 : 1);
            curr.y = Mathf.Clamp(curr.y, 0f, 180f);
            transform.eulerAngles = curr;
        }

        // Determine movement direction based on movement dir
        movement.offset.x = boosting.boostSpeed * (dir == PandaDirection.Left ? -1 : 1);
		
		// CharacterController.Move() should only be called once per frame
		controller.Move(movement.offset * Time.fixedDeltaTime);
	}
	
	void SetBoostSpeed()
	{
		movement.currentSpeed = boosting.boostSpeed;
    }

    private IEnumerator JumpToBamboo(float timeToWaitUp, float timeToWaitDown, Transform c, CollidableTypes bambooDirection)
    {
        
        float timeToWait;

        if (CollidableTypes.BambooEscapeDown == bambooDirection)
            timeToWait = timeToWaitDown;
        else
            timeToWait = timeToWaitUp;

        yield return new WaitForSeconds(timeToWait);

        if (CollidableTypes.BambooEscapeDown == bambooDirection)
        {
            escape.pandaJumpToBambooDown = true;
        }
        else
        {
            escape.pandaJumpToBambooUp = true;
        }
            

    }
    # endregion

}