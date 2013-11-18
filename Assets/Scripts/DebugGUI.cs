using UnityEngine;
using System.Collections;

public class DebugGUI : MonoBehaviour {

	
	// fps
	public float updateInterval = 0.5F;
    private double lastInterval;
    private int frames = 0;
    private double fps;
	
	Vector2 initGroupPos = new Vector2(0.27f, 0.01f); 
	
	string[] cellNames = new string[] {"Stats", "Input Handler", "3", "4", "5", "6" };
	
	
	GuiButton restartButton;
	GuiLabel fpsLabel;
	
	GuiSlider swipeThresholdSlider;
	GuiSelectionGrid selectionGrid;
	
	Vector2 buttonsize = new Vector2(0.2f, 0.06f);
	Vector2 slidersize = new Vector2(0.3f, 0.06f);
	
	
	
	public InputHandler ih;


    void Start()
    {
		lastInterval = Time.realtimeSinceStartup;
        frames = 0;
		
		selectionGrid = new GuiSelectionGrid(new Vector2(0.01f,0.01f), new Vector2(0.25f, 0.2f), cellNames, 5, 2);
		
		fpsLabel = new GuiLabel(initGroupPos, new Vector2(0.1f, 0.2f));
		
		restartButton = new GuiButton(new Vector2(0.79f, 0.02f), buttonsize);
		restartButton.buttonText = "RESTART";
		
		if(ih != null)
			swipeThresholdSlider = new GuiSlider(initGroupPos, slidersize, 0, 0.2f);
		
    }
	
	
	void Update()
	{
		if(selectionGrid.GetSelectedId()==0)
		{
			++frames;
		    float timeNow = Time.realtimeSinceStartup;
		    if (timeNow > lastInterval + updateInterval) {
				fps = frames / (timeNow - lastInterval);
		        fpsLabel.text = fps.ToString("f2") + " fps";
		        frames = 0;
		        lastInterval = timeNow;		
				if(fps < 30)
					fpsLabel.SetColor(Color.yellow);
				else if(fps < 10)
					fpsLabel.SetColor(Color.red);
				else
					fpsLabel.SetColor(Color.green);
			}
		}
	}
	
	
	DebugInputHandler d = new DebugInputHandler();
    void OnGUI()
    {
		
		
		
		
        if (restartButton.DrawButton())
        {
            Application.LoadLevel(0);
        }
		
		d.DrawGroup(initGroupPos);
		selectionGrid.DrawGrid();
		
		if(selectionGrid.GetSelectedId() == 0)
			fpsLabel.DrawLabel();
		
		if(ih != null && selectionGrid.GetSelectedId() == 1)
		{
			swipeThresholdSlider.text = "Swipe Threshold: " + ih.swipeThreshold;
			ih.swipeThreshold = swipeThresholdSlider.DrawSlider(ih.swipeThreshold);
		}
    }

}


class GuiSelectionGrid
{
	Color? textColor;
	int cellId;
	int xCount;
	
	string[] buttonNames;
	private Rect box;
	private Rect sliderBox;
		
	public GuiSelectionGrid(Vector2 Position, Vector2 Size, string[] cellNames, int initVal, int anXCount)
	{
		box = new Rect(Screen.width * Position.x, Screen.height * Position.y, Screen.width * Size.x, Screen.height * Size.y);
		buttonNames = cellNames;
		xCount = anXCount;
	}
	
	public int DrawGrid()
	{
		Color temp = GUI.contentColor;
		if(textColor!=null)
			GUI.contentColor = textColor.Value;
		cellId = GUI.SelectionGrid(box, cellId, buttonNames, xCount);
		GUI.contentColor = temp;
		return cellId;
	}
	
	public void SetColor(Color c)
	{
		textColor = c;
	}
	
	public int GetSelectedId()
	{
		return cellId;
	}
}


class GuiSlider
{
	
	
	public string text;
	Color? textColor;
	float sliderMin;
	float sliderMax;
	
	private Rect box;
	private Rect sliderBox;
		
	public GuiSlider(Vector2 Position, Vector2 Size, float min, float max)
	{
		box = new Rect(Screen.width * Position.x, Screen.height * Position.y, Screen.width * Size.x, Screen.height * Size.y);
		sliderBox = new Rect(Screen.width * Position.x + Screen.width * Size.x * 0.05f, Screen.height * Position.y + Screen.height * Size.y * 0.6f
			, Screen.width * Size.x * 0.9f, Screen.height * Size.y * 0.4f);
		sliderMin = min;
		sliderMax = max;
	}
	
	public float DrawSlider(float val)
	{
		Color temp = GUI.contentColor;
		if(textColor!=null)
			GUI.contentColor = textColor.Value;
		GUI.Box(box, text);
		GUI.contentColor = temp;
		
		return GUI.HorizontalSlider(sliderBox, val, sliderMin, sliderMax);
	}
	
	public void SetColor(Color c)
	{
		textColor = c;
	}
}


class GuiLabel
{
	
	
	public string text;
	Color? textColor;
	
	private Rect box;
		
	public GuiLabel(Vector2 Position, Vector2 Size)
	{
		box = new Rect(Screen.width * Position.x, Screen.height * Position.y, Screen.width * Size.x, Screen.height * Size.y);
	}
	
	public void DrawLabel()
	{
		Color temp = GUI.contentColor;
		if(textColor.HasValue)
			GUI.contentColor = textColor.Value;
		GUI.Label(box, text);
		GUI.contentColor = temp;
	}
	
	public void SetColor(Color c)
	{
		textColor = c;
	}
}


class GuiBox
{
	public string text = "";
	Color? textColor;
	
	private Rect box;
		
	public GuiBox(Vector2 Position, Vector2 Size)
	{
		box = new Rect(Screen.width * Position.x, Screen.height * Position.y, Screen.width * Size.x, Screen.height * Size.y);
	}
	
	public void DrawBox()
	{
		Color temp = GUI.contentColor;
		if(textColor!=null)
			GUI.contentColor = textColor.Value;
		GUI.Box(box, text);
		GUI.contentColor = temp;
	}
	
	public void SetColor(Color c)
	{
		textColor = c;
	}
}

// a basic gui button, all coordinates are screen ratios
class GuiButton
{
	private Rect buttonBox;
	public string buttonText;
	
	
	public GuiButton(Rect box)
	{
		buttonBox = box;	
	}
	
	public GuiButton(Vector2 Position, Vector2 Size)
	{
		buttonBox = new Rect(Screen.width * Position.x, Screen.height * Position.y, Screen.width * Size.x, Screen.height * Size.y);
	}
	
	public bool DrawButton()
	{
		return GUI.Button(buttonBox, buttonText);
	}

}