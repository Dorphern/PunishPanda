using System;
using UnityEngine;

namespace InAudio
{
    public static class FolderSettings
    {
        public const string Name = "InAudio";

        public readonly static string FullPathResources = "Assets/" + Name + "/Resources/" + Name+"/";
        public readonly static string RelativePathResources = "InAudio/";
               
        public readonly static string IconPath = "InAudio/Icons/";
               
        public readonly static string AudioSaveDataPath          = FullPathResources + "AudioSave.prefab";
        public readonly static string EventSaveDataPath          = FullPathResources + "EventSave.prefab";
        public readonly static string BusSaveDataPath            = FullPathResources + "BusSave.prefab";
        public readonly static string BankLinkSaveDataPath       = FullPathResources + "BankLinkSave.prefab";
               
        public readonly static string AudioLoadData      =  RelativePathResources+"AudioSave";
        public readonly static string EventLoadData      =  RelativePathResources+"EventSave";
        public readonly static string BusLoadData        =  RelativePathResources+"BusSave";
        public readonly static string BankLinkLoadData   =  RelativePathResources+"BankLinkSave";
               

        public readonly static string BankCreateFolder = FullPathResources + "Banks/";
        public readonly static string BankDeleteFolder = FullPathResources + "Banks/";
        public readonly static string BankSaveFolder =  FullPathResources  + "Banks/";
        public readonly static string BankLoadFolder = RelativePathResources + "Banks/";

        public readonly static string AudioManagerPath = "Assets/" + Name + "/Prefabs/InAudio Manager.prefab";

        public readonly static string GUIUserPrefs = RelativePathResources + "Other/GUIUserPrefs";

    }

}
