using DataManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using User.Data;

public class AutomationSettingsBehavior : MonoBehaviour
{
    public static AutomationSettingsBehavior Instance;

    public GameObject raycastBlocker;
    public GameObject container;
    public Button confirmBtn;

    public AutomationToggleContainer skillAutomationToggle;
    public List<SkillRankButtonContainer> skillRankTriggers;

    public void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        skillAutomationToggle.automationSettingsBtn.onClick.AddListener(() => ShowAutomationSettings());
      
        skillRankTriggers.ForEach(x => x.rankBtn.onClick.AddListener(() => ToggleAutomationSKillRank(x)));

        confirmBtn.onClick.AddListener(() => HideAutomationSettings());
    }

    internal void ShowAutomationSettings()
    {
        raycastBlocker.SetActive(true);
        container.SetActive(true);
    }

    internal void HideAutomationSettings()
    {
        container.SetActive(false);
        raycastBlocker.SetActive(false);
    }

    internal void ToggleAutomationSKillRank(SkillRankButtonContainer rank)
    {
        List<SkillRankEnum> automatedSkillsOnUser = UserDataBehavior.GetAutomatedSkillRarity();

        if(automatedSkillsOnUser.Contains(rank.buttonRank))
        {
            automatedSkillsOnUser.Remove(rank.buttonRank);
        }
        else
        {
            automatedSkillsOnUser.Add(rank.buttonRank);
        }

        rank.UpdateButtonSetup();
    }

    internal void SetupAutomation()
    {
        skillAutomationToggle.SetToggleAutomation();
    }
}
