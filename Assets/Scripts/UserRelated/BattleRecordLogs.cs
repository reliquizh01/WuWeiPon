using System.Collections.Generic;
using System.Runtime.Serialization;

[DataContract]
public class BattleRecordLogs
{
    #region Constructors

    public BattleRecordLogs() { }

    public BattleRecordLogs(BattleRecordLogs battleRecordLogs)
    {
        battleId = battleRecordLogs.battleId;
        battleTransactionLog = new List<BattleTransaction>(battleTransactionLog);
        winningWeapon = battleRecordLogs.winningWeapon;
        enemyUser = battleRecordLogs.enemyUser;
    }

    #endregion Constructors

    [DataMember]
    public string battleType = "Normal";

    [DataMember]
    public string battleId = "";

    [DataMember]
    public List<BattleTransaction> battleTransactionLog = new List<BattleTransaction>();

    [DataMember]
    public string winningWeapon;

    [DataMember]
    public string enemyUser;
}