using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHiltBehavior : MonoBehaviour
{
    private bool CanDetectCollision = false;
    private List<Action> OnDamageReceived = new List<Action>();

    internal WeaponBehavior m_behavior;
    public void DamageReceived()
    {
        if (OnDamageReceived.Count > 0)
        {
            OnDamageReceived.ForEach(x => x.Invoke());
        }
    }

    public void SetCollisionDetection(bool setTo)
    {
        CanDetectCollision = setTo;

        if (!CanDetectCollision)
        {
            RemoveActionOnCollision();
        }
    }

    public void AddActionOnCollision(Action action)
    {

    }

    public void RemoveActionOnCollision()
    {
        OnDamageReceived.Clear();
    }

}