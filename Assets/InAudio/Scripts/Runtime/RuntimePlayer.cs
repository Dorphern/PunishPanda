using System;
using InAudio;
using InAudio.ExtensionMethods;
using InAudio.RuntimeHelperClass;
using UnityEngine;
using System.Collections;


/// <summary>
/// The class that actually plays the audio
/// </summary>
[AddComponentMenu(FolderSettings.ComponentPathPrefabs+"Audio Player/Runtime Player")]
[RequireComponent(typeof(AudioSource))]
public class RuntimePlayer : MonoBehaviour
{
    public void Play(AudioNode node, RuntimeInfo playingInfo)
    {
        dspPool = InAudioInstanceFinder.DSPTimePool;
        attachedToBus = node.GetBus();
        busVolume = attachedToBus.RuntimeVolume;

        //This is to queue the next playing node, as the first clip will not yield a waitforseconds
        firstClip = true;
        runtimeInfo = playingInfo;


        PlayingNode = node;
        DSPTime time = dspPool.GetObject();
        time.CurrentEndTime = AudioSettings.dspTime;
        StartCoroutine(StartPlay(node, node, time));
    }

    public void Break()
    {
        breakLoop = true;
    }

    public void Stop()
    {
        
        StopForReuse();

        spawnedFrom.ReleaseObject(this);
        runtimeInfo.Node.GetBus().RuntimePlayers.Remove(this);
       
        StopAllCoroutines();
    }

    public AudioNode NodePlaying
    {
        get
        {
            return PlayingNode;
        }
    }

    private void StopForReuse()
    {
        for (int i = 0; i < audioSources.Length; ++i)
        {
            audioSources[i].clip = null;
            audioSources[i].Stop();
            endTimes[i] = 0;
            audioSources[i].SetScheduledEndTime(AudioSettings.dspTime - 2); //Set to end in the past so it won't suddenly start playing when enabled
        }

        var instances = PlayingNode.CurrentInstances;
        for (int i = 0; i < instances.Count; i++)
        {
            if (instances[i].Player == this)
            {
                instances.SwapRemoveAt(i);
                break;
            }
        }

        RuntimeHelper.ReleaseRuntimeInfo(runtimeInfo);

    }

    public void UpdateBusVolume(float newBusVolume)
    {
        busVolume = newBusVolume;
        for (int i = 0; i < audioSources.Length; i++)
        {
            if (audioSources == null)
                continue;
            audioSources[i].volume = originalVolume[i] * newBusVolume;
        }
    }

    public void SetNewBus(AudioBus bus)
    {
        attachedToBus = bus;
    }

    public void Initialize(AudioGOPool spawnedFrom)
    {
        
        this.spawnedFrom = spawnedFrom;
        if (audioSources == null)
            audioSources = GetComponents<AudioSource>();
        if (endTimes == null)
            endTimes = new double[audioSources.Length];
        if(originalVolume == null)
            originalVolume = new float[audioSources.Length];
    }

    private AudioNode PlayingNode;

    private AudioSource[] audioSources;
    private double[] endTimes;
    private float[] originalVolume;

    private RuntimeInfo runtimeInfo;

    private DSPTimePool dspPool;

    private bool firstClip;

    private int currentIndex = 0;

    private AudioGOPool spawnedFrom;

    private float busVolume;

    private AudioBus attachedToBus;

    private bool breakLoop;

    private AudioSource Current
    {
        get
        {
            return audioSources[currentIndex];
        }
    }

    private IEnumerator StartPlay(AudioNode root, AudioNode current, DSPTime endTime)
    {
        breakLoop = false;
        current.GetBus().RuntimePlayers.Add(this);

        yield return StartCoroutine(NextNode(root, current, endTime));
        dspPool.ReleaseObject(endTime);
        yield return new WaitForSeconds((float)(endTime.CurrentEndTime - AudioSettings.dspTime));
        //Clean up object
        StopAndCleanup();
    }

    private IEnumerator NextNode(AudioNode root, AudioNode current, DSPTime endTime)
    {
        byte loops = 0;
        var nodeData = current.NodeData;
        bool loopInfinite = nodeData.LoopInfinite;
        if(!loopInfinite)
            loops = RuntimeHelper.GetLoops(current);

        endTime.CurrentEndTime += RuntimeHelper.InitialDelay(nodeData);

        if (nodeData.Loop == false)
        {
            loops = 0;
            loopInfinite = false;
        }
        for (int i = 0; i < 1 + loops || loopInfinite; ++i) //For at least once
        {
            if (breakLoop)
            {
                loops = 0;
                loopInfinite = false;
            }

            if (current.Type == AudioNodeType.Audio)
            {
                NextFreeAudioSource();
                float nodeVolume;
                
                float length = PlayScheduled(root, current, endTime.CurrentEndTime, out nodeVolume);
                
                originalVolume[currentIndex] = nodeVolume;
                
                endTime.CurrentEndTime += length;

                if (!firstClip)
                    yield return new WaitForSeconds((float)(endTime.CurrentEndTime - AudioSettings.dspTime) - length + 0.050f);

                firstClip = false;
            }
            else if (current.Type == AudioNodeType.Random)
            {
                yield return StartCoroutine(NextNode(root, RuntimeHelper.SelectRandom(current), endTime));
            }
            else if (current.Type == AudioNodeType.Sequence)
            {
                for (int j = 0; j < current.Children.Count; ++j)
                {
                    yield return StartCoroutine(NextNode(root, current.Children[j], endTime));
                }
            }
            else if (current.Type == AudioNodeType.Multi)
            {
                Coroutine[] toStart = new Coroutine[current.Children.Count];
                DSPTime[] childTimes = new DSPTime[current.Children.Count];

                for (int j = 0; j < childTimes.Length; ++j)
                {
                    DSPTime dspTime = dspPool.GetObject();
                    dspTime.CurrentEndTime = endTime.CurrentEndTime;
                    childTimes[j] = dspTime;
                }
                for (int j = 0; j < current.Children.Count; ++j)
                {
                    toStart[j] = StartCoroutine(NextNode(root, current.Children[j], childTimes[j]));
                }
                for (int j = 0; j < childTimes.Length; ++j)
                {
                    DSPTime dspTime = childTimes[j];
                    if (endTime.CurrentEndTime < dspTime.CurrentEndTime)
                        endTime.CurrentEndTime = dspTime.CurrentEndTime;
                    else
                        dspPool.ReleaseObject(dspTime);
                }
            }
        }
    }

    private float PlayScheduled(AudioNode startNode, AudioNode currentNode, double playAtDSPTime, out float volume)
    {
        var audioData = currentNode.NodeData as AudioData;
        float length = 0;
        volume = 1;
        if (audioData.Clip != null)
        {
            length = audioData.Clip.samples/(float) audioData.Clip.frequency;

            Current.clip = audioData.Clip;
            Current.volume = RuntimeHelper.ApplyVolume(startNode, currentNode);
            volume = Current.volume;
            Current.volume *= busVolume;

            Current.pitch = RuntimeHelper.ApplyPitch(startNode, currentNode);
            RuntimeHelper.ApplyAttentuation(startNode, currentNode, Current);

            length = RuntimeHelper.LengthFromPitch(length, Current.pitch);
            endTimes[currentIndex] = playAtDSPTime + length;

            Current.PlayScheduled(playAtDSPTime);

        }
        
        return length;
    }

    private void NextFreeAudioSource()
    {
        double dspTime = AudioSettings.dspTime;

        if (endTimes == null)
        {
            Initialize(spawnedFrom); 
        }
         
        for (int i = 0; i < audioSources.Length; ++i)
        {
            if (endTimes[i] < dspTime)
            { 
                currentIndex = i;
                return;
            }

        }
        Array.Resize(ref endTimes, endTimes.Length + 1);
        Array.Resize(ref originalVolume, originalVolume.Length + 1);

        currentIndex = audioSources.Length;
        gameObject.AddComponent<AudioSource>();
        audioSources = GetComponents<AudioSource>();
    }

    private void StopAndCleanup()
    {
        Stop();
    }
}

namespace InAudio.RuntimeHelperClass
{
    public class DSPTime
    {
        public double CurrentEndTime;

        public DSPTime(double currentEndTime)
        {
            CurrentEndTime = currentEndTime;
        }

        public DSPTime()
        {
        }

        public DSPTime(DSPTime time)
        {
            CurrentEndTime = time.CurrentEndTime;
        }
    }
}
