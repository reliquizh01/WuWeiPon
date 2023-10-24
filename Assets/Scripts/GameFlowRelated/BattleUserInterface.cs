using UnityEngine;
using UnityEngine.UI;
using System;

public class BattleUserInterface : MonoBehaviour
{
    public Button FightButton;

    public WeaponBattleInformation playerInformaton;
    public WeaponBattleInformation enemyInformation;

    internal void setupButtons(Action searchBattle)
    {
        FightButton.onClick.RemoveAllListeners();

        FightButton.onClick.AddListener(() => searchBattle.Invoke());
    }
}