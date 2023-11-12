
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InformationPopUpContainer : MonoBehaviour
{
    public static InformationPopUpContainer Instance;

    public GameObject Container;

    public WeaponSkillSlot skillSlot;

    public TextMeshProUGUI rarityText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI statsText;

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void SetupSkillInformation(SkillData skillData)
    {
        if(skillData == null)
        {
            return;
        }

        Container.SetActive(true);

        skillSlot.SetupCurrentSkillAttached(skillData);
        SkillEnum skillEnum = UniformityConverter.SkillStringToEnum(skillData.skillName);

        rarityText.text = UniformityConverter.SkillRankToString(skillData.skillRank);
        rarityText.color = UniformityConverter.SkillRankToColor(skillData.skillRank);

        levelText.text = "Level " + skillData.skillLevel.ToString();
        nameText.text = UniformityConverter.SkillStringToSkillName(skillData.skillName);
        descriptionText.text = UniformityConverter.SkillNameToStatDescription(skillEnum, skillData);
    }

    public void HideInformation()
    {
        Container.SetActive(false);
    }
}