using DataManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SocialPlatforms;
using User.Data;

public class SpiritCondensationContainer : AnimationMonoBehavior
{
    public static SpiritCondensationContainer Instance;

    public bool startCondensing = false;
    public bool spiritCondensingOngoing = false;
    public SpriteRenderer blackBackground;

    public List<FloatingSpiritBehavior> currentSpirits = new List<FloatingSpiritBehavior>();

    float randomInterval = 0.0f;
    float currentInterval = 0.0f;

    int currentSpiritIdx = 0;
    List<SpiritEnhancement> statsList;
    Dictionary<WeaponStatEnum, float> successfulCondensedSpirits = new Dictionary<WeaponStatEnum, float>();

    public SpiritCondensation currentCondensation;
    public void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        if (startCondensing)
        {
            currentInterval += Time.deltaTime;
            if(currentInterval > randomInterval)
            {
                SetNewRandomInternal();
                currentInterval = 0.0f;

                SummonFloatingSpirit();
            }
        }
    }

    public void PrepareSpiritCondensation(float condensedSpiritAmount)
    {
        GameManager.Instance.SetUserInterface(GameStateEnum.Condensing);

        WeaponData weaponData = UserDataBehavior.GetPlayerEquippedWeapon();
        SpiritCondensation newCondensation = new SpiritCondensation(weaponData, condensedSpiritAmount);
        currentSpiritIdx = 0;

        newCondensation.GenerateCondensation();

        UserDataBehavior.AddSpiritCondensation(newCondensation);

        Play("BlackbackgroundShow", () => StartSpiritCondensation());
    }

    private void StartSpiritCondensation()
    {  
        currentCondensation = new SpiritCondensation(UserDataBehavior.GetUserCurrentCondensation());

        statsList = new List<SpiritEnhancement>(currentCondensation.potentialSpiritEnhancements);

        startCondensing = true;

        SetNewRandomInternal();
    }

    private void SuccessfullyCondensed(WeaponStatEnum thisStat, float amount, FloatingSpiritBehavior thisSpirit)
    {
        successfulCondensedSpirits.Add(thisStat, amount);

        if (currentSpirits.Find(x => x.isMoving) == null && (currentSpiritIdx == statsList.Count))
        {
            IncreasePlayerStats();
            EndCondensation();
        }
    }

    private void IncreasePlayerStats()
    {
        foreach (WeaponStatEnum spirit in successfulCondensedSpirits.Keys)
        {
            UserDataBehavior.IncreaseCurrentEquippedWeaponStat(spirit, successfulCondensedSpirits[spirit]);
        }

        successfulCondensedSpirits.Clear();
        currentSpirits.Clear();

        UserDataBehavior.RemoveCurrentSpiritCondensation();
    }

    internal void SummonFloatingSpirit()
    {
        FloatingSpiritBehavior floatingSpirit = PrefabManager.Instance.CreateFloatingSpirit(Vector2.zero).GetComponent<FloatingSpiritBehavior>();

        currentSpirits.Add(floatingSpirit);
        floatingSpirit.InitializeSprit(statsList[currentSpiritIdx].WeaponStat, statsList[currentSpiritIdx].EnhancementAmount, SuccessfullyCondensed);
        currentSpiritIdx++;

        if (currentSpiritIdx >= statsList.Count)
        {
            startCondensing = false;
        }
    }

    private void EndCondensation()
    {
        if(currentSpirits.Count > 0)
        {
            currentSpirits.ForEach(x => Destroy(x.gameObject));
            currentSpirits.Clear();
        }

        Play("BlackbackgroundHide", () => GameManager.Instance.SetGameState(GameStateEnum.Idle));
    }

    internal void RemoveFloatingSpiritNoIncreaseStats(FloatingSpiritBehavior thisSpirit)
    {
        currentSpirits.Remove(thisSpirit);
        Destroy(thisSpirit.gameObject);
        
        if (currentSpirits.Find(x => x.isMoving) == null && (currentSpiritIdx == statsList.Count))
        {
            currentSpirits.Clear();
            EndCondensation();
        }
    }

    private void SetNewRandomInternal()
    {
        randomInterval = (float)UnityEngine.Random.Range(1, 2);
    }
}
