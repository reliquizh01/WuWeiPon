using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponBattleInformation : MonoBehaviour
{
    public List<WeaponSkillSlot> slots = new List<WeaponSkillSlot>();
    public List<AttributeSlot> attributes = new List<AttributeSlot>();

    public void LoadWeaponInformation(WeaponData weaponData)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if(i < weaponData.behaviorSkillSlotCount)
            {
                slots[i].gameObject.SetActive(false);
            }
        }

        for (int i = 0;i < attributes.Count; i++)
        {
            attributes[i].behaviorPill.enabled = true;

            if(i < weaponData.attributeSlotCount)
            {
                slots[i].gameObject.SetActive(false);
            }
        }
    }

    public void OnWeaponDamaged(int damageCount)
    {
        attributes.ForEach(x =>
        {
            damageCount = x.ReceiveDamage(damageCount);
        });
    }
}
