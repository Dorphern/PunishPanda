using UnityEngine;
using System.Collections.Generic;

namespace InAudio.Runtime
{
    public enum FadeCurveType
    {
        Lerp,
        SmoothLerp
    }

    public class Fader
    {
        public FadeCurveType FadeCurveType;
        public double StartTime;
        public double EndTime;

        public double EndValue;
        public double StartValue;

        public bool Activated;

        private double duration;
        public double Duration
        {
            get
            {
                return duration;
            }
        }
        
        public void Initialize(FadeCurveType type, double startTime, double endTime, double startValue, double endValue)
        {
            Activated = true;
            duration = endTime - startTime;
            FadeCurveType = type;
            StartTime = startTime;
            EndTime = endTime;

            StartValue = startValue;
            EndValue = endValue;
        }

        public double Lerp(double currentTime)
        {
            double t = 1-(EndTime - currentTime)/Duration;
            if (FadeCurveType == FadeCurveType.Lerp)
            {
                if (t < 0.0f)
                    return StartValue;
                else if (t > 1.0f)
                    return EndValue;

                return (StartValue + t*(EndValue - StartValue));
            }
            else
            {   
                if (t < 0.0f) 
                    return StartValue;
                else if (t > 1.0f)
                    return EndValue;

                float ft = Mathf.SmoothStep(0.0f, 1.0f, (float)t);
                return (StartValue + ft * (EndValue - StartValue));               
            }
        }
        
    }
}
