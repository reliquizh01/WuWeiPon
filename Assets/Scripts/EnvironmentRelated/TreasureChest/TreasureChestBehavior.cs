using Interactable;
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
                        PrefabManager.instance.CreateWeaponContainer(this.transform.position);
                    }

                    UserDataBehavior.AddNewWeapon(weapons);
                }
            }
        }
    }
}
