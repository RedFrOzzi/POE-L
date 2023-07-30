using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Database;
using UnityEngine.EventSystems;
using System.Text;

public class Challenge_UI_Element : MonoBehaviour, IPointerClickHandler
{
    public int Index { get; private set; }

    private ChallengesManager challengesManager;

    public ChallengeMod[] ChallengeMods { get; private set; } //максимум 6 модов
    public ChallengeReward ChallengeReward { get; private set; }
    public ChallengeCondition ChallengeCondition { get; private set; }

    [field: SerializeField] public Image BGImage { get; private set; }
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI challengeModsText;
    [SerializeField] private TextMeshProUGUI challengeRewardText;
    [SerializeField] private TextMeshProUGUI challengeConditionText;
    [SerializeField] private RectTransform challengesPanel;

    private readonly StringBuilder strBldr = new();
    
    private void Awake()
    {
        ChallengeMods = new ChallengeMod[6];
        BGImage = GetComponent<Image>();
    }

    public void SetUpManagerRef(ChallengesManager challengesManager)
    {
        this.challengesManager = challengesManager;
    }

    public void SetUpChallenge(int index, ChallengeMod[] challengeMods, ChallengeReward reward, ChallengeCondition condition)
    {
        Index = index;
        this.ChallengeMods = challengeMods;
        ChallengeReward = reward;
        ChallengeCondition = condition;

        SetUpModsText();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        challengesManager.SelectCard(Index);
    }

    private void SetUpModsText()
    {
        strBldr.Clear();

        foreach (ChallengeMod mod in ChallengeMods)
        {
            strBldr.Append($"{mod.Description?.Invoke(mod)}\n\n");
        }

        challengeModsText.text = strBldr.ToString();

        challengeConditionText.text = $"Condition: {ChallengeCondition.Description?.Invoke(ChallengeCondition)}";

        challengeRewardText.text = $"Reward: {ChallengeReward.Description?.Invoke(ChallengeReward)}";
    }
}
