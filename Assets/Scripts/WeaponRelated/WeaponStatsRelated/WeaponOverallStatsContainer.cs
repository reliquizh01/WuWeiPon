using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using User.Data;

public class WeaponOverallStatsContainer : MonoBehaviour
{
    public float screenWidththreshold = 1050;
    public GameObject aboveThreshold, belowThreshold;

    public List<WeaponStatsContainer> weaponStatsBelowWidthThreshold;
    public List<WeaponStatsContainer> weaponStatsAboveWidthThreshold;

    public List<WeaponStatsContainer> currentWeaponStatsContainers;
    public CanvasGroup canvasGroup;

    public Button condenseBtn;

    public void Start()
    {
        GameManager.Instance.AddToUpdateCurrencyCallBacks(CheckCondendseBtn);

        condenseBtn.onClick.AddListener(() =>
        {
            GameManager.Instance.SetGameState(GameStateEnum.Condensing);
        });
    }

    internal void CheckCondendseBtn(CurrencyEnum currency)
    {
        if(currency == CurrencyEnum.spirtualEssence)
        {
            condenseBtn.interactable = (UserDataBehavior.GetCurrency(currency) >= 1000);
        }
    }

    public void UpdateCurrentStatContainers()
    {
        if (Screen.width > screenWidththreshold)
        {
            belowThreshold.SetActive(false);

            aboveThreshold.SetActive(true);
            currentWeaponStatsContainers = weaponStatsAboveWidthThreshold;
        }
        else
        {
            aboveThreshold.SetActive(false);

            belowThreshold.SetActive(true);
            currentWeaponStatsContainers = weaponStatsBelowWidthThreshold;
        }
    }

    public void UpdateWeaponStats(WeaponData weaponData, WeaponStatEnum weaponStat)
    {
        WeaponStatsContainer container = currentWeaponStatsContainers.FirstOrDefault(x => x.weaponStat == weaponStat);

        if(container != null)
        {
            container.SetAmountFromWeaponData(weaponData);
        }
    }

    public void ShowWeaponStats()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void HideWeaponStats()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    internal void SetWeaponStats(WeaponData weaponData)
    {
        UpdateCurrentStatContainers();

        foreach(WeaponStatEnum weaponStat in Enum.GetValues(typeof(WeaponStatEnum)))
        {
            UpdateWeaponStats(weaponData, weaponStat);
        }
    }
}