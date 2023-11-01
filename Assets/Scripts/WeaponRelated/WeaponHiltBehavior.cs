using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WeaponRelated
{
    public class WeaponHiltBehavior : MonoBehaviour
    {
        public AudioSource sfx;
        internal bool isPlayingSFX = false;

        public Collider2D myCollision;

        private bool CanDetectCollision = false;
        private List<Action> OnDamageReceived = new List<Action>();

        internal WeaponBehavior m_behavior;

        public void OnCollisionEnter2D(Collision2D col)
        {
            WeaponBehavior opposingWeapon = col.gameObject.GetComponent<WeaponBehavior>();

            if(opposingWeapon != null )
            {
                WeaponHiltBehavior hilt = opposingWeapon.weaponHilts.FirstOrDefault(x => x.myCollision == col.collider);

                if (hilt != null)
                {
                    //SFX
                    if (!hilt.isPlayingSFX)
                    {
                        PlayHiltToHiltImpact();
                    }
                }
            }
        }

        public void OnCollisionExit2D(Collision2D col)
        {
            WeaponBehavior opposingWeapon = col.gameObject.GetComponent<WeaponBehavior>();

            if(opposingWeapon != null)
            {
                WeaponHiltBehavior hilt = opposingWeapon.weaponHilts.FirstOrDefault(x => x.myCollision == col.collider);
                WeaponBladeBehavior blade = opposingWeapon.weaponBlades.FirstOrDefault(x => x.myCollision == col.collider);

                if (hilt != null)
                {
                    if (isPlayingSFX)
                    {
                        isPlayingSFX = false;
                    }
                }
                else if (blade != null)
                {
                    //SFX
                    PlayHiltToBladeImpact();
                }
                else
                {
                    //Skill Collision

                    //CallbackOnceBladeHItsOpposingBlade

                    //SFX
                    PlayBladeToWeaponImpact();
                }
            }
        }

        public void PlayBladeToWeaponImpact()
        {
            int random = UnityEngine.Random.Range(0, SoundManager.Instance.hiltToHiltClips.Count);
            sfx.clip = SoundManager.Instance.hiltToHiltClips[random];
            sfx.Play();
        }

        public void PlayHiltToBladeImpact()
        {
            int random = UnityEngine.Random.Range(0, SoundManager.Instance.bladeToHiltClips.Count);
            sfx.clip = SoundManager.Instance.bladeToHiltClips[random];
            sfx.Play();
        }

        public void PlayHiltToHiltImpact()
        {
            int random = UnityEngine.Random.Range(0, SoundManager.Instance.hiltToHiltClips.Count);
            sfx.clip = SoundManager.Instance.hiltToHiltClips[random];
            sfx.Play();

            isPlayingSFX = true;

            StartCoroutine(SetPlayingSFXToFalse(sfx.clip.length));
        }

        IEnumerator SetPlayingSFXToFalse(float lastSfxLength)
        {
            yield return new WaitForSeconds(lastSfxLength);

            isPlayingSFX = false;
        }
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
}