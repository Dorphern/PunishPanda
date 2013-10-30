using UnityEngine;
using System.Collections;

public enum CollidableTypes {
	
	None,
	Panda,
	DeathTrap
	
}

public class Collidable : MonoBehaviour {
	
	public CollidableTypes type;

}
