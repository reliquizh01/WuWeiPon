using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using User.Data;

public class SpiritCondensationConfirmPopUpContainer : MonoBehaviour
{
    public static SpiritCondensationConfirmPopUpContainer Instance;

    public GameObject raycastBlocker;

    public GameObject container;
    public Button confirmBtn;

    public List<ConfirmationStatsContainer> statReceiptList;

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        confirmBtn.onClick.AddListener(HideStatResult);
    }

    public void ShowStatResult()
    {
        raycastBlocker.SetActive(true);
        container.SetActive(true);
    }

    public void HideStatResult()
    {
        ResetStatResult();
        raycastBlocker.SetActive(false);
        container.SetActive(false);

        SpiritCondensationContainer.Instance.Play("BlackbackgroundHide", () => GameManager.Instance.SetGameState(GameStateEnum.Idle));
    }

    public void ResetStatResult()
    {
        statReceiptList.ForEach(x => x.gameObject.SetActive(false));
    }

    public void SetupStatResult(Dictionary<WeaponStatEnum, float> addedAmount)
    {
        List<WeaponStatEnum> keys = addedAmount.Keys.ToList();
        
        foreach (WeaponStatEnum key in keys)
        {
            ConfirmationStatsContainer thisStat = statReceiptList.FirstOrDefault(x => x.weaponStatEnum == key);

            if(thisStat != null)
            {
                thisStat.gameObject.SetActive(true);
                float curAmount = UserDataBehavior.GetCurrentEquippedWeaponStat(key);
                float previousAmount = curAmount - addedAmount[key];

                thisStat.SetStatContainerValues(previousAmount, curAmount);
            }
            else
            {
                Debug.Log("Stats:" + key + " has no stat container!");
            }
        }
    }
}
