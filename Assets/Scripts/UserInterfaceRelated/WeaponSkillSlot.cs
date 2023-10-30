using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSkillSlot : MonoBehaviour
{
    public Image skillIcon;
    public Image frame;
    public Image background;

    public CanvasGroup coolDownCanvasGroup;
    public CanvasGroup usageCanvasGroup;

    BaseBattleSkillBehavior currentSkillBehavior;

    public void Update()
    {
        if (currentSkillBehavior != null && currentSkillBehavior.isSkillOnCooldown())
        {
            currentSkillBehavior.Update(Time.deltaTime);

            frame.fillAmount = currentSkillBehavior.currentCooldown/currentSkillBehavior.cooldown;
        }
    }

    #region Setup

    public void SetupCurrentSkillAttached(SkillData skillData)
    {
        skillIcon.gameObject.SetActive(true);
        skillIcon.sprite = DataVaultManager.Instance.GetSkillSprite(skillData.skillIconFileName);

    }

    public void ResetCurrentSetup()
    {
        usageCanvasGroup.alpha = 1;
        coolDownCanvasGroup.alpha = 1;

        skillIcon.gameObject.SetActive(false);
        frame.fillAmount = 1;

        if(currentSkillBehavior != null)
        {
            currentSkillBehavior.RemoveCallbacks();
        }

        currentSkillBehavior = null;
    }

    #endregion Setup

    #region Battle Skill Behavior

    public void AttachToSkillBehavior(ref BaseBattleSkillBehavior skillBehavior)
    {
        currentSkillBehavior = skillBehavior;

        currentSkillBehavior.AddMaxUsageReachedCallback(OnUsageUpdate);
        currentSkillBehavior.AddOnCooldownUpdateCallback(OnCooldownUpdate);
        currentSkillBehavior.AddOnCooldownFinishCallback(OnCooldownFinish);
    }

    public void OnUsageUpdate()
    {
        if (currentSkillBehavior.isMaxUsageReached())
        {
            usageCanvasGroup.alpha = 0.5f;
        }
    }

    public void OnCooldownUpdate()
    {
        coolDownCanvasGroup.alpha = 0.5f;
    }

    private void OnCooldownFinish()
    {
        coolDownCanvasGroup.alpha = 1.0f;
    }

    #endregion Battle Skill Behavior
}
