using System;
using UnityEngine;

namespace HDRAudio
{
    public static class FolderSettings
    {
        public readonly static string FullPathResources  = "Assets/HDR Audio/Resources/HDR Audio/";
        public readonly static string RelativePathResources = "HDR Audio/";
               
        public readonly static string IconPath = "HDR Audio/Icons/";
               
        public readonly static string AudioSaveDataPath          = FullPathResources + "HDRAudioSave.prefab";
        public readonly static string EventSaveDataPath          = FullPathResources + "HDREventSave.prefab";
        public readonly static string BusSaveDataPath            = FullPathResources + "HDRBusSave.prefab";
        public readonly static string BankLinkSaveDataPath       = FullPathResources + "HDRBankLinkSave.prefab";
               
        public readonly static string AudioLoadData      =  RelativePathResources+"HDRAudioSave";
        public readonly static string EventLoadData      =  RelativePathResources+"HDREventSave";
        public readonly static string BusLoadData        =  RelativePathResources+"HDRBusSave";
        public readonly static string BankLinkLoadData   =  RelativePathResources+"HDRBankLinkSave";
               

        public readonly static string BankCreateFolder = FullPathResources + "Banks/";
        public readonly static string BankDeleteFolder = FullPathResources + "Banks/";
        public readonly static string BankSaveFolder =  FullPathResources  + "Banks/";
        public readonly static string BankLoadFolder = RelativePathResources + "Banks/";
        
        public readonly static string AudioManagerPath = "Assets/HDR Audio/Prefabs/HDRAudioManager.prefab";

        public readonly static string GUIUserPrefs = "Assets/HDR Audio/Prefabs/" + "HDRGUIUserPrefs.prefab";

    }

}
