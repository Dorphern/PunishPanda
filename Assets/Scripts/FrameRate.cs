using UnityEngine;
using System.Collections;

public class FrameRate : MonoBehaviour {

    GUIStyle fpsStyle;

    public float updateInterval = 0.5F;

    private float accum = 0; // FPS accumulated over the interval
    private int frames = 0; // Frames drawn over the interval
    private float timeleft; // Left time for current interval

    string format;


    void Start ()
    {

        fpsStyle = new GUIStyle();
        fpsStyle.normal.textColor = Color.black;
        timeleft = updateInterval;
    }

    void Update ()
    {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;

        // Interval ended - update GUI text and start new interval
        if (timeleft <= 0.0)
        {
            // display two fractional digits (f2 format)
            float fps = accum / frames;
            format = System.String.Format("{0:F2} FPS", fps);

            //	DebugConsole.Log(format,level);
            timeleft = updateInterval;
            accum = 0.0F;
            frames = 0;
        }
    }

    void OnGUI ()
    {


        GUI.Label(new Rect(10f, 10f, 100f, 20f), format, fpsStyle);
    }
}
