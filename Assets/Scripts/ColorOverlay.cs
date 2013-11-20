/*	Filter to add a tint on the screen
*/

using UnityEngine;
using System.Collections;

public class ColorOverlay : MonoBehaviour
{

    GUIStyle overlay;
    public Color clr;
    public bool ismegaactiv = true;

    // Use this for initialization
    void Start ()
    {
        Texture2D text = new Texture2D(1, 1);
        // Yellow
        text.SetPixel(0, 0, new Color(1f, 0.8f, 0f, 0.02f));

        // original blue
        //text.SetPixel(0, 0, new Color(0f, 0f, 1f, 0.02f));

        // blue
        //text.SetPixel(0, 0, new Color(0f, 0.5f, 1f, 0.015f));

        // greenish
        //text.SetPixel(0, 0, new Color(0f, 1f, 1f, 0.02f));

        //text.SetPixel(0, 0, clr);

        text.wrapMode = TextureWrapMode.Repeat;
        text.Apply();
        overlay = new GUIStyle();
        overlay.normal.background = text;
    }

    // Update is called once per frame
    void Update ()
    {

    }

    void OnGUI ()
    {
        if (ismegaactiv)
            GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "", overlay);
    }
}
