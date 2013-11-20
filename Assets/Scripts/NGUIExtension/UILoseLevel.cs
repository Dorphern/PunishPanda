using UnityEngine;
using System.Collections;

[AddComponentMenu("NGUI/PunishPanda/Lose Level")]
public class UILoseLevel : MonoBehaviour
{
    [SerializeField]
    private GameObject ToEnable;

    private UITweener[] twenners;

    void OnEnable()
    {
        twenners = GetComponentsInChildren<UITweener>();
        InstanceFinder.GameManager.ActiveLevel.onLevelLost += () =>
        {
            ToEnable.SetActive(true);
            for (int i = 0; i < twenners.Length; i++)
            {
                twenners[i].Reset();
                twenners[i].PlayForward();
            }
        };
    }
}
