using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

                break;
        }
    }
}