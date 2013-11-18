using UnityEngine;
using System.Collections;

[AddComponentMenu("NGUI/PunishPanda/Check If Unlocked")]
public class CheckIfUnlocked : MonoBehaviour
{
    public Texture2D LockScreen;

    private int index;

    void OnEnable()
    {
        Transform superParent = transform.parent.parent;
        Transform parent = transform.parent;
        int childCount = superParent.childCount;
        for (int i = 0; i < childCount; i++)
        {
            if (superParent.GetChild(i) == parent)
            {
                index = i;
                break;
            }
        }

        var levels = InstanceFinder.LevelManager.CurrentWorld.Levels;
        if (levels.Count > index && !levels[index].Unlocked)
        {
            GetComponent<UITexture>().mainTexture = LockScreen;
        }
    }
}
