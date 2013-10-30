using UnityEngine;
using System.Collections;

public enum CollidableTypes {
	
	None,
	Panda,
	Wall
	
}

public class Collidable : MonoBehaviour {
	
	public CollidableTypes type;

}
