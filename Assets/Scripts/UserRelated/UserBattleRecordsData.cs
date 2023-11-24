using System.Collections.Generic;
using System.Runtime.Serialization;

[DataContract]
public class UserBattleRecordsData
{
    public UserBattleRecordsData() { }

    public UserBattleRecordsData(UserBattleRecordsData userBattleRecordsData)
    {
        battleRecordLogs = new List<BattleRecordLogs>(userBattleRecordsData.battleRecordLogs);
    }

    [DataMember]
    public List<BattleRecordLogs> battleRecordLogs = new List<BattleRecordLogs>();

    [DataMember]
    public BattleRecordLogs highestDamageTakenLog = null;

    [DataMember]
    public BattleRecordLogs highestOneHitDamage = null;
    
    [DataMember]
    public BattleRecordLogs highestTotalDamage = null;
}