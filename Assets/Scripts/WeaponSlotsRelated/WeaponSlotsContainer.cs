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
            // Resets Skill Slots to remove old data
            ResetSkillSlots();


            for (int i = 0; i < skillSlots.Count; i++)
            {
                // Enable Skill Slots that are available
                if(weaponData.behaviorSkillSlotCount > i)
                {
                    skillSlots[i].gameObject.SetActive(true);
                }

                skillSlots[i].SetupSkillSlotVisuals(weaponData.skills.Find(x => x.slotNumber == skillSlots[i].slotNumber));
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
