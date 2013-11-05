using UnityEngine;
using System.Collections;

public class LevelLoading : MonoBehaviour
{
    [SerializeField] private GameObject _loadLevelObject;

    void Awake()
    {
        StartCoroutine(LoadLevel(InstanceFinder.LevelManager.LevelToLoad));
    }

    private IEnumerator LoadLevel(string levelName)
    {
        yield return new WaitForSeconds(1.2f);
        yield return Application.LoadLevelAdditiveAsync(levelName);

        Destroy(_loadLevelObject);
        Destroy(gameObject);
    }
}
