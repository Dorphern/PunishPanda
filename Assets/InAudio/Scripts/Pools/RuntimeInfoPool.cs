using InAudio;
using InAudio.RuntimeHelperClass;
using UnityEngine;

[AddComponentMenu(FolderSettings.ComponentPathPrefabsManager + "Runtime Info Pool")]
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