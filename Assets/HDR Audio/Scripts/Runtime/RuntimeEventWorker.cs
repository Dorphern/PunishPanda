using HDRAudio;
using HDRAudio.ExtensionMethods;
using UnityEngine;
using System.Collections.Generic;

public class RuntimeEventWorker : MonoBehaviour
{
    public void PlayAttachedTo(GameObject controllingObject, AudioNode audioNode, GameObject attachedTo)
    {
        var poolObject = audioGOPool.GetObject();
        poolObject.transform.parent = attachedTo.transform;
        poolObject.transform.localPosition = new Vector3();
        Play(controllingObject, audioNode, poolObject);
    }

    public void PlayAtPosition(GameObject controllingObject, AudioNode audioNode, Vector3 position)
    {
        var poolObject = audioGOPool.GetObject();
        Debug.Log(poolObject.name);
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
                    valueTupleList.SwapRemoveAt(i);
                }
            }
        }
    }

    private void Play(GameObject controllingObject, AudioNode audioNode, GameObject poolObject)
    {
        var runtimeInfo = runtimeInfoPool.GetObject();
        var player = poolObject.GetComponent<RuntimePlayer>();
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

