using InAudio;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[AddComponentMenu(FolderSettings.ComponentPathInternalManager + "Instance Finder")]
public class InAudioInstanceFinder : MonoBehaviour
{
    private static InAudioInstanceFinder instance;
    void OnEnable()
    {
        if(instance == null)
            instance = this;
    }

    private static CommonDataManager _dataManager;
    public static CommonDataManager DataManager
    {
        get
        {
            if (_dataManager == null)
            {
                _dataManager = FindObjectOfType(typeof(CommonDataManager)) as CommonDataManager;
                if(_dataManager != null)
                    _dataManager.Load();
            }
            return _dataManager;
        }        
    }

    private static RuntimeAudioData _runtimeAudioData;
    public static RuntimeAudioData RuntimeAudioData
    {
        get
        {
            if (_runtimeAudioData == null)
            {
                _runtimeAudioData = instance.GetComponent<RuntimeAudioData>();
            }
            return _runtimeAudioData;
        }
    }

    private static RuntimeEventWorker _runtimeEventWorker;
    public static RuntimeEventWorker RuntimeEventWorker
    {
        get
        {
            if (_runtimeEventWorker == null)
            {
                _runtimeEventWorker = instance.GetComponent<RuntimeEventWorker>();
            }
            return _runtimeEventWorker;
        }
    }

    private static RuntimeInfoPool _runtimeInfoPool;
    public static RuntimeInfoPool RuntimeInfoPool
    {
        get
        {
            if (_runtimeInfoPool == null)
            {
                _runtimeInfoPool = instance.GetComponent<RuntimeInfoPool>();
            }
            return _runtimeInfoPool;
        }
    }

    private static DSPTimePool _dspTimePool;
    public static DSPTimePool DSPTimePool
    {
        get 
        {
            if (_dspTimePool == null)
            {
                _dspTimePool = instance.GetComponent<DSPTimePool>();
            }
            return _dspTimePool;
        }
    }

#if UNITY_EDITOR
    private static InAudioGUIUserPrefs _inAudioGuiUserPref;
    public static InAudioGUIUserPrefs InAudioGuiUserPrefs
    {
        get
        {
            if (_inAudioGuiUserPref == null)
            {
                var prefGO = Resources.Load(FolderSettings.GUIUserPrefs) as GameObject;
                if (prefGO != null)
                {
                    _inAudioGuiUserPref = prefGO.GetComponent<InAudioGUIUserPrefs>();
                    if (_inAudioGuiUserPref == null)
                    {
                        _inAudioGuiUserPref = prefGO.AddComponent<InAudioGUIUserPrefs>();
                    }
                }

            }
            return _inAudioGuiUserPref;
        }
    }
#endif
}
