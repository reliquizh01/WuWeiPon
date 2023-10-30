using System;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponRelated
{

    public class WeaponBehavior : MonoBehaviour
    {
        public WeaponData weapon;

        public WeaponMovement weaponMovement;
        public SpriteRenderer weaponSprite;

        public List<WeaponHiltBehavior> weaponHilts;
        public List<WeaponBladeBehavior> weaponBlades;

        internal float currentDamage = 1;
        internal float nextDamageToInflict = 0;
        internal int teamNumber = -1;

        public void Start()
        {
            if (weaponHilts != null && weaponHilts.Count > 0)
            {
                weaponHilts.ForEach(x => x.m_behavior = this);
            }

            if (weaponBlades != null && weaponBlades.Count > 0)
            {
                weaponBlades.ForEach(x => x.m_behavior = this);
            }
        }

        /// <summary>
        /// Setup the Weapon behavior by providing the weaponData and the team the weapon is aligned to
        /// </summary>
        /// <param name="weaponData">the data used to create the weapon</param>
        /// <param name="team">the team the weapon will not detect when dealing damage</param>
        public void SetWeaponBehavior(WeaponData weaponData, int team)
        {
            weapon = weaponData;
            teamNumber = team;

            currentDamage = weaponData.damage_physical;
        }

        public List<BaseBattleSkillBehavior> SetupWeaponSkills()
        {
            List<BaseBattleSkillBehavior> battleSkills = new List<BaseBattleSkillBehavior>();

            foreach (SkillData skill in weapon.skills)
            {
                switch (skill.skillType)
                {
                    case SkillTypeEnum.Damage:
                        DamageBattleSkillBehavior newDamage = new DamageBattleSkillBehavior();
                        newDamage.InitializeSkill(skill);
                        AddBladeSkillAction(newDamage);
                        battleSkills.Add(newDamage);
                        break;
                    case SkillTypeEnum.Heal:
                        break;
                    case SkillTypeEnum.Movement:
                        break;
                    default:
                        break;
                }
            }

            return battleSkills;
        }

        public void SetWeaponDetection(bool setTo)
        {
            if (weaponHilts != null)
            {
                weaponHilts.ForEach(x => x.SetCollisionDetection(setTo));
            }
            if (weaponBlades != null)
            {
                weaponHilts.ForEach(x => x.SetCollisionDetection(setTo));
            }

            if (setTo)
            {
                weaponMovement.weaponRigidBody.bodyType = RigidbodyType2D.Dynamic;
            }
            else
            {
                weaponMovement.weaponRigidBody.bodyType = RigidbodyType2D.Static;
            }
        }

        private void AddBladeSkillAction(DamageBattleSkillBehavior action)
        {
            weaponBlades.ForEach(x => x.AddOnSkillCollisionActions(action));
        }

        public void AddBladeActionsForOpposingUserInterfaceUpdateOnHit(Action<float> action)
        {
            weaponBlades.ForEach(x => x.AddCallBackOnceBladeHitsHilt(action));
        }

        public void ResestActons()
        {
            weaponHilts.ForEach(x => x.RemoveActionOnCollision());
            weaponBlades.ForEach(x => x.RemoveActionOnCollision());
        }
    }
}