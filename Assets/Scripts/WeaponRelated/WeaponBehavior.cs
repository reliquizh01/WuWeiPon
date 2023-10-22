using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using WeaponRelated;

public class WeaponBehavior : MonoBehaviour
{
    WeaponDataBehavior dataBehavior = new WeaponDataBehavior();
    public Transform container;
    public Animation myAnim;

    public WeaponBehaviorStateEnum state = WeaponBehaviorStateEnum.Idle;

    public WeaponMovement currentWeapon;

    float forcedMovementSpeed = 40.0f;
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
            transform.position = Vector2.MoveTowards(transform.position, forcedPositon, forcedMovementSpeed * Time.deltaTime);

            float dist = Vector2.Distance(forcedPositon, transform.position);

            if(dist < 0.001f)
            {
                positionReached();
            }
        }
    }

    public void SetWeaponState(WeaponBehaviorStateEnum nextState)
    {
        state = nextState;

        switch (state)
        {
            case WeaponBehaviorStateEnum.Idle:
                myAnim.Play("WeaponIdleAnimation_1");
                break;
            case WeaponBehaviorStateEnum.Battle:
                myAnim.Stop();
                container.localPosition = Vector2.zero;
                container.localRotation = Quaternion.identity;
                break;
            case WeaponBehaviorStateEnum.FromChest:
                myAnim.Play("WeaponFromChest");
                break;
            case WeaponBehaviorStateEnum.ToBattlePosition:
                myAnim.Play("WeaponGoToPosition_1");
                break;
            default:
                break;
        }
    }
    public void SetWeaponData(WeaponData weaponData)
    {
        dataBehavior.weaponData = new WeaponData(weaponData);

        //TODO
        //LOAD THE DATA AND ITS PROPER EFFECTS HERE (WEAPON LOOKS SHOULD CHANGE)
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
