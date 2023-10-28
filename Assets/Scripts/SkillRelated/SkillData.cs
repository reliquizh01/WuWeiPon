using System.Collections.Generic;
using System.Runtime.Serialization;

[DataContract]
public class SkillData
{
    #region Constructors

    public SkillData() { }

    public SkillData(SkillData skillData)
    {
        skillName = skillData.skillName;
        description = skillData.description;
        skillIconFileName = skillData.skillIconFileName;
        skillLevel = skillData.skillLevel;
        skillType = skillData.skillType;
        skillValues = new Dictionary<string, object>(skillData.skillValues);
        isSkillConditionOnHit = skillData.isSkillConditionOnHit;
    }

    #endregion Constructors
    
    [DataMember]
    public string skillName = "";

    [DataMember]
    public string description = "";

    [DataMember]
    public string skillIconFileName = "";

    [DataMember]
    public int skillLevel = 0;

    [DataMember]
    public SkillTypeEnum skillType;

    [DataMember]
    public Dictionary<string, object> skillValues = new Dictionary<string, object>();

    [DataMember]
    public bool isSkillConditionOnHit = false;
}
