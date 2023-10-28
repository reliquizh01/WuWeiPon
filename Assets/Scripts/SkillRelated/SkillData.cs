using System.Collections.Generic;
using System.Runtime.Serialization;

[DataContract]
public class SkillData
{
    [DataMember]
    public string skillName = "";

    [DataMember]
    public string description = "";

    [DataMember]
    public string skillIconFileName = "";

    [DataMember]
    public SkillTypeEnum skillType;

    [DataMember]
    public Dictionary<string, float> skillValues = new Dictionary<string, float>();
}
