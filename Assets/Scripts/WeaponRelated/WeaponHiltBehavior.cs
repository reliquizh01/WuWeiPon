using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponRelated
{
    public class WeaponHiltBehavior : MonoBehaviour
    {
        public AudioSource sfx;
        internal bool isPlayingSFX = false;

        private bool CanDetectCollision = false;
        private List<Action> OnDamageReceived = new List<Action>();

        internal WeaponBehavior m_behavior;

        public void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.GetComponent<WeaponHiltBehavior>() != null)
            {
                WeaponHiltBehavior enemyHilt = col.gameObject.GetComponent<WeaponHiltBehavior>();
                //SFX
                if (!enemyHilt.isPlayingSFX)
                {
                    PlayHiltToHiltImpact();
                }
            }
            else if(col.gameObject.GetComponent<WeaponBladeBehavior>() != null)
            {
                //SFX
                PlayHiltToBladeImpact();
            }
        }

        public void OnTriggerExit2D(Collider2D col)
        {
            if (col.gameObject.GetComponent<WeaponHiltBehavior>() != null)
            {
                if (isPlayingSFX)
                {
                    isPlayingSFX = false;
                }
            }
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