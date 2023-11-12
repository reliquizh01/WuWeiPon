using System.Collections.Generic;
using System.Runtime.Serialization;

[DataContract]
public class UserSettingsData
{
    #region Auto Skill

    [DataMember]
    public bool isAutoRollSkllsEnabled;

    [DataMember]
    public List<SkillRankEnum> acceptableSkillRankList = new List<SkillRankEnum>();

    #endregion Auto Skill

    #region Settings

    [DataMember]
    public float volumeStrength = 1.0f;

    #endregion

    public UserSettingsData() { }

    public UserSettingsData(UserSettingsData userSettingsData)
    {
        isAutoRollSkllsEnabled = userSettingsData.isAutoRollSkllsEnabled;
        volumeStrength = userSettingsData.volumeStrength;

        acceptableSkillRankList = new List<SkillRankEnum>(userSettingsData.acceptableSkillRankList);
    }
}