using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using System.Collections;

public class UnlockScreenPopulator : MonoBehaviour
{
    public GameObject PanelPrefab;
    public float SpaceBetweenItems;
    public GameObject Grid;
    public Texture2D LockScreen;

    private float lastPos = 0;

    private List<UILabel> textLabels = new List<UILabel>();

    void Start()
    {
        var levels = InstanceFinder.LevelManager.CurrentWorld.Levels;
        for (int i = 0; i < levels.Count; i++)
        {
            var go = Object.Instantiate(PanelPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            var labelChild = go.GetComponentInChildren<UILabel>();
            textLabels.Add(labelChild);
            go.transform.parent = Grid.transform;
            go.transform.localPosition = new Vector3(lastPos, 0, 0);
            go.transform.localScale = Vector3.one;
            labelChild.transform.localPosition = new Vector3(0, -250, 0);


            var ffTexture = go.GetComponentInChildren<UITexture>();
            
            if (!levels[i].UnlockedFunFact)
            {
                ffTexture.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                
                ffTexture.mainTexture = LockScreen;
 
            }
            else
            {
                ffTexture.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                ffTexture.mainTexture = levels[i].FunFactsTexture;
              
            }
            lastPos += SpaceBetweenItems;
        }
        OnEnable();
    }

    //Set labels to the right language
    void OnEnable()
    {
        var levels = InstanceFinder.LevelManager.CurrentWorld.Levels;
        for (int i = 0; i < textLabels.Count; i++)
        {
            if (levels[i].UnlockedFunFact)
            {
                if (Localization.instance.IsEnglish)
                {
                    textLabels[i].text = levels[i].FunFactsText;
                }
                else
                {
                    textLabels[i].text = levels[i].DanishFunFactsText;
                }
            }
        }
    }
}
