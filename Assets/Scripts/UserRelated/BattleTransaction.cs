using System.Runtime.Serialization;

[DataContract]
public class BattleTransaction
{
    [DataMember]
    public string timeStamp;

    [DataMember]
    public RecordStatsEnum recordStats;

    [DataMember]
    public string dealer;

    [DataMember]
    public string receipient;

    [DataMember]
    public float amount;
}