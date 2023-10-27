using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class BattleUserInterface : MonoBehaviour
{
    public Button speedUpButton;
    public TextMeshProUGUI speedupText;

    public Button FightButton;

    public WeaponBattleInformation playerInformaton;
    public WeaponBattleInformation enemyInformation;

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
}