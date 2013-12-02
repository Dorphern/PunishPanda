using UnityEngine;
using System.Collections;

public class ExternalAudioSource : MonoBehaviour
{
    public AudioBus Bus;
    public AudioSource AudioSource;

    [SerializeField] [Range(0.0f,1.0f)]
    private float _volume;

    public float volume
    {
        get { return _volume; }
        set
        {
            _volume = Mathf.Clamp(value, 0.0f, 1.0f);
            if (AudioSource != null)
            {
                if (Bus != null)
                    AudioSource.volume = _volume*Bus.RuntimeVolume;
                else
                    AudioSource.volume = _volume;
            }
        }
    }

    public void UpdateBusVolume(float newVolume)
    {
        
    }

    public void Start()
    {
        
    }
}
