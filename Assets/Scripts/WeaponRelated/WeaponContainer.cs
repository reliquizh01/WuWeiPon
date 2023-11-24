using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using WeaponRelated;

namespace WeaponRelated
{

    public class WeaponContainer : AnimationMonoBehavior
    {
        internal WeaponDataBehavior dataBehavior = new WeaponDataBehavior();

        public WeaponBehaviorStateEnum state = WeaponBehaviorStateEnum.Idle;

        #region Reference
        public Transform container;

        public WeaponSlotsContainer weaponSlotsContainer;
        public WeaponBehavior currentWeapon;
        public WeaponOverallStatsContainer weaponOverallStatsContainer;
        #endregion Reference

        #region ForceMovement

        float forcedMovementSpeed = 40.0f;
        bool forcedToPosition = false;
        Vector2 forcedPositon = Vector2.zero;
        List<Action> forcedPositionReached = new List<Action>();

        #endregion ForceMovement

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

                if (dist < 0.001f)
                {
                    positionReached();
                }
            }
        }

        public void HideUI()
        {
            weaponSlotsContainer.gameObject.SetActive(false);
            weaponOverallStatsContainer.HideWeaponStats();
        }
        
        public void ShowUI()
        {
            weaponSlotsContainer.gameObject.SetActive(true);
            weaponOverallStatsContainer.ShowWeaponStats();
        }

        public void SetWeaponState  (WeaponBehaviorStateEnum nextState, Action weaponStateCallback = null)
        {
            state = nextState;

            switch (state)
            {
                case WeaponBehaviorStateEnum.Idle:
                    Play("WeaponIdleAnimation_1");
                    ShowUI();
                    weaponOverallStatsContainer.ShowWeaponStats();
                    container.localPosition = Vector2.zero;
                    container.localRotation = Quaternion.identity;
                    break;
                case WeaponBehaviorStateEnum.Battle:
                    myAnim.Stop();
                    HideUI();
                    weaponOverallStatsContainer.HideWeaponStats();
                    break;
                case WeaponBehaviorStateEnum.FromChest:
                    MoveToPosition(Vector2.zero);
                    Play("WeaponFromChest", () => SetWeaponState(WeaponBehaviorStateEnum.Idle));
                    break;
                case WeaponBehaviorStateEnum.ToBattlePosition:
                    Play("WeaponGoToPosition_1", () =>
                    {
                        if(weaponStateCallback != null)
                        {
                            weaponStateCallback.Invoke();
                        }
                        currentWeapon.SetWeaponDetection(true);
                    });

                    HideUI();

                    container.localPosition = Vector2.zero;
                    container.localRotation = Quaternion.identity;
                    break;
                case WeaponBehaviorStateEnum.ToIdlePosition:
                    currentWeapon.SetWeaponDetection(false);
                    currentWeapon.transform.localPosition = Vector2.zero;
                    currentWeapon.transform.localRotation = Quaternion.identity;
                    MoveToPosition(Vector2.zero, () => SetWeaponState(WeaponBehaviorStateEnum.Idle));
                    break;
                default:
                    break;
            }
        }
        public void SetWeaponData(WeaponData weaponData)
        {
            dataBehavior.weaponData = new WeaponData(weaponData);
            weaponSlotsContainer.SetupSkillSlots(weaponData);
            weaponOverallStatsContainer.SetWeaponStats(weaponData);

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

        internal void ResetWeaponCallbacks()
        {
            forcedPositionReached.Clear();
            currentWeapon.ResestActons();
        }

        internal void ResetWeaponPhysics()
        {
            currentWeapon.weaponMovement.ResetTorque();
        }

        private void positionReached()
        {
            forcedToPosition = false;
            forcedPositionReached.ForEach(x => x.Invoke());
            forcedPositionReached.Clear();
        }

        public void PrepareWeaponDeath()
        {
            ResetWeaponCallbacks();
            ResetWeaponPhysics();
        }
    }
}