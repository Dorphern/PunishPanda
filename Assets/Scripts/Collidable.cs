using UnityEngine;
using System.Collections;

public enum CollidableTypes {
	
	None,
	Panda,
	DeathTrap,
	Wall,
    Hotspot,
	Floor,
	ThrowingStar
	
}

public class Collidable : MonoBehaviour {
	
	public CollidableTypes type;

}
