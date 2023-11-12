using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using User.Data;

public class SkillRankButtonContainer : MonoBehaviour
{
    public SkillRankEnum buttonRank;
    public Image buttonImage;
    public Button rankBtn;
    public TextMeshProUGUI rankText;

    public void Awake()
    {

    }

    public void Start()
    {
        rankText.color = UniformityConverter.SkillRankToColor(buttonRank);
        rankText.text = UniformityConverter.SkillRankToString(buttonRank);
        UpdateButtonSetup();
    }

    public void UpdateButtonSetup()
    {
        List<SkillRankEnum> automatedSkillsOnUser = UserDataBehavior.GetAutomatedSkillRarity();

        if (automatedSkillsOnUser.Contains(buttonRank))
        {
            buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, 1.0f);
            rankText.color = new Color(rankText.color.r, rankText.color.g, rankText.color.b, 1.0f);
        }
        else
        {
            buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, 0.5f);
            rankText.color = new Color(rankText.color.r, rankText.color.g, rankText.color.b, 0.5f);
        }

    }
}
