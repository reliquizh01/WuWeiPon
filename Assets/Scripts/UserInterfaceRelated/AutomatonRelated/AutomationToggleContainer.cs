using DataManagement;
using DataManagement.Adapter;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using User.Data;

public class AutomationToggleContainer : MonoBehaviour
{
    public Slider automationSlider;
    internal Action automationChangedCallback;
    public Button automationBtn;
    public Button automationSettingsBtn;
    public void Start()
    {
        automationBtn.onClick.AddListener(() => ToggleAutomation());
    }

    public void ToggleAutomation()
    {
        automationSlider.value = (automationSlider.value == 1) ? 0 : 1;

        UserDataBehavior.ToggleAutomation(automationSlider.value == 1);
    }

    public void SetToggleAutomation()
    {
        automationSlider.value = (UserDataBehavior.IsSkillSpinAutomated()) ? 1 : 0;
    }

}
