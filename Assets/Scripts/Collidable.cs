using UnityEngine;
using System.Collections;

public enum CollidableTypes {
	
	None,
	Panda,
	DeathTrap,
	Wall,
    Hotspot,
	Floor,
	ThrowingStar,
    LedgeFall,
    BambooEscapeDown,
    BambooEscapeUp
	
}

public class Collidable : MonoBehaviour {
	
	public CollidableTypes type;

}
