using InAudio.ExtensionMethods;
using InAudio.Runtime;
using UnityEngine;

public static class AudioBusVolumeHelper {
    public static void SetTargetVolume(AudioBus bus, float targetVolume, EventBusAction.VolumeSetMode setMode, float duration, FadeCurveType curveType)
    {
        if (duration == 0)
        {
            bus.Fader.Activated = false;
            bus.RuntimeSelfVolume = targetVolume;
        }
        else
        {
            if (setMode == EventBusAction.VolumeSetMode.Absolute)
            {
                bus.Fader.Activated = true;
                double currentTime = AudioSettings.dspTime;
                bus.Fader.Initialize(curveType, currentTime, currentTime + duration, bus.RuntimeSelfVolume,
                    targetVolume);
            }
            else
            {
                bus.Fader.Activated = true;
                double currentTime = AudioSettings.dspTime;
                float newVolume = Mathf.Clamp(bus.RuntimeSelfVolume + targetVolume, 0.0f, 1.0f);
                bus.Fader.Initialize(curveType, currentTime, currentTime + duration, bus.RuntimeSelfVolume,
                    newVolume);
            }
        }

    }

    public static void UpdateBusVolumes(AudioBus bus)
    {   
        
        Fader fader = bus.Fader;
        if (fader.Activated)
        {
            double currentTime = AudioSettings.dspTime;
            bus.RuntimeSelfVolume = (float)fader.Lerp(AudioSettings.dspTime);

            if (/*bus.RuntimeSelfVolume == fader.EndValue ||*/  currentTime >= fader.EndTime)
            {
                fader.Activated = false;
            }
        }

        /**/

        float parentVolume;
        if (bus.Parent != null)
        {
            parentVolume = bus.Parent.RuntimeVolume;
        }
        else
        {
            parentVolume = 1.0f;
        }
        float oldVolume = bus.RuntimeVolume;
        
        bus.RuntimeVolume = bus.Volume * bus.RuntimeSelfVolume * parentVolume;
        
        if (bus.RuntimeVolume != oldVolume)
        {
            var players = bus.RuntimePlayers;
            for (int i = 0; i < players.Count; ++i)
            {
                if (players[i] != null)
                {
                    players[i].UpdateBusVolume(bus.RuntimeVolume);
                }
                else
                {
                    players.SwapRemoveAt(i);
                }
            }
        }

        for (int i = 0; i < bus.Children.Count; ++i)
        {
            UpdateBusVolumes(bus.Children[i]);
        }
    }

    public static void InitVolumes(AudioBus bus)
    {
        bus.RuntimeSelfVolume = bus.SelfVolume;
        for (int i = 0; i < bus.Children.Count; ++i)
        {
            InitVolumes(bus.Children[i]);
        }        
    }
}
