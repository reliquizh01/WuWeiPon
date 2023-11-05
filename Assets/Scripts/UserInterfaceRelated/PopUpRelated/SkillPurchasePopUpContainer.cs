using TMPro;
using UnityEngine;
using UnityEngine.UI;
using User.Data;

public class SkillPurchasePopUpContainer : MonoBehaviour
{
    internal static SkillPurchasePopUpContainer Instance;

    public GameObject raycastBlocker;
    public GameObject popUpContainer;

    public TextMeshProUGUI purchaseMessageText;

    public Image currentSkillIcon;
    public Image purchasedSkillIcon;

    public Button currentSkillBtn;
    public Button purchasedSkillBtn;

    int currentSlotNumber;
    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void Start()
    {
        if(Instance != null)
        {
            Destroy(this);
        }
    }

    public void SetupSkillPurchase(SkillData currentSkillSlotData, SkillData purchasedSkillData, TrainingSkillSlotsBehavior skillSlotRef)
    {
        openSkillPurchase();
        currentSlotNumber = currentSkillSlotData.slotNumber;

        currentSkillIcon.sprite = DataVaultManager.Instance.GetSkillSprite(currentSkillSlotData.skillIconFileName);
        purchasedSkillIcon.sprite = DataVaultManager.Instance.GetSkillSprite(purchasedSkillData.skillIconFileName);

        purchaseMessageText.text = string.Format($"{ConstantMessageTexts.SKILL_PURCHASE_CONFIRM}", currentSkillSlotData.skillName, purchasedSkillData.skillName);

        currentSkillBtn.onClick.RemoveAllListeners();
        purchasedSkillBtn.onClick.RemoveAllListeners();

        currentSkillBtn.onClick.AddListener(()=> 
        {
            playerChooseCurrentSkill();
            skillSlotRef.SetupSkillSlot(currentSkillSlotData);
        });

        purchasedSkillBtn.onClick.AddListener(()=>
        {
            playerChoosePurchasedSkill();
            WeaponData weapon = UserDataBehavior.GetPlayerEquippedWeapon();
            skillSlotRef.SetupSkillSlot(weapon.skills[weapon.skills.Count - 1]);
        });
    }

    private void playerChooseCurrentSkill()
    {
        UserDataBehavior.PlayerChooseCurrentSkill(currentSlotNumber);
        closeSkillPurchase();
    }

    private void playerChoosePurchasedSkill()
    {
        UserDataBehavior.PlayerChooseNewSkill(currentSlotNumber);
        closeSkillPurchase();
    }

    private void openSkillPurchase()
    {
        raycastBlocker.SetActive(true);
        popUpContainer.SetActive(true);
    }

    private void closeSkillPurchase()
    {
        raycastBlocker.SetActive(false);
        popUpContainer.SetActive(false);
    }
}