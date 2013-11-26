using InAudio;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class HDRInstanceFinder : MonoBehaviour
{
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
                _runtimeAudioData = FindObjectOfType(typeof(RuntimeAudioData)) as RuntimeAudioData;
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
                _runtimeEventWorker = FindObjectOfType(typeof(RuntimeEventWorker)) as RuntimeEventWorker;
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
                _runtimeInfoPool = FindObjectOfType(typeof(RuntimeInfoPool)) as RuntimeInfoPool;
            }
            return _runtimeInfoPool;
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
                    _inAudioGuiUserPref = prefGO.GetComponent<InAudioGUIUserPrefs>();

            }
            return _inAudioGuiUserPref;
        }
    }
#endif
}
