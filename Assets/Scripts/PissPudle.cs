using UnityEngine;
using System.Collections;

public class PissPudle : MonoBehaviour {
	
	//vars for the whole sheet
	public int colCount =  4;
	public int rowCount =  4;
	 
	//vars for animation
	public int rowNumber =  0; //Zero Indexed
	public int colNumber =  0; //Zero Indexed
	public int totalCells =  4;
	
	Vector2 offset;
	
	//SetSpriteAnimation
	public void SetSpriteCell(int index){
	 
		
		// Size of every cell
		Vector2 size =  new Vector2 (1.0f / colCount, 1.0f / rowCount);
	 
		// split into horizontal and vertical index
		var uIndex = index % colCount;
		var vIndex = index / colCount;
	 
		// build offset
		offset = new Vector2 ((float)(uIndex+colNumber) * size.x, (vIndex+rowNumber) * size.y);
	 
		renderer.material.SetTextureOffset ("_MainTex", offset);
		renderer.material.SetTextureScale  ("_MainTex", size);
	}
}
