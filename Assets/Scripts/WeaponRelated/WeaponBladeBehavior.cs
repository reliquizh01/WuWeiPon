using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponBladeBehavior : MonoBehaviour
{
    private bool CanDetectCollision = false;
    private List<Action<int>> OnCollisionActions = new List<Action<int>>();

    internal WeaponBehavior m_behavior;

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<WeaponHiltBehavior>() != null)
        {
            OnCollisionActions.ForEach(x => x.Invoke(m_behavior.currentDamage));
        }
    }

    public void OnCollisionEnter2D(Collision2D col)
    {

    }

    public void SetCollisionDetection(bool setTo)
    {
        CanDetectCollision = setTo;

        if (!CanDetectCollision)
        {
            RemoveActionOnCollision();
        }
    }

    public void AddActionOnCollision(Action<int> action)
    {
        OnCollisionActions.Add(action);
    }

    public void RemoveActionOnCollision()
    {
        OnCollisionActions.Clear();
    }

}