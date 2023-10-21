using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using WeaponRelated;

public class WeaponBehavior : MonoBehaviour
{
    WeaponDataBehavior dataBehavior;
    public Animation animation;
    public WeaponBehaviorStateEnum state = WeaponBehaviorStateEnum.Idle;

    float moveSpeed = 20.0f;
    bool forcedToPosition = false;
    Vector2 forcedPositon = Vector2.zero;

    List<Action> forcedPositionReached = new List<Action>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (forcedToPosition)
        {
            transform.position = Vector2.MoveTowards(transform.position, forcedPositon, moveSpeed * Time.deltaTime);

            float dist = Vector2.Distance(forcedPositon, transform.position);

            if(dist < 0.001f)
            {
                positionReached();
            }
        }
    }

    public void MoveToPosition(Vector2 forcePosition, Action onPositionReached = null)
    {
        forcedToPosition = true;
        forcedPositon = forcePosition;

        if (onPositionReached != null)
        {
            forcedPositionReached.Add(onPositionReached);
        }
    }

    private void positionReached()
    {
        forcedToPosition = false;
        forcedPositionReached.ForEach(x => x.Invoke());
        forcedPositionReached.Clear();
    }
}
