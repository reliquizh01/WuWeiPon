using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using WeaponRelated;

public class BattleUserInterface : MonoBehaviour
{
    public Button speedUpButton;
    public TextMeshProUGUI speedupText;
    public CanvasGroup canvasGroup;
    public AnimationMonoBehavior battleCounterUi;

    public Button FightButton;

    public WeaponBattleInformation playerInformaton;
    public WeaponBattleInformation enemyInformation;

    public void Reset()
    {
        playerInformaton.ResetWeaponInformation();
        enemyInformation.ResetWeaponInformation();
    }

    internal void setupButtons(Action searchBattle)
    {
        FightButton.onClick.RemoveAllListeners();

        FightButton.onClick.AddListener(() => searchBattle.Invoke());
    }

    internal void setupSpeedUpButton(Action nextAction)
    {
        speedUpButton.onClick.RemoveAllListeners();

        speedUpButton.onClick.AddListener(() => nextAction.Invoke());
    }

    internal void SetVisibility(bool v)
    {
        if (v)
        {
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }
}