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

    public List<Action<int>> OnReceiveDamage = new List<Action<int>>();

    public int ReceiveDamage(int amount)
    {

        if(OnReceiveDamage.Count > 0)
        {
            OnReceiveDamage.ForEach(x => x.Invoke(amount));
        }

        if(amount > 0 && behaviorPill.isActiveAndEnabled)
        {
            behaviorPill.enabled = false;
            amount--;
        }

        return amount;
    }
}
