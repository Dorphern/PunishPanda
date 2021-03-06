﻿using System.Collections.Generic;
using PunishPanda;
using UnityEngine;
using System.Collections;

namespace PunishPanda
{
    public struct LevelDeaths
    {
        public int TotalPandaCount;
        public int AlivePandas;

        public List<ComboKill> ComboKills;
    }

    public struct ComboKill
    {
        public int ComboCount
        {
            get
            {
                return PerfectKills + NormalKills;
            }
        }

        public int PerfectKills;
        public int NormalKills;
    }

}

public class ComboSystem : MonoBehaviour
{
    [SerializeField]
    private float maxTimeBetweenKills = 1.0f;
    [SerializeField]
    private float showDelayTime = 0.0f;

    [SerializeField]
    private GameObject comboObject;
    [SerializeField]
    private UILabel comboLabel;
    [SerializeField]
    private UILabel comboLabelChild;

    [SerializeField]
    private GameObject killObject;
    [SerializeField]
    private UILabel perfectKillLabel;
    [SerializeField]
    private UILabel normalKillLabel;

    [SerializeField]
    private GameObject bloodParticles;


    [SerializeField]
    [EventHookAttribute("Slobby Kill")]
    private List<AudioEvent> slobbyKillEvents;

    [SerializeField]
    [EventHookAttribute("Perfect Kill")]
    private List<AudioEvent> perfectKillEvents;

    [SerializeField]
    [EventHookAttribute("Combo Kill")]
    private List<AudioEvent> comboKillEvents;

    private float lastPandaKillTime;
    private int pandaComboKills;
    private bool isDoingCombo = false;
    private int displayedKills = 0;
    private bool perfectKill;

    private int perfectKillsCombo;
    private int normalKillsCombo;

    private LevelDeaths levelDeaths;

    public LevelDeaths LevelDeaths
    {
        get
        {
            return levelDeaths;
        }
    }

    public int AlivePandas
    {
        get
        {
            return levelDeaths.AlivePandas;
        }
    }

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

        normalKillsCombo = 0;
        perfectKillsCombo = 0;
    }

    public void RegisterPanda()
    {
        levelDeaths.TotalPandaCount += 1;
        levelDeaths.AlivePandas += 1;
        //pandas.Add(panda);
    }

    public void OnPandaDeath(bool perfectKill)
    {
        this.perfectKill = perfectKill;
        isDoingCombo = true;

        if (perfectKill)
            perfectKillsCombo += 1;
        else
            normalKillsCombo += 1;

        pandaComboKills += 1;
        lastPandaKillTime = Time.time;
        levelDeaths.AlivePandas -= 1;
        InstanceFinder.StatsManager.PandasKilled++;

        if (pandaComboKills > 1)
        {
            HDRSystem.PostEvents(gameObject, comboKillEvents);
        }
        else
        {
            if (perfectKill)
                HDRSystem.PostEvents(gameObject, perfectKillEvents);
            else
                HDRSystem.PostEvents(gameObject, slobbyKillEvents);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDoingCombo && Time.time > lastPandaKillTime + showDelayTime)
        {
            if (displayedKills != pandaComboKills)
            {
                //Show combo screen for maxTimeBetweenKills time
                StopAllCoroutines();
                comboObject.SetActive(false);
                killObject.SetActive(false);
                if (bloodParticles != null)
                    bloodParticles.SetActive(true);
                StartCoroutine(ShowComboScreen());
            }
        }
        else if (Time.time > lastPandaKillTime + showDelayTime)
        {
            Reset();
        }
    }

    IEnumerator ShowComboScreen()
    {
        displayedKills = pandaComboKills;




        if (pandaComboKills == 1)
        {
            killObject.SetActive(true);
            if (perfectKill)
            {
                perfectKillLabel.gameObject.SetActive(true);
            }
            else
            {
                normalKillLabel.gameObject.SetActive(true);
            }
        }
        else if (pandaComboKills > 1)
        {
            string comboText = "";
            switch (pandaComboKills)
            {
                case 2:
                    comboText = "2X";
                    break;
                case 3:
                    comboText = "3X";
                    break;
                case 4:
                    comboText = "4X";
                    break;
                case 5:
                    comboText = "5X";
                    break;
                case 6:
                    comboText = "6X";
                    break;
                case 7:
                    comboText = "7X";
                    break;
                case 8:
                    comboText = "8X";
                    break;
                default:
                    comboLabel.text = "MEGA";
                    break;
            }
            comboLabel.text = comboText;
            comboLabelChild.text = comboText;

            comboObject.SetActive(true);
        }

        killObject.GetComponent<UIPlayTween>().Play(true);
        comboObject.GetComponent<UIPlayTween>().Play(true);

        yield return new WaitForSeconds(maxTimeBetweenKills + 0.2f);
        if (levelDeaths.ComboKills == null)
            levelDeaths.ComboKills = new List<ComboKill>(2);
        levelDeaths.ComboKills.Add(new ComboKill()
        {
            NormalKills = normalKillsCombo,
            PerfectKills = perfectKillsCombo,
        });

        AddStatistics();
        Reset();
        comboObject.SetActive(false);
        killObject.SetActive(false);
        normalKillLabel.gameObject.SetActive(false);
        perfectKillLabel.gameObject.SetActive(false);
        if (bloodParticles != null)
            bloodParticles.SetActive(false);
    }

    void AddStatistics()
    {
        if (InstanceFinder.StatsManager != null && pandaComboKills > 1)
        {
            InstanceFinder.StatsManager.PandasComboKilled += pandaComboKills;


        }
    }
}