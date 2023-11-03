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
        internal float nextHealToInflict = 0;
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

        public void OnCollisionEnter2D(Collision2D collision)
        {
            
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

                        if (newDamage.skillDetectionPartEnums.Count > 0)
                        { 
                            if(newDamage.skillDetectionPartEnums.Contains(SkillDetectionPartEnum.BladeOnlyDetection))
                            {
                                AddToBladeSkillAction(newDamage);
                            }
                            if(newDamage.skillDetectionPartEnums.Contains(SkillDetectionPartEnum.HiltOnlyDetection))
                            {
                                AddToHiltSkillAction(newDamage);
                            }
                            battleSkills.Add(newDamage);
                        }
                        
                        break;
                    case SkillTypeEnum.Heal:
                        HealBattleSkillBehavior newHeal = new HealBattleSkillBehavior();
                        newHeal.InitializeSkill(skill);

                        if(newHeal.skillDetectionPartEnums.Count > 0)
                        {
                            if (newHeal.skillDetectionPartEnums.Contains(SkillDetectionPartEnum.BladeOnlyDetection))
                            {
                                AddToBladeSkillAction(newHeal);
                            }
                            if (newHeal.skillDetectionPartEnums.Contains(SkillDetectionPartEnum.HiltOnlyDetection))
                            {
                                AddToHiltSkillAction(newHeal);
                            }
                            battleSkills.Add(newHeal);
                        }
                        break;
                    case SkillTypeEnum.Movement:

                        MovementBattleSkillBehavior newMovement = new MovementBattleSkillBehavior();
                        newMovement.InitializeSkill(skill);

                        if (newMovement.skillDetectionPartEnums.Count > 0)
                        {
                            if (newMovement.skillDetectionPartEnums.Contains(SkillDetectionPartEnum.BladeOnlyDetection))
                            {
                                AddToBladeSkillAction(newMovement);
                            }
                            if (newMovement.skillDetectionPartEnums.Contains(SkillDetectionPartEnum.HiltOnlyDetection))
                            {
                                AddToHiltSkillAction(newMovement);
                            }
                            battleSkills.Add(newMovement);
                        }
                        break;
                    default:
                        break;
                }
            }

            battleSkills.ForEach(x =>
            {
                if(x.hasCooldown)
                {
                    x.SetSkillOnCooldown(true);
                }
            });

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

        private void AddToBladeSkillAction(DamageBattleSkillBehavior action)
        {
            weaponBlades.ForEach(x => x.AddOnSkillCollisionActions(action));
        }
        private void AddToBladeSkillAction(MovementBattleSkillBehavior action)
        {
            weaponBlades.ForEach(x => x.AddOnSkillCollisionActions(action));
        }

        private void AddToBladeSkillAction(HealBattleSkillBehavior action)
        {
            weaponBlades.ForEach(x => x.AddOnSkillCollisionActions(action));

        }
        private void AddToHiltSkillAction(DamageBattleSkillBehavior action)
        {
            weaponHilts.ForEach(x => x.AddOnSkillCollisionActions(action));
        }
        private void AddToHiltSkillAction(HealBattleSkillBehavior action)
        {
            weaponHilts.ForEach(x => x.AddOnSkillCollisionActions(action));
        }

        private void AddToHiltSkillAction(MovementBattleSkillBehavior action)
        {
            weaponHilts.ForEach(x => x.AddOnSkillCollisionActions(action));
        }

        public void AddBladeActionsForOpposingUserInterfaceUpdateOnHit(Action<float> action)
        {
            weaponBlades.ForEach(x => x.AddCallBackOnceBladeHitsHilt(action));
        }

        public void AddBladeActonForSelfUserInterfaceUpdateOnHit(Action<float> action)
        {
            weaponBlades.ForEach(x => x.AddCallBackForChangesInSelfHealth(action));
        }

        public void ResestActons()
        {
            weaponHilts.ForEach(x => x.RemoveActionOnCollision());
            weaponBlades.ForEach(x => x.RemoveActionOnCollision());
        }
    }
}