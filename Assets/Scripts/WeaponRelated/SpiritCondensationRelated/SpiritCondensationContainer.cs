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

    List<FloatingSpiritBehavior> currentSpirits = new List<FloatingSpiritBehavior>();
    List<FloatingSpiritBehavior> condensedSpirits = new List<FloatingSpiritBehavior>();

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

    public void ContinueLastTransaction()
    {
        PrepareSpiritCondensation(0);
    }

    public void PrepareSpiritCondensation(int condensedSpiritAmount)
    {
        GameManager.Instance.SetUserInterface(GameStateEnum.Condensing);

        WeaponData weaponData = UserDataBehavior.GetPlayerEquippedWeapon();
        currentSpiritIdx = 0;

        if(UserDataBehavior.GetUserCurrentCondensation() == null)
        {
            SpiritCondensation newCondensation = new SpiritCondensation(weaponData, condensedSpiritAmount);
            newCondensation.GenerateCondensation();
            UserDataBehavior.AddSpiritCondensation(newCondensation);
            UserDataBehavior.addSpiritualEssence(-condensedSpiritAmount);
        }

        Play("BlackbackgroundShow", () => StartSpiritCondensation());
    }

    private void StartSpiritCondensation()
    {
        currentCondensation = new SpiritCondensation(UserDataBehavior.GetUserCurrentCondensation());

        statsList = new List<SpiritEnhancement>(currentCondensation.potentialSpiritEnhancements);

        startCondensing = true;

        SetNewRandomInternal();
    }

    /// <summary>
    /// Adds the FloatingSpirit  to the list of successfully condensed Spirits.
    /// </summary>
    /// <param name="thisStat">stats</param>
    /// <param name="amount"></param>
    /// <param name="thisSpirit"></param>
    private void SuccessfullyCondensed(WeaponStatEnum thisStat, float amount, FloatingSpiritBehavior thisSpirit)
    {
        condensedSpirits.Add(thisSpirit);

        if (successfulCondensedSpirits.ContainsKey(thisStat)){
            successfulCondensedSpirits[thisStat] += amount;
        }
        else
        {
            successfulCondensedSpirits.Add(thisStat, amount);        
        }

        if(currentSpiritIdx >= statsList.Count)
        {
            if (currentSpirits[currentSpirits.Count-1] == thisSpirit)
            {
                CheckCondensationProgress();
            }
        }
    }

    private void IncreasePlayerStats()
    {
        foreach (WeaponStatEnum spirit in successfulCondensedSpirits.Keys)
        {
            UserDataBehavior.IncreaseCurrentEquippedWeaponStat(spirit, successfulCondensedSpirits[spirit]);
        }

        SpiritCondensationConfirmPopUpContainer.Instance.SetupStatResult(successfulCondensedSpirits);
        SpiritCondensationConfirmPopUpContainer.Instance.ShowStatResult();

        successfulCondensedSpirits.Clear();
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

    private void PrepareFinalCondensation()
    {
        FloatingSpiritBehavior lastSpirit = currentSpirits[currentSpirits.Count - 1];

        if (condensedSpirits.Count > 0 && (lastSpirit == null || !lastSpirit.isMoving && lastSpirit.isClicked))
        {
            currentSpirits.ForEach(x =>
            {
                if(x != null)
                {
                    x.SpinToCenter();
                }
            });
        }
    }

    private void EndCondensation()
    {
        currentSpirits.Clear();
        condensedSpirits.Clear();
        currentSpiritIdx = 0;

        IncreasePlayerStats();
        UserDataBehavior.RemoveCurrentSpiritCondensation();

    }

    internal void RemoveFloatingSpiritNoIncreaseStats(FloatingSpiritBehavior thisSpirit)
    {
        // If the final Spirit Is released, we can now check the progress.
        if(currentSpiritIdx >= statsList.Count)
        {
            if (currentSpirits[currentSpirits.Count-1] == thisSpirit)
            {
                currentSpirits[currentSpirits.Count - 1] = null;
                CheckCondensationProgress();
            }
        }


        Destroy(thisSpirit.gameObject);
    }

    private void CheckCondensationProgress()
    {
        // All Spirits have been summoned and were either Interacted or Destroyed.
        if(currentSpiritIdx >= statsList.Count)
        {
            PrepareFinalCondensation();
        }
    }

    internal void FloatingSpiritReachedCenter(FloatingSpiritBehavior thisSpirit)
    {
        condensedSpirits.Remove(thisSpirit);
        Destroy(thisSpirit.gameObject);

        if(condensedSpirits.Count <= 0)
        {
            EndCondensation();
        }
    }

    private void SetNewRandomInternal()
    {
        randomInterval = (float)UnityEngine.Random.Range(1, 2);
    }
}
