using System;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponRelated
{

    public class WeaponBehavior : MonoBehaviour
    {
        public WeaponMovement weaponMovement;
        public SpriteRenderer weaponSprite;

        public List<WeaponHiltBehavior> weaponHilts;
        public List<WeaponBladeBehavior> weaponBlades;

        internal float currentDamage = 1;

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

        public void SetWeaponBehavior(WeaponData weaponData)
        {
            currentDamage = weaponData.damage_physical;
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

        public void AddHiltAction(Action action)
        {
            weaponHilts.ForEach(x => x.AddActionOnCollision(action));
        }

        public void AddBladeAction(Action<float> action)
        {
            weaponBlades.ForEach(x => x.AddActionOnCollision(action));
        }

        public void ResestActons()
        {
            weaponHilts.ForEach(x => x.RemoveActionOnCollision());
            weaponBlades.ForEach(x => x.RemoveActionOnCollision());
        }
    }
}