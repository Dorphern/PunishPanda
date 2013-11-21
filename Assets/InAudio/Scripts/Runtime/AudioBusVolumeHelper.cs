using InAudio.ExtensionMethods;
using InAudio.Runtime;
using UnityEngine;

public static class AudioBusVolumeHelper {
    public static void SetTargetVolume(AudioBus bus, float targetVolume, EventBusAction.VolumeSetMode setMode, float duration, FadeCurveType curveType)
    {
        bus.Dirty = true;
        if (setMode == EventBusAction.VolumeSetMode.Absolute)
        {
            if (duration == 0)
                bus.RuntimeSelfVolume = targetVolume;
            else
            {
                bus.Fader.Activated = true;
                double currentTime = AudioSettings.dspTime;
                bus.Fader.Initialize(curveType, currentTime, currentTime + duration, bus.RuntimeSelfVolume, targetVolume);
            }
            
        }
        else
        {
            if (duration == 0)
                bus.RuntimeSelfVolume = Mathf.Clamp(bus.RuntimeSelfVolume + targetVolume, 0.0f, 1.0f);
            else
            {
                bus.Fader.Activated = true;
                double currentTime = AudioSettings.dspTime;
                float newVolume = Mathf.Clamp(bus.RuntimeSelfVolume + targetVolume, 0.0f, 1.0f);
                bus.Fader.Initialize(curveType, currentTime, currentTime + duration, bus.RuntimeSelfVolume, newVolume);
            }
        }
        
    }

    public static void UpdateBusVolumes(AudioBus bus)
    {
        double currentTime = AudioSettings.dspTime;
        Fader fader = bus.Fader;
        if (fader.Activated)
        {
            bus.Dirty = true;
            bus.RuntimeSelfVolume = (float)fader.Lerp(AudioSettings.dspTime);

            if (bus.RuntimeSelfVolume == fader.EndValue || fader.EndTime <= currentTime)
            {
                //Debug.Log(bus.RuntimeSelfVolume +"=="+ fader.EndValue +"||"+ fader.EndTime +"<="+ currentTime);
                fader.Activated = false;
            }
        }
        if (bus.Dirty)
        {
            if (bus.Parent != null)
            {
                bus.CombinedVolume = bus.Parent.RuntimeVolume;
            }
            else
            {
                bus.CombinedVolume = 1.0f;
            }
            
            double currentVolume = bus.RuntimeVolume;
            
            bus.RuntimeVolume = bus.RuntimeSelfVolume*bus.CombinedVolume * bus.Volume;
            if (bus.RuntimeVolume == currentVolume)
            {
                bus.Dirty = false;
            }
            var nodes = bus.GetRuntimePlayers();
            for (int i = 0; i < nodes.Count; ++i)
            {
                if (nodes[i] != null)
                    nodes[i].UpdateBusVolume(bus.RuntimeVolume);
                else
                {
                    nodes.SwapRemoveAt(i);
                }
            }
           
        }

        for (int i = 0; i < bus.Children.Count; ++i)
        {
            if (bus.Dirty)
                bus.Children[i].Dirty = true;
            UpdateBusVolumes(bus.Children[i]);
        }
    }

    public static void UpdateCombinedVolume(AudioBus bus)
    {
        UpdateCombinedVolume(bus, 1.0f);
    }

    private static void UpdateCombinedVolume(AudioBus bus, float volume)
    {
        if (bus != null)
        {
            float newVolume = bus.Volume*volume;
            if (newVolume != bus.CombinedVolume)
            {
                bus.Dirty = true;
                    //Non serialized, so will only stick while playing, will then get updated by the runtime system
            }
            bus.CombinedVolume = newVolume;
            bus.Volume = newVolume;
            bus.RuntimeSelfVolume = bus.Volume;

            for (int i = 0; i < bus.Children.Count; i++)
            {
                UpdateCombinedVolume(bus.Children[i], bus.CombinedVolume);
            }
        }
    }
}
