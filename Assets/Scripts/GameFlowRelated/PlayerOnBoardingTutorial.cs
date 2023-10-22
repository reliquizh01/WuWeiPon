using System;
using DataManagement.Adapter;
using Interactable;
using PlayerPulls.Chest;
using UnityEngine;
using User.Data;
using WeaponRelated;

public class PlayerOnBoardingTutorial
{
    #region Constructor

    public PlayerOnBoardingTutorial(GameManager instance, PlayerOnBoardingEnum nextOnboarding)
    {
        gameManager = instance;

        PlayOnBoardingSequence(nextOnboarding);
    }

    #endregion Constructor

    #region References

    GameManager gameManager;

    #endregion References

    public void PlayOnBoardingSequence(PlayerOnBoardingEnum sequence)
    {
        switch(sequence)
        {
            case PlayerOnBoardingEnum.FirstWeaponOpening:
                FirstWeaponOpening();
                break;
        }
    }

    public void FirstWeaponOpening()
    {
        GameObject chest = PrefabManager.Instance.CreateTreasureChest(new Vector2(0.5f, -7.0f), gameManager.EnvironmentItems.transform);


        //Weapon is created
        WeaponGenerator generator = new WeaponGenerator();
        WeaponData firstWeapon = generator.GenerateWeapon(WeaponRankEnum.ordinary);


        TreasureChestData fixedTreasure = new TreasureChestData();
        fixedTreasure.containedWeapon.Add(firstWeapon);
        
        TreasureChestBehavior treasureBehavior = chest.GetComponent<TreasureChestBehavior>();
        treasureBehavior.treasureChestData = fixedTreasure;


        // Implement Interaction
        InteractableItem interactable = chest.GetComponent<InteractableItem>();

        Action addOnboardingToUser = () =>
        {
            UserDataBehavior.AddOnboardingProgress(PlayerOnBoardingEnum.FirstWeaponOpening);

            IntroductionWeaponStats();
        };

        interactable.interactionAction[InteractionEnum.OnClick].Add(addOnboardingToUser);
    }

    public void IntroductionWeaponStats()
    {

    }
}