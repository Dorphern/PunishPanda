using InAudio.Runtime;

public class EventBusAction : AudioEventAction
{
    public AudioBus Bus;
    public float Volume = 1.0f;
    public VolumeSetMode VolumeMode = VolumeSetMode.Absolute;

    public FadeCurveType FadeCurve = FadeCurveType.SmoothLerp;
    public float Duration;

    public enum VolumeSetMode
    {
        Absolute, Relative
    }

    public override string ObjectName
    {
        get
        {
            if (Bus != null)
                return Bus.GetName;
            else
            {
                return "Missing Bus";
            }
        }
    }
}
