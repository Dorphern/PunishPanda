using InAudio;
using InAudio.RuntimeHelperClass;

public class RuntimeInfoPool : InAudioObjectPool<RuntimeInfo>{
    public new RuntimeInfo GetObject()
    {
        if (freeObjects.Count == 0)
        {
            ReserveExtra(allocateSize);
        }

        var go = freeObjects[freeObjects.Count - 1];
        freeObjects.RemoveAt(freeObjects.Count - 1);
        return go;
    }
}