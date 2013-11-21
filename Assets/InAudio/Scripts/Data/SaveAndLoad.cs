using UnityEngine;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace InAudio
{
public static class SaveAndLoad  
{
    private static Component[] GetComponents(GameObject go)
    {
        if (go != null)
        {
            return go.GetComponentsInChildren(typeof(MonoBehaviour), true);
        }
        return null;
    }

    public static void LoadManagerData(out Component[] audioData, out Component[] eventData, out Component[] busData, out Component[] bankLinkData)
    {
        GameObject eventDataGO      = Resources.Load(FolderSettings.EventLoadData) as GameObject;
        GameObject audioDataGO      = Resources.Load(FolderSettings.AudioLoadData) as GameObject;
        GameObject busDataGO        = Resources.Load(FolderSettings.BusLoadData) as GameObject;
        GameObject bankLinkDataGO   = Resources.Load(FolderSettings.BankLinkLoadData) as GameObject;


        busData = GetComponents(busDataGO);
        audioData = GetComponents(audioDataGO);
        eventData = GetComponents(eventDataGO);
        bankLinkData = GetComponents(bankLinkDataGO);
    }

    public static AudioBank LoadAudioBank(int id)
    {
        GameObject bankGO = Resources.Load(FolderSettings.BankLoadFolder + id) as GameObject;
        if(bankGO != null)
        {
            var components = bankGO.GetComponentsInChildren(typeof(AudioBank), true);
            if (components != null && components.Length > 0 && components[0] as AudioBank != null)
            {
                return components[0] as AudioBank;
            }
        }
        Debug.LogWarning("Audio Bank with id " + id + " could not be found");
        return null;
    }

    #if UNITY_EDITOR    
    public static void CreateDataPrefabs(GameObject AudioRoot, GameObject EventRoot, GameObject BusRoot, GameObject BankLinkRoot)
    {
        CreateAudioNodeRootPrefab(AudioRoot);
        CreateAudioEventRootPrefab(EventRoot);
        CreateAudioBusRootPrefab(BusRoot);
        CreateAudioBankLinkPrefab(BankLinkRoot);
    }

    public static void CreateAudioNodeRootPrefab(GameObject root)
    {
        PrefabUtility.CreatePrefab(FolderSettings.AudioSaveDataPath, root);
        Object.DestroyImmediate(root);
    }
    public static void CreateAudioEventRootPrefab(GameObject root)
    {
        PrefabUtility.CreatePrefab(FolderSettings.EventSaveDataPath, root);
        Object.DestroyImmediate(root);
    }
    public static void CreateAudioBusRootPrefab(GameObject root)
    {
        PrefabUtility.CreatePrefab(FolderSettings.BusSaveDataPath, root);
        Object.DestroyImmediate(root);
    }
    public static void CreateAudioBankLinkPrefab(GameObject root)
    {
        PrefabUtility.CreatePrefab(FolderSettings.BankLinkSaveDataPath, root);
        Object.DestroyImmediate(root);
    }

    public static AudioBank CreateAudioBank(int guid)
    {
        GameObject go = new GameObject(guid.ToString());
        var bank = go.AddComponent<AudioBank>();
        bank.GUID = guid;
        
        PrefabUtility.CreatePrefab(FolderSettings.BankSaveFolder + guid + ".prefab", go);
        Object.DestroyImmediate(go);
        return LoadAudioBank(guid);
    }


    #endif
}
}
