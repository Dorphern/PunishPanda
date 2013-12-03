using System.Threading;
using UnityEngine;
using System.Collections;

public class CountDownTime : MonoBehaviour
{
    private ExternalAudioSource external;

    public float StartDelayBetweenSounds;
    public float EndDelayBetweenSounds;
    private float length;

    void Start()
    {
        external = GetComponent<ExternalAudioSource>();
    }


    public void Activate(float length)
    {
        this.length = length;
        StartCoroutine(PlaySounds());
    }

    public IEnumerator PlaySounds()
    {
        float startTime = Time.time;
        float endTime = startTime + length;

        while (Time.time < endTime)
        {
            external.AudioSource.Play();
            yield return new WaitForSeconds(external.AudioSource.clip.length + Mathf.Lerp(StartDelayBetweenSounds, EndDelayBetweenSounds, Mathf.SmoothStep(0.0f, 1.0f, BetweenTime(Time.time, startTime, endTime))));
        }
        external.AudioSource.Play();
    }

    public float BetweenTime(float currentTime, float startTime, float endTime)
    {
        if (currentTime > endTime)
            return 1.0f;
        else if (currentTime < startTime)
            return 0.0f;
        float duration = endTime - startTime;
        return 1 - (endTime - currentTime) / duration;
    }
}
