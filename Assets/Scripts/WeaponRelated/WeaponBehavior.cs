using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBehavior : MonoBehaviour
{
    public WeaponMovement weaponMovement;
    public Sprite weaponSprite;

    public List<WeaponHiltBehavior> weaponHilts;
    public List<WeaponBladeBehavior> weaponBlades;

    internal int currentDamage = 1;

    public void Start()
    {
        if(weaponHilts != null && weaponHilts.Count > 0) 
        {
            weaponHilts.ForEach(x => x.m_behavior = this);
        }
        
        if(weaponBlades != null && weaponBlades.Count > 0) 
        {
            weaponBlades.ForEach(x => x.m_behavior = this);
        }
    }

    public void SetWeaponDetection(bool setTo)
    {
        if(weaponHilts != null)
        {
            weaponHilts.ForEach(x => x.SetCollisionDetection(setTo));
        }
        if(weaponBlades != null)
        {
            weaponHilts.ForEach(x => x.SetCollisionDetection(setTo));
        }
    }

    public void AddHiltAction(Action action)
    {
        weaponHilts.ForEach(x => x.AddActionOnCollision(action));
    }

    public void AddBladeAction(Action<int> action)
    {
        weaponBlades.ForEach(x => x.AddActionOnCollision(action));
    }
}