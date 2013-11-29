using System.Collections.Generic;
using InAudio;
using UnityEngine;

[AddComponentMenu(FolderSettings.ComponentPathInternalPools + "Game Object Pool")]
public class InAudioObjectPool<T> : MonoBehaviour where T : new()
{
    [SerializeField]
    protected int allocateSize = 10;

    [SerializeField]
    protected int initialAllocation = 10;

    protected List<T> freeObjects = new List<T>();

    public int AllocateSize
    {
        get { return allocateSize; }
        set
        {
            if (value > 0)
            {
                allocateSize = value;
            }
        }
    }

    public void ReleaseObject(T obj)
    {
        freeObjects.Add(obj);
    }

    public void ReserveExtra(int extra)
    {
        for (int i = 0; i < extra; ++i)
        {
            freeObjects.Add(new T());
        }
    }

    public T GetObject()
    {
        if (freeObjects.Count == 0)
        {
            ReserveExtra(allocateSize);
        }
        T go = freeObjects[freeObjects.Count - 1];
        freeObjects.RemoveAt(freeObjects.Count - 1);
        return go;
    }
}
