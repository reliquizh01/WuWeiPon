using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WeaponRelated
{
    public class WeaponSlotsContainer : MonoBehaviour
    {
        public List<TrainingSkillSlotsBehavior> skillSlots = new List<TrainingSkillSlotsBehavior>();
        
        public void SetupSkillSlots(WeaponData weaponData)
        {
            ResetSkillSlots();

            for (int i = 0; i < skillSlots.Count; i++)
            {
                if(weaponData.behaviorSkillSlotCount > i)
                {
                    skillSlots[i].gameObject.SetActive(true);
                }

                if(weaponData.skills.Count > i)
                {
                    skillSlots[i].SetupSkillSlot(weaponData.skills.First(x => x.slotNumber == i));
                }
                else
                {
                    skillSlots[i].SetupSkillSlot(null);
                }
            }
        }

        public void ResetSkillSlots()
        {
            skillSlots.ForEach(x => x.gameObject.SetActive(false));
        }

        public void UpgradeSkillSlot(SkillData skillData)
        {
            skillSlots.First(x => x.slotNumber == skillData.slotNumber).PlayUpgrade();
        }
    }
}
