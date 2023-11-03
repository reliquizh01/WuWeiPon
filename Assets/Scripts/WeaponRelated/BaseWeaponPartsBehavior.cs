using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace WeaponRelated
{
    public class BaseWeaponPartsBehavior : MonoBehaviour
    {
        internal List<DamageBattleSkillBehavior> OnDamageSkillCollisionActions = new List<DamageBattleSkillBehavior>();
        internal List<MovementBattleSkillBehavior> OnMovementSkillCollisionActions = new List<MovementBattleSkillBehavior>();
        internal List<HealBattleSkillBehavior> OnHealSkillCollisionActions = new List<HealBattleSkillBehavior>();

        internal WeaponBehavior m_behavior;

        internal void AddOnSkillCollisionActions(DamageBattleSkillBehavior action)
        {
            OnDamageSkillCollisionActions.Add(action);
        }

        internal void AddOnSkillCollisionActions(HealBattleSkillBehavior action)
        {
            OnHealSkillCollisionActions.Add(action);
        }

        internal void AddOnSkillCollisionActions(MovementBattleSkillBehavior action)
        {
            OnMovementSkillCollisionActions.Add(action);
        }
    }
}