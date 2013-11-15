using UnityEngine;
using System.Collections;

public class BackButtonHandler : MonoBehaviour
{
    [SerializeField] private UIButton PauseButton;
    [SerializeField] private UIButton ResumeButton;
    [SerializeField] private UIButton LevelSelectButton;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (LevelSelectButton.gameObject.activeInHierarchy)
            {
                LevelSelectButton.gameObject.SendMessage("OnClick");
            }
            else if (PauseButton.gameObject.activeInHierarchy)
            {
                PauseButton.gameObject.SendMessage("OnClick");
            }
                   else if (ResumeButton.gameObject.activeInHierarchy)
            {
                ResumeButton.gameObject.SendMessage("OnClick");
            }
        }
    }
}
