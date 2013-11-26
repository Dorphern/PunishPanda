using System;
using InAudio;
using InAudio.ExtensionMethods;
using UnityEngine;
using System.Collections.Generic;

public class RuntimeEventWorker : MonoBehaviour
{
    public void PlayAttachedTo(GameObject controllingObject, AudioNode audioNode, GameObject attachedTo)
    {
        List<InstanceInfo> currentInstances = audioNode.CurrentInstances;
        if (!AllowedStealing(audioNode, currentInstances))
        {
            return;
        }
        var runtimePlayer = audioGOPool.GetObject();
        currentInstances.Add(new InstanceInfo(AudioSettings.dspTime, runtimePlayer));
        runtimePlayer.transform.parent = attachedTo.transform;
        runtimePlayer.transform.localPosition = new Vector3();
        Play(controllingObject, audioNode, runtimePlayer);
    }

   

    public void PlayAtPosition(GameObject controllingObject, AudioNode audioNode, Vector3 position)
    {
        List<InstanceInfo> currentInstances = audioNode.CurrentInstances;
        if (!AllowedStealing(audioNode, currentInstances))
            return;

        var poolObject = audioGOPool.GetObject();
        poolObject.transform.position = position; 
        Play(controllingObject, audioNode, poolObject);
    }

    public void StopAll(GameObject controllingObject)
    {
        List<RuntimeInfo> valueTupleList;
        GOAudioNodes.TryGetValue(controllingObject, out valueTupleList);
        if (valueTupleList != null)
        {
            for (int i = 0; i < valueTupleList.Count; ++i)
            {
                RuntimePlayer player = valueTupleList[i].Player;
                player.Stop();
                valueTupleList.SwapRemoveAt(i);
            }
        }
    }

    public void Break(GameObject controllingObject, AudioNode toBreak)
    {
        List<RuntimeInfo> valueTupleList;
        GOAudioNodes.TryGetValue(controllingObject, out valueTupleList);
        if (valueTupleList != null)
        {
            for (int i = 0; i < valueTupleList.Count; ++i)
            {
                if (valueTupleList[i].Node == toBreak)
                {
                    valueTupleList[i].Player.Break();
                }
            }
        }
    }

    public void StopByNode(GameObject controllingObject, AudioNode nodeToStop)
    {
        List<RuntimeInfo> valueTupleList;
        GOAudioNodes.TryGetValue(controllingObject, out valueTupleList);
        
        if (valueTupleList != null)
        {
            
            for (int i = 0; i < valueTupleList.Count; ++i)
            {
                if (valueTupleList[i].Node == nodeToStop)
                {
                    valueTupleList[i].Player.Stop();
                }
            }
        } 
    }

    private void Play(GameObject controllingObject, AudioNode audioNode, RuntimePlayer player)
    {
        var runtimeInfo = runtimeInfoPool.GetObject();
        runtimeInfo.Node = audioNode;
        runtimeInfo.Player = player;
        List<RuntimeInfo> tupleList = GetValue(GOAudioNodes, controllingObject);
        tupleList.Add(runtimeInfo);
        runtimeInfo.ListIndex = tupleList.Count - 1;
        runtimeInfo.PlacedIn = tupleList;
        player.Play(audioNode, runtimeInfo);
    }


    private List<RuntimeInfo> GetValue(Dictionary<GameObject, List<RuntimeInfo>> dictionary, GameObject go)
    {
        List<RuntimeInfo> tupleList;
        if (!dictionary.TryGetValue(go, out tupleList))
        {
            tupleList = new List<RuntimeInfo>();
            dictionary.Add(go, tupleList);
        }
        return tupleList;
    }

    private AudioGOPool audioGOPool;
    private RuntimeInfoPool runtimeInfoPool;

    private Dictionary<GameObject, List<RuntimeInfo>> GOAudioNodes = new Dictionary<GameObject, List<RuntimeInfo>>();

    private static bool AllowedStealing(AudioNode audioNode, List<InstanceInfo> currentInstances)
    {

        if (audioNode.LimitInstances && currentInstances.Count >= audioNode.MaxInstances)
        {
            RuntimePlayer player = null;
            var stealType = audioNode.InstanceStealingTypes;
            if (stealType == InstanceStealingTypes.NoStealing)
                return false;

            int index = 0;
            InstanceInfo foundInfo;
            if (stealType == InstanceStealingTypes.Newest)
            {
                double newestTime = 0;

                for (int i = 0; i < currentInstances.Count; i++)
                {
                    InstanceInfo instanceInfo = currentInstances[i];
                    if (instanceInfo.Timestamp > newestTime)
                    {
                        newestTime = instanceInfo.Timestamp;
                        index = i;
                    }
                }
            }
            else if (stealType == InstanceStealingTypes.Oldest)
            {
                double oldestTime = Double.MaxValue;
                for (int i = 0; i < currentInstances.Count; i++)
                {
                    InstanceInfo instanceInfo = currentInstances[i];
                    if (instanceInfo.Timestamp < oldestTime)
                    {
                        oldestTime = instanceInfo.Timestamp;
                        index = i;
                    }
                }
            }

            foundInfo = currentInstances[index];
            player = foundInfo.Player;
            currentInstances.SwapRemoveAt(index);
            player.Stop();
        }
        return true;
    }

    void OnEnable()
    {
        if (audioGOPool == null)
        {
            audioGOPool = GetComponent<AudioGOPool>(); 
        }
        if (runtimeInfoPool == null)
        {
            runtimeInfoPool = GetComponent<RuntimeInfoPool>();
        }
        if (GOAudioNodes == null)
        {
            GOAudioNodes = new Dictionary<GameObject, List<RuntimeInfo>>();
        }
    }
}