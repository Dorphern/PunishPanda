using UnityEngine;
using System.Collections.Generic;

namespace HDRAudio.Runtime
{
    public enum CurveType
    {
        Lerp,
        SLerp
    }

    public class Fader
    {
        public CurveType CurveType;
        public double StartTime;
        public double EndTime;

        public double EndValue;
        public double StartValue;

        public double LastTime;
        public double CurrentTime;

        public double Duration;
        
        
        public void Start(CurveType type, double startTime, float endTime)
        {
            Duration = endTime - startTime;
            CurveType = type;
            StartTime = startTime;
            EndTime = endTime;
        }

        void Update()
        {
            
            CurrentTime += LastTime - StartTime;
        }

        /*public float Evaluate()
        {
            Lerp(StartValue, EndValue, Duration / (EndTime - StartTime));
            LastTime = AudioSettings.dspTime;
        }*/

        public static double Lerp(double from, double to, double value)
        {
            if (value < 0.0f)
                return from;
            else if (value > 1.0f)
                return to;
            return (to - from) * value + from;
        }
        
    }
}
