using UnityEditor;
using UnityEngine;
using System.Collections;

public static class SystemFolderHelper {

    //TODO Convert to http://docs.unity3d.com/Documentation/ScriptReference/AssetDatabase.CreateFolder.html
    public static void DeleteFolderContent(string path)
    {
        System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(path);
        if (directory.Exists)
        {
            foreach (System.IO.FileInfo file in directory.GetFiles())
                file.Delete();
            foreach (System.IO.DirectoryInfo subDirectory in directory.GetDirectories())
                subDirectory.Delete(true);
        }
        else
        {
            System.IO.Directory.CreateDirectory(path);
        }
    }
}
