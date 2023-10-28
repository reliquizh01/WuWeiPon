using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace WeaponRelated
{

public class WeaponBladeBehavior : MonoBehaviour
{
    private bool CanDetectCollision = false;
    private List<Action<float>> OnCollisionActions = new List<Action<float>>();

    internal WeaponBehavior m_behavior;

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<WeaponHiltBehavior>() != null)
        {
            List<Action<float>> tmpActions = new List<Action<float>>(OnCollisionActions);

            foreach(Action<float> action in tmpActions)
            {
                action.Invoke(m_behavior.currentDamage);
                if(OnCollisionActions.Count == 0)
                {
                    break;
                }
            }
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

    public void AddActionOnCollision(Action<float> action)
    {
        OnCollisionActions.Add(action);
    }

    public void RemoveActionOnCollision()
    {
        OnCollisionActions.Clear();
    }

}

}