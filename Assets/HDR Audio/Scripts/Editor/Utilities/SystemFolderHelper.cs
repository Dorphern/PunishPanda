using UnityEngine;
using System.Collections;

public static class SystemFolderHelper {

    public static void DeleteFolderContent(string path)
    {
        System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(path);
        foreach (System.IO.FileInfo file in directory.GetFiles())
            file.Delete();
        foreach (System.IO.DirectoryInfo subDirectory in directory.GetDirectories())
            subDirectory.Delete(true);
    }
}
