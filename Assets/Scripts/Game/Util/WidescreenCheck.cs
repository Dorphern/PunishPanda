using UnityEngine;
using System.Collections;

public static class WidescreenCheck {

    public static bool IsWideScreen()
    {
        Debug.Log(iPhone.generation);
        if (!isPhone4() && Screen.width / (float)Screen.height > 1.4)
            return true;
        else
            return false;
    }

    private static bool isPhone4()
    {
        return iPhone.generation == iPhoneGeneration.iPhone4 || iPhone.generation == iPhoneGeneration.iPhone4S;
    }
}
