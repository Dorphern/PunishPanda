using System.Collections.Generic;
using InAudio.ExtensionMethods;
using UnityEngine;
using System.Collections;

namespace InAudio 
{
public static class RuntimeHelper
{
    public static AudioNode SelectRandom(AudioNode randomNode)
    {
        int childCount = randomNode.Children.Count;
        var weights = ((RandomData)randomNode.NodeData).weights;
        int sum = 0;
        for (int i = 0; i < childCount; ++i)
        {
            sum += weights[i];
        }
        int randomArea = Random.Range(0, sum + 1); //+1 because range is non-inclusive

        int currentMax = 0;
        for (int i = 0; i < childCount; ++i)
        {
            currentMax += weights[i];
            if (weights[i] != 0 && randomArea <= currentMax)
            {
                return randomNode.Children[i];
            }
        }
        return null; //Only happens if all the sums are 0*/
    }

    public static AudioRolloffMode ApplyRolloffData(AudioNode current, AudioSource workOn)
    {
        var nodeData = current.NodeData;
        workOn.rolloffMode = nodeData.RolloffMode;
        workOn.maxDistance = nodeData.MaxDistance;
        workOn.minDistance = nodeData.MinDistance;

        if (nodeData.RolloffMode == AudioRolloffMode.Custom)
        {
            workOn.maxDistance = float.MaxValue;//Set to max so we can use our own animation curve
        }
        return nodeData.RolloffMode;
    }


    public static AudioRolloffMode ApplyAttentuation(AudioNode root, AudioNode current, AudioSource workOn)
    {
        if (current == root || current.NodeData.OverrideAttenuation)
        {
            return ApplyRolloffData(current, workOn);
        }
        else
        {
            return ApplyAttentuation(root, current.Parent, workOn);
        }
    }

    public static void ReleaseRuntimeInfo(RuntimeInfo info)
    {
        //Swap remove and set the new position
        info.PlacedIn.FindSwapRemove(info);
        InAudioInstanceFinder.RuntimeInfoPool.ReleaseObject(info);
    }

    public static float InitialDelay(NodeTypeData data)
    {
        if (!data.RandomizeDelay)
            return data.InitialDelayMin;

        return Random.Range(data.InitialDelayMin, data.InitialDelayMax);
    }


    public static float ApplyVolume(AudioNode root, AudioNode current)
    {
        if (current == root)
        {
            if (!current.NodeData.RandomVolume)
                return current.NodeData.MinVolume;
            else
                return Random.Range(current.NodeData.MinVolume, current.NodeData.MaxVolume);
        }

        if (!current.NodeData.RandomVolume)
            return current.NodeData.MinVolume * ApplyVolume(root, current.Parent);
        else
            return Random.Range(current.NodeData.MinVolume, current.NodeData.MaxVolume) * ApplyVolume(root, current.Parent);

    }

    public static byte GetLoops(AudioNode node)
    {
        byte loops;
        if (node.NodeData.RandomizeLoops)
            loops = (byte)Mathf.Min(Random.Range(node.NodeData.MinIterations, node.NodeData.MaxIterations + 1), 255);
        else
            loops = node.NodeData.MinIterations;
        return loops;
    }

    public static float ApplyPitch(AudioNode root, AudioNode current)
    {
        NodeTypeData nodeData = current.NodeData;
        float minPitch = nodeData.MinPitch;
        float maxPitch = nodeData.MaxPitch;
        bool isRandom = nodeData.RandomPitch;
        if (current == root)
        {
            if(!isRandom)
                return minPitch;
            else
            {
                return Random.Range(minPitch, maxPitch);
            }
        }

        if (!isRandom)
            return current.NodeData.MinPitch + ApplyPitch(root, current.Parent) - 1;
        else
        {
            return Random.Range(minPitch, maxPitch) + ApplyPitch(root, current.Parent) - 1;
        }

        
    }

    public static float LengthFromPitch(float length, float pitch)
    {
        return length / pitch;
    }
}
}
