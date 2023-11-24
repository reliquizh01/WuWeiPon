using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace WeaponRelated
{

    public class WeaponBladeBehavior : BaseWeaponPartsBehavior
    {
        public AudioSource sfx;
        internal bool isPlayingSFX = false;

        public Collider2D myCollision;

        private bool CanDetectCollision = false;
        private List<Action<float>> callBackOnceBladeHitsOpposingHilt = new List<Action<float>>();
        private List<Action<float>> callBackForChangesInSelfHealth = new List<Action<float>>();

        public void OnCollisionEnter2D(Collision2D col)
        {
            WeaponBehavior opposingWeapon = col.gameObject.GetComponent<WeaponBehavior>();

            if(opposingWeapon != null)
            {
                WeaponHiltBehavior hilt = opposingWeapon.weaponHilts.FirstOrDefault(x => x.myCollision == col.collider);
                WeaponBladeBehavior blade = opposingWeapon.weaponBlades.FirstOrDefault(x => x.myCollision == col.collider);

                if (hilt != null)
                {
                    m_behavior.nextDamageToInflict += m_behavior.currentDamage;

                    // Calls that are related to the Weapon Skill
                    if (OnDamageSkillCollisionActions.Count > 0)
                    {
                        List<DamageBattleSkillBehavior> copyOnSkillCollisionActions = new List<DamageBattleSkillBehavior>(OnDamageSkillCollisionActions);

                        foreach (DamageBattleSkillBehavior skill in copyOnSkillCollisionActions)
                        {
                            if (hilt != null && skill.IsObjectInListOfTargetValues(SkillTargetEnum.Hilt))
                            {
                                if (skill.hasDamageBonusByPercent)
                                {
                                    skill.EnhanceDamageToBeInflicted(ref m_behavior.nextDamageToInflict);
                                }

                                hilt.DamageReceived(m_behavior.nextDamageToInflict);
                            }
                            else if(blade != null && skill.IsObjectInListOfTargetValues(SkillTargetEnum.Blade))
                            {

                            }
                        }
                    }

                    if(OnMovementSkillCollisionActions.Count > 0)
                    {

                    }

                    if(OnHealSkillCollisionActions.Count > 0)
                    {
                        List<HealBattleSkillBehavior> copyOnHealBattleSkillBehavior = new List<HealBattleSkillBehavior>(OnHealSkillCollisionActions);

                        foreach (HealBattleSkillBehavior skill in copyOnHealBattleSkillBehavior)
                        {
                            if (hilt != null && skill.IsObjectInListOfTargetValues(SkillTargetEnum.Hilt))
                            {
                                if (skill.HealOnceOnCall)
                                {
                                    skill.HealWeaponBasedOnDamagePercentage(ref m_behavior.nextHealToInflict, ref m_behavior.nextDamageToInflict);
                                }
                            }
                        }
                    }

                    // Calls that are related to healing self
                    if(callBackForChangesInSelfHealth.Count > 0)
                    {
                        List<Action<float>> copyCallBackForChangesInSelfHealth = new List<Action<float>>(callBackForChangesInSelfHealth);

                        foreach (Action<float> action in copyCallBackForChangesInSelfHealth)
                        {
                            action.Invoke(m_behavior.nextHealToInflict);
                            if (callBackForChangesInSelfHealth.Count == 0)
                            {
                                break;
                            }
                        }

                        // Heal already provided, reset Next Heal to 0
                        m_behavior.nextHealToInflict = 0;
                    }

                    // Calls that are related to damaging the enemy weapon
                    if (callBackOnceBladeHitsOpposingHilt.Count > 0)
                    {
                        List<Action<float>> copyUpdateUiOnDamageDetection = new List<Action<float>>(callBackOnceBladeHitsOpposingHilt);

                        foreach (Action<float> action in copyUpdateUiOnDamageDetection)
                        {
                            action.Invoke(m_behavior.nextDamageToInflict);
                            if (callBackOnceBladeHitsOpposingHilt.Count == 0)
                            {
                                break;
                            }
                        }

                        // Damage Already Inflicted, reset Next Damage to 0
                        m_behavior.nextDamageToInflict = 0;
                    }
                }
                else if (blade != null)
                {
                    //Skill Collision

                    //CallbackOnceBladeHItsOpposingBlade

                    //SFX
                    if (!blade.isPlayingSFX)
                    {
                        PlayBladeToBladeImpact();
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
                    // Calls that are related to the Weapon Skill

                    // Calls that are related to damaging the enemy weapon

                    //SFX
                }
                else if (blade != null)
                {
                    //Skill Collision

                    //CallbackOnceBladeHItsOpposingBlade

                    //SFX
                    if (isPlayingSFX)
                    {
                        isPlayingSFX = false;
                    }
                }
                else if (col.gameObject.GetComponent<WeaponBehavior>() != null)
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
            if (sfx.enabled)
            {
                sfx.Play();
            }
        }

        public void PlayBladeToBladeImpact()
        {
            int random = UnityEngine.Random.Range(0, SoundManager.Instance.bladeToBladeClips.Count);
            sfx.clip = SoundManager.Instance.bladeToBladeClips[random];
            if (sfx.enabled)
            {
                sfx.Play();
            }

            isPlayingSFX = true;
            StartCoroutine(SetPlayingSFXToFalse(sfx.clip.length));
        }
        

        IEnumerator SetPlayingSFXToFalse(float lastSfxLength)
        {
            yield return new WaitForSeconds(lastSfxLength);

            isPlayingSFX = false;
        }

        public void SetCollisionDetection(bool setTo)
        {
            CanDetectCollision = setTo;

            if (!CanDetectCollision)
            {
                RemoveActionOnCollision();
            }
        }

        public void AddCallBackOnceBladeHitsHilt(Action<float> action)
        {
            callBackOnceBladeHitsOpposingHilt.Add(action);
        }

        public void AddCallBackForChangesInSelfHealth(Action<float> action)
        {
            callBackForChangesInSelfHealth.Add(action);
        }

        public void RemoveActionOnCollision()
        {
            callBackOnceBladeHitsOpposingHilt.Clear();
            OnDamageSkillCollisionActions.Clear();
        }

    }
}