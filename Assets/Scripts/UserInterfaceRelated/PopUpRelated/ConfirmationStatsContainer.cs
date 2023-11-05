using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConfirmationStatsContainer : MonoBehaviour
{
    public TextMeshProUGUI statNameText;
    public TextMeshProUGUI statBeforeAmountText;
    public TextMeshProUGUI statAfterAmountText;
    public WeaponStatEnum weaponStatEnum;

    public void SetStatContainerValues(float previousAmount, float nextAmount)
    {
        statNameText.text = UniformityConverter.StatEnumToStatName(weaponStatEnum);

        statBeforeAmountText.text = UniformityConverter.StatValueToStatString(weaponStatEnum, previousAmount);
        statAfterAmountText.text = UniformityConverter.StatValueToStatString(weaponStatEnum, nextAmount);
    }
}
