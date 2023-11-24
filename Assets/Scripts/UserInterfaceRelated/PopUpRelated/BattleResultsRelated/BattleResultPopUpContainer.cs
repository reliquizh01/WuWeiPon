using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using User.Data;

public class BattleResultPopUpContainer : MonoBehaviour
{
    public GameObject container;
    public List<BattleResultsStatsContainer> statsResults;
    public Button confirmBtn;

    internal void SetStatResults(BattleRecordLogs battleLog)
    {
        foreach(RecordStatsEnum recordStats in Enum.GetValues(typeof(RecordStatsEnum)))
        {
            BattleTransaction transaction = battleLog.battleTransactionLog.Find(x => x.recordStats == recordStats);

            BattleResultsStatsContainer statsContainer = statsResults.First(x => x.statEnum == recordStats);
            bool isNewHigh = (UserDataBehavior.GetHighestRecordedStats(recordStats) < transaction.amount);

            if (isNewHigh)
            {
                switch (recordStats)
                {
                    case RecordStatsEnum.DamageTaken:
                        UserDataBehavior.SetHighestDamageTakenRecordNoSave(battleLog);
                        break;
                    case RecordStatsEnum.HighestOneHitDamage:
                        UserDataBehavior.SetOneHitDamageDealtRecordNoSave(battleLog);
                        break;
                    case RecordStatsEnum.TotalDamage:
                        UserDataBehavior.SetTotalDamageDealtRecordNoSave(battleLog);
                        break;
                    default:
                        break;
                }
            }

            statsContainer.SetupStatAmount(transaction.amount,isNewHigh);
        }
    }
}