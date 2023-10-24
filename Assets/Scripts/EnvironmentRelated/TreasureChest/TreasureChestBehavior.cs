using Interactable;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using User.Data;

namespace PlayerPulls.Chest
{
    public class TreasureChestBehavior : InteractableItem
    {
        internal TreasureChestData treasureChestData;

        public void Start()
        {

        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

            GiveTreasureToPlayer();

            DestroyImmediate(this.gameObject);
        }

        public void GiveTreasureToPlayer()
        {
            if(treasureChestData.containedWeapon.Count > 0)
            {
                foreach(WeaponData weapons in treasureChestData.containedWeapon)
                {
                    if(UserDataBehavior.GetPlayerEquippedWeapon() == null)
                    {
                        WeaponContainer behavior = PrefabManager.Instance.CreateWeaponContainer(this.transform.position).GetComponent<WeaponContainer>();
                        GameManager.Instance.equippedWeaponContainer = behavior;

                        Action afterGoingToCenter = () =>
                        {
                            behavior.SetWeaponState(WeaponBehaviorStateEnum.Idle);
                        };

                        behavior.MoveToPosition(Vector2.zero, afterGoingToCenter);
                        behavior.SetWeaponState(WeaponBehaviorStateEnum.FromChest);    

                        weapons.isEquipped = true;
                    }

                    UserDataBehavior.AddNewWeapon(weapons);
                }
            }
        }
    }
}
