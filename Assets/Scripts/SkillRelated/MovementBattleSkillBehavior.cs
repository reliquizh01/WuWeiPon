public class MovementBattleSkillBehavior : BaseBattleSkillBehavior
{
    public bool hasMovementBonus = false;
    public float burstSpeedForce = 0.0f;
    public float constantSpeedAdd = 0.0f;

    public override void InitializeSkill(SkillData skillData)
    {
        base.InitializeSkill(skillData);

        if (skillData.skillValues.ContainsKey(SkillVariableNames.SET_TARGET_TO_WALLS))
        {
            skillTargetEnums.Add(SkillTargetEnum.Walls);
        }

        if (skillData.skillValues.ContainsKey(SkillVariableNames.ADD_BURST_SPEED_FORCE))
        {
            burstSpeedForce = (float)skillData.skillValues[SkillVariableNames.ADD_BURST_SPEED_FORCE];
        }
    }
}