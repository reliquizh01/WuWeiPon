using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace WeaponRelated
{
    public class WeaponBattleInformation : MonoBehaviour
    {
        public List<WeaponSkillSlot> slots = new List<WeaponSkillSlot>();
        public List<AttributeSlot> attributes = new List<AttributeSlot>();
        public Image healthPointsbar;
        private float currentHealth = 0;
        private float maxHealth = 0;

        public void LoadWeaponInformation(WeaponData weaponData)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (i < weaponData.behaviorSkillSlotCount)
                {
                    slots[i].gameObject.SetActive(true);
                    if(weaponData.skills.Count > i)
                    {
                        slots[i].SetupCurrentSkillAttached(weaponData.skills[i]);
                    }
                }
            }

            for (int i = 0; i < attributes.Count; i++)
            {
                attributes[i].behaviorPill.enabled = true;

                if (i < weaponData.attributeSlotCount)
                {
                    attributes[i].gameObject.SetActive(true);
                }
            }

            currentHealth = weaponData.weaponHealth;
            maxHealth = weaponData.weaponHealth;
            healthPointsbar.fillAmount = currentHealth / maxHealth;
        }

        public void ResetWeaponInformation()
        {
            slots.ForEach(x =>
            {
                x.ResetCurrentSetup();
                x.gameObject.SetActive(false);
            });

            attributes.ForEach(x => x.gameObject.SetActive(false));
        }

        public void OnWeaponDamaged(float damageCount)
        {
            attributes.ForEach(x =>
            {
                damageCount = x.ReceiveDamage(damageCount);
            });

            currentHealth -= damageCount;
            healthPointsbar.fillAmount = currentHealth / maxHealth;

            // All HP is gone
            if (currentHealth <= 0)
            {
                BattleManager.Instance.EndBattle(this);
            }
        }
    }
}