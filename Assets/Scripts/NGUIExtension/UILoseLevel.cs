using UnityEngine;
using System.Collections;

[AddComponentMenu("NGUI/PunishPanda/Lose Level")]
public class UILoseLevel : MonoBehaviour
{
    [SerializeField]
    private GameObject LoseCamera;

    [SerializeField]
    private GameObject EscapeLabel;

    [SerializeField]
    private float LoseFadeTime = 4.0f;


    void OnEnable()
    {
        EscapeLabel.GetComponent<UIPlayTween>().Play(true);
        InstanceFinder.GameManager.ActiveLevel.onLevelLost += () =>
        {
            EscapeLabel.SetActive(true);           
            StartCoroutine(WaitForLoseFade());
        };
    }

    private IEnumerator WaitForLoseFade()
    {
        yield return new WaitForSeconds(LoseFadeTime);
        Time.timeScale = 0;
		if(InputHandler.instance!=null) InputHandler.instance.PausedGame();
        LoseCamera.SetActive(true);
    }

    public void RestartLevel()
    {
        InstanceFinder.LevelManager.Reload();
    }

    public void SelectLevel()
    {
        InstanceFinder.LevelManager.LoadLevelsMenu();
    }
}
