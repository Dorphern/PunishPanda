using System;
using UnityEngine;

namespace InAudio
{
    public static class FolderSettings
    {
        public const string Name = "InAudio";

        public const string RelativePathResources = "InAudio/";
               
        public const string AudioLoadData      =  RelativePathResources+"AudioSave";
        public const string EventLoadData      =  RelativePathResources+"EventSave";
        public const string BusLoadData        =  RelativePathResources+"BusSave";
        public const string BankLinkLoadData   =  RelativePathResources+"BankLinkSave";

        public const string BankLoadFolder = RelativePathResources + "Banks/";
               
#if UNITY_EDITOR
        public const string FullPathResources = "Assets/" + Name + "/Resources/" + Name + "/";
        public const string IconPath = "InAudio/Icons/";

        public const string AudioSaveDataPath = FullPathResources + "AudioSave.prefab";
        public const string EventSaveDataPath = FullPathResources + "EventSave.prefab";
        public const string BusSaveDataPath = FullPathResources + "BusSave.prefab";
        public const string BankLinkSaveDataPath = FullPathResources + "BankLinkSave.prefab";

        public const string BankCreateFolder = FullPathResources + "Banks/";
        public const string BankRelativeDictory = "/"+Name + "/Resources/" + Name + "/" + "Banks/";
        public const string BankDeleteDictory = FullPathResources + "Banks/";
        public const string BankSaveFolder =  FullPathResources  + "Banks/";


        public const string AudioManagerPath = "Assets/" + Name + "/Prefabs/InAudio Manager.prefab";

        public const string GUIUserPrefs = RelativePathResources + "Other/GUIUserPrefs";

        public const string ComponentPathInternal = "InAudio/Internal/";
        public const string ComponentPathInternalPools = "InAudio/Internal/Pools/";
        public const string ComponentPathInternalManager = ComponentPathInternal + "Manager/";
#endif



    }

}
