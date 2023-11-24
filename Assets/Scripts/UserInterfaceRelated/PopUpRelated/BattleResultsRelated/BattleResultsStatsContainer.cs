using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleResultsStatsContainer : MonoBehaviour
{
    public RecordStatsEnum statEnum;

    public TextMeshProUGUI statTitle;
    public TextMeshProUGUI statAmount;
    public GameObject newHighIndicator;

    public void SetupStatAmount(float amount, bool isHighest = false)
    {
        statTitle.text = UniformityConverter.RecordStatsResultToString(statEnum);

        statAmount.text = amount.ToString("N2");

        newHighIndicator.SetActive(isHighest);
    }
}
