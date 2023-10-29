using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSkillSlot : MonoBehaviour
{
    public Image skillIcon;
    public Image frame;
    public Image background;

    public void SetupCurrentSkillAttached(SkillData skillData)
    {
        skillIcon.gameObject.SetActive(true);
        skillIcon.sprite = DataVaultManager.Instance.GetSkillSprite(skillData.skillIconFileName);
    }

    public void ResetCurrentSetup()
    {
        skillIcon.gameObject.SetActive(false);
    }
}
