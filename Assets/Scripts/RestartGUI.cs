using UnityEngine;
using System.Collections;

public class RestartGUI : MonoBehaviour {

    private float restartLevelX = 0.01f;
    private float restartLevelY = 0.01f;
    private float restartLevelWidth = 0.2f;
    private float restartLevelHeight = 0.06f;
	
	private float bboxX = 0.01f;
    private float bboxY = 0.13f;
    private float bboxWidth = 0.27f;
    private float bboxHeight = 0.06f;
	private float sliderX = 0.03f;
	private float sliderY = 0.03f;
	private float sliderHeight = 0.03f;
	private float sliderWidth = 0.9f;
	
	private float bbox2X = 0.01f;
    private float bbox2Y = 0.20f;
    private float bbox2Width = 0.27f;
    private float bbox2Height = 0.06f;
	private float slider2X = 0.03f;
	private float slider2Y = 0.03f;
	private float slider2Height = 0.03f;
	private float slider2Width = 0.9f;
	
	
	public InputHandler ih;
	float maxSliderVal = 0.2F;
	float minSliderVal = 0F;
	
	float maxSlider2Val = 1F;
	float minSlider2Val = 0F;

    void Start()
    {

    }
    void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width * restartLevelX, Screen.height * restartLevelY, Screen.width * restartLevelWidth, Screen.height * restartLevelHeight), "RESTART"))
        {
            
            Application.LoadLevel(0);
        }
		
		
		if(ih != null)
		{
			GUI.Box(new Rect(Screen.width * bboxX, Screen.height * bboxY, Screen.width * bboxWidth, Screen.height * bboxHeight), "Swipe Threshold: " + ih.swipeThreshold);
			
			ih.swipeThreshold = GUI.HorizontalSlider(new Rect(Screen.width * (bboxX + sliderX), Screen.height * (bboxY + sliderY), Screen.width * (sliderWidth), Screen.height * sliderHeight), ih.swipeThreshold, minSliderVal, maxSliderVal);
		}
		
		
		if(ih != null)
		{
			GUI.Box(new Rect(Screen.width * bbox2X, Screen.height * bbox2Y, Screen.width * bbox2Width, Screen.height * bbox2Height), "Dragging minimum Threshold: " + ih.draggingBoxMaximumThreshold);
			
			ih.draggingBoxMaximumThreshold = GUI.HorizontalSlider(new Rect(Screen.width * (bbox2X + slider2X), Screen.height * (bbox2Y + slider2Y), Screen.width * (slider2Width), Screen.height * slider2Height), ih.draggingBoxMaximumThreshold, minSlider2Val, maxSlider2Val);
		}
		
    }

}
