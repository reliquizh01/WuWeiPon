using UnityEngine;

public class MovementBattleSkillBehavior : BaseBattleSkillBehavior
{
    public bool hasMovementBonus = false;
    public float burstSpeedForce = 0.0f;
    public float constantSpeedAdd = 0.0f;

    public override void InitializeSkill(SkillData skillData)
    {
        base.InitializeSkill(skillData);

        if (skillData.skillValues.ContainsKey(SkillVariableNames.ADD_BURST_SPEED_FORCE))
        {
            hasMovementBonus = true;
            burstSpeedForce = (float)skillData.skillValues[SkillVariableNames.ADD_BURST_SPEED_FORCE];
        }
    }

    public void AddSpeedForce(Rigidbody2D rigidBody)
    {

        if (hasCooldown)
        {
            if (!cooldownStarted)
            {
                rigidBody.AddRelativeForce(rigidBody.velocity * burstSpeedForce);
                SetSkillOnCooldown(true);
            }
        }
        else
        {
                rigidBody.AddRelativeForce(rigidBody.velocity * burstSpeedForce);
        }
    }
}