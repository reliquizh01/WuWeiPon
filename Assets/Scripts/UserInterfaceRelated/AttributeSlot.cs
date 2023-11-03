using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttributeSlot : MonoBehaviour
{
    public Image background;
    public Image behaviorPill;
    public Image frame;

    public List<Action<float>> OnReceiveDamage = new List<Action<float>>();
    public List<Action<float>> OnReceiveHeal = new List<Action<float>>();
    public List<Action<float>> OnReceiveMovement = new List<Action<float>>();

    public float ReceiveDamage(float amount)
    {

        if(OnReceiveDamage.Count > 0)
        {
            OnReceiveDamage.ForEach(x => x.Invoke(amount));
        }

        return amount;
    }

    public float ReceiveMovement(float amount)
    {
        if (OnReceiveMovement.Count > 0)
        {
            OnReceiveMovement.ForEach(x => x.Invoke(amount));
        }

        return amount;
    }
    public float ReceiveHealing(float amount)
    {
        if (OnReceiveDamage.Count > 0)
        {
            OnReceiveDamage.ForEach(x => x.Invoke(amount));
        }

        return amount;
    }
}
