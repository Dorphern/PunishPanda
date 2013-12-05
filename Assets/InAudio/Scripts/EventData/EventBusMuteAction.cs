using InAudio.Runtime;

public class EventBusMuteAction : AudioEventAction
{
    public AudioBus Bus;

    public enum MuteAction
    {
        Mute,
        Unmute
    }

    public MuteAction Action;

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
