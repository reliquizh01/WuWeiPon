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

        internal WeaponBehavior m_behavior;
    }
}