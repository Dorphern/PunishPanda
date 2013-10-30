using UnityEngine;
using System.Collections;

public enum CollidableTypes {
	
	None,
	Panda,
	Wall,
	
	//startTraps after 100 to give space for other tags
	
	startTraps = 100
	
}

public class Collidable : MonoBehaviour {
	
	public CollidableTypes type;

}
