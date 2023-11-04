using System.Runtime.Serialization;

[DataContract]
public class SpiritEnhancement
{
    [DataMember]
    public WeaponStatEnum WeaponStat;

    [DataMember]
    public float EnhancementAmount;
}