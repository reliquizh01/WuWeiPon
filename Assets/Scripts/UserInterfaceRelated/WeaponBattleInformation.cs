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


        internal bool enableExtraHealth = false;
        private float extraHealth = 0;
        
        public void LoadWeaponInformation(WeaponData weaponData,ref List<BaseBattleSkillBehavior> currentEquippedSkills)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (i < weaponData.behaviorSkillSlotCount)
                {
                    slots[i].gameObject.SetActive(true);

                    slots[i].skillIcon.gameObject.SetActive(weaponData.skills.Count > i);
                    
                    if(weaponData.skills.Count > i)
                    {
                        BaseBattleSkillBehavior skillBehavior = currentEquippedSkills.FirstOrDefault(x => x.skillName == weaponData.skills[i].skillName);

                        slots[i].SetupCurrentSkillAttached(weaponData.skills[i]);
    
                        if(skillBehavior != null)
                        {
                            slots[i].AttachToSkillBehavior(ref skillBehavior);
                        }
                    }
                    else
                    {

                    }
                }
            }

            for (int i = 0; i < attributes.Count; i++)
            {
                attributes[i].behaviorPill.enabled = true;

                if (i < weaponData.attributeSlotCount)
                {
                    attributes[i].gameObject.SetActive(true);

                    attributes[i].behaviorPill.gameObject.SetActive(weaponData.attributes.Count > i);
                }
            }

            currentHealth = weaponData.weapon_Health;
            maxHealth = weaponData.weapon_Health;
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

            if(enableExtraHealth && extraHealth > 0)
            {
                if(extraHealth >= damageCount)
                {
                    extraHealth -= damageCount;
                }
                else
                {
                    damageCount -= extraHealth;
                    extraHealth = 0;

                    currentHealth -= damageCount;
                }
            }
            else if((enableExtraHealth && extraHealth <= 0))
            {
                currentHealth -= damageCount;
            }
            else
            {
                currentHealth -= damageCount;
            }

            healthPointsbar.fillAmount = currentHealth / maxHealth;

            // All HP is gone
            if (currentHealth <= 0)
            {
                BattleManager.Instance.EndBattle(this);
            }
        }

        public void OnWeaponHeal(float healCount)
        {
            attributes.ForEach(x =>
            {
                healCount = x.ReceiveHealing(healCount);
            });

            float tmpHealth = currentHealth + healCount;
            
            if(tmpHealth > maxHealth && enableExtraHealth)
            {
                float difference = maxHealth - currentHealth;
                healCount -= difference;
                currentHealth += difference;

                extraHealth += healCount;
            }
            else if(tmpHealth > maxHealth && !enableExtraHealth)
            {
                currentHealth = maxHealth;
            }
            else
            {
                currentHealth += healCount;
            }

            healthPointsbar.fillAmount = currentHealth / maxHealth;
        }
    }
}