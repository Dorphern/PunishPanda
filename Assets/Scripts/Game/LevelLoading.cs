using UnityEngine;
using System.Collections;

public class LevelLoading : MonoBehaviour
{
    [SerializeField] private GameObject loadLevelObject;

    void Awake()
    {
        InstanceFinder.LevelManager.IsLoadingLevel = true;
        StartCoroutine(LoadLevel(InstanceFinder.LevelManager.LevelToLoad));
    }

    private IEnumerator LoadLevel(string levelName)
    {
        yield return Application.LoadLevelAdditiveAsync(levelName);
        
        Destroy(loadLevelObject);
        Destroy(gameObject);
        InstanceFinder.LevelManager.IsLoadingLevel = false;
    }
}
