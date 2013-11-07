using UnityEngine;

public static class PandaDebug
{
    public static bool UseDebug = true;

    public static void Log(string log)
    {
        if (UseDebug)
        {
            Debug.Log(log);
        }
    }

    public static void Log(string name, UnityEngine.Object obj)
    {
        if (UseDebug)
        {
            string toPrint = obj != null ? obj.ToString() : "Null";
            Debug.Log(name + " " + toPrint);
        }
    }

    public static void Log(string name, System.Object obj)
    {
        if (UseDebug)
        {
            string toPrint = obj != null ? obj.ToString() : "Null";
            Debug.Log(name + " " + toPrint);
        }
    }
}
