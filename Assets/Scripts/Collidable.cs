using UnityEngine;
using System.Collections;

public enum CollidableTypes {
	
	None                = 0,
	Panda               = 1,
	DeathTrap           = 2,
	Wall                = 3,
    Hotspot             = 4,
	Floor               = 5,
	ThrowingStar        = 6,
    LedgeFall           = 7
	
}

public class Collidable : MonoBehaviour {
	
	public CollidableTypes type;

}
