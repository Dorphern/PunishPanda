using UnityEngine;
using System.Collections;

public class ComboSystem : MonoBehaviour
{
    [SerializeField] private float maxTimeBetweenKills = 1.0f;
    [SerializeField] private float showDelayTime = 0.0f;

    [SerializeField] private GameObject comboObject;
    [SerializeField] private UILabel comboLabel;

    [SerializeField] private GameObject killObject;
    [SerializeField] private UILabel killLabel;

    [SerializeField] private string normalKillString;
    [SerializeField] private string perfectKillString;

    [SerializeField]
    private UITweener[] tweens;

    private float lastPandaKillTime;
    private int pandaComboKills;
    private bool isDoingCombo = false;
    private int displayedKills = 0;
    private bool perfectKill;

    void OnEnable()
    {
        InstanceFinder.ComboSystem = this;
    }

    private void Reset()
    {
        perfectKill = false;
        displayedKills = 0;
        isDoingCombo = false;
        pandaComboKills = 0;
        lastPandaKillTime = 0;
    }

    public void OnPandaDeath(bool perfectKill)
    {
        this.perfectKill = true;
        isDoingCombo = true;
        pandaComboKills += 1;
        lastPandaKillTime = Time.time;

    }
	
	// Update is called once per frame
	void Update () {
        if (isDoingCombo && Time.time > lastPandaKillTime + showDelayTime)
	    {
            if (displayedKills != pandaComboKills)
	        {
                //Show combo screen for maxTimeBetweenKills time
                StopAllCoroutines();
                comboObject.SetActive(false);
                killObject.SetActive(false);
	            StartCoroutine(ShowComboScreen());
	        }
	    }
	}

    IEnumerator ShowComboScreen()
    {
        displayedKills = pandaComboKills;
        
        if (pandaComboKills == 1)
        {
            killObject.SetActive(true);
            if (perfectKill)
                killLabel.text = perfectKillString;
            else
            {
                killLabel.text = normalKillString;
            }
        }
        else if (pandaComboKills > 1)
        {
            switch (pandaComboKills)
            {
                case 2:
                    comboLabel.text = "2X";
                    break;
                case 3:
                    comboLabel.text = "3X";
                    break;
                case 4:
                    comboLabel.text = "4X";
                    break;
                case 5:
                    comboLabel.text = "5X";
                    break;
                case 6:
                    comboLabel.text = "6X";
                    break;
                case 7:
                    comboLabel.text = "7X";
                    break;
                case 8:
                    comboLabel.text = "8X";
                    break;
                default:
                    comboLabel.text = "MEGA";
                    break;
            }

            comboObject.SetActive(true);
        }
        for (int i = 0; i < tweens.Length; i++)
        {
            tweens[i].Reset();
            tweens[i].PlayForward();
        }
        yield return new WaitForSeconds(maxTimeBetweenKills + 0.2f);
        Reset();
        comboObject.SetActive(false);
        killObject.SetActive(false);
    }
}