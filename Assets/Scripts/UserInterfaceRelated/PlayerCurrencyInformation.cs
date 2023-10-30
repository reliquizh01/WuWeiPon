using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using User.Data;

public class PlayerCurrencyInformation : MonoBehaviour
{
    public CurrencyEnum currencyType;
    public bool withIcon;

    public Image currencyIcon;
    public TextMeshProUGUI amountText;

    public void Start()
    {
        if (withIcon)
        {
            SetupCurrencyWithIcon(currencyType);
        }
        else
        {
            SetupCurrencyOnly(currencyType);
        }

        GameManager.Instance.UpdateCurrencyValues(currencyType);
    }

    public void OnDestroy()
    {
        GameManager.Instance.RemoveFromUpdateCurrencyCallbacks(this.UpdateCurrencyValue);
    }

    public void UpdateCurrencyValue(CurrencyEnum currency)
    {
        if(currencyType == currency)
        {
            int amount = UserDataBehavior.GetCurrency(currency);
            string formatted = amount.ToString("N0");
            amountText.text = formatted;
        }
    }

    private void SetupCurrencyWithIcon(CurrencyEnum type)
    {
        currencyType = type;
        currencyIcon.sprite = DataVaultManager.Instance.GetCurrencySprite(type);

        GameManager.Instance.AddToUpdateCurrencyCallBacks(UpdateCurrencyValue);
    }

    private void SetupCurrencyOnly(CurrencyEnum type)
    {
        currencyType = type;
        GameManager.Instance.AddToUpdateCurrencyCallBacks(UpdateCurrencyValue);
    }
}
