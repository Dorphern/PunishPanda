using UnityEngine;
using System.Collections;


public class ProgressBar : PropertyAttribute
{
    public string TextLabel;
    public float MinValue;
    public float MaxValue;
    public ProgressBar(string label, float minValue, float maxValue)
    {
        MinValue = minValue;
        MaxValue = maxValue;
        this.TextLabel = label;
    }
}