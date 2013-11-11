using UnityEngine;
using System.Collections;

public class DebugInputHandler 
{
	
	float width;
	float height;
	
	GuiBox SwipeThresholdBox;
	
	public void DrawGroup(Vector2 groupPos)//, Vector2 groupSize)
    {
		GUI.BeginGroup(new Rect(Screen.width * groupPos.x, Screen.height * groupPos.y, 300, 200));
        
		
		GUI.Box(new Rect(0, 0, 800, 600), "This box is now centered! - here you would put your main menu");
     
		
		
		
		GUI.EndGroup();
    }
}
