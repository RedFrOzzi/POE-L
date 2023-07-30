using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Database;
using System.Reflection;
using System;
using TMPro;
using UnityEngine.EventSystems;
using System.Linq;

public class ChallengesManager : MonoBehaviour
{
    public GlobalEnemyModifiers GlobalEnemyModifiers { get; private set; }
    [SerializeField] private ChallengeConditionsManager challengeConditionsManager;
    [SerializeField] private GameObject challengeModsPanel;
    [SerializeField] private Challenge_UI_Element[] challenge_UI_Elements;
    [SerializeField] private EventTrigger acceptButton;
    private GameObject player;
    public CH_Stats Stats { get; private set; }

    private ChallengeModsDatabase chalengeModsDatabase;
    private ChallengeRewardsDatabase challengeRewardsDatabase;
    private ChallengeConditionsDatabase challengeConditionsDatabase;

    private readonly ChallengeMod[] currentChallengeMods = new ChallengeMod[6]; //максимум в 6 модов
    private ChallengeReward currentReward;

    [Space(10)]
    [SerializeField] private TextMeshProUGUI leftTextField;
    [SerializeField] private TextMeshProUGUI rightTextField;

    private int? selectedCardIndex = null;

    private static float maxPosibleModValue;

    private const float hideAlpha = 0.67f;
    private const float showAlpha = 1;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Stats = player.GetComponent<CH_Stats>();
    }

    private void Start()
    {
        foreach (Challenge_UI_Element element in challenge_UI_Elements)
        {
            element.SetUpManagerRef(this);
        }

        GlobalEnemyModifiers = GlobalEnemyModifiers.Instance;

        chalengeModsDatabase = GameDatabasesManager.Instance.ChalengeModsDatabase;
        challengeRewardsDatabase = GameDatabasesManager.Instance.ChallengeRewardsDatabase;
        challengeConditionsDatabase = GameDatabasesManager.Instance.ChallengeConditionsDatabase;

        EventTrigger.Entry entry = new();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((eventData) => AcceptCard());
        acceptButton.triggers.Add(entry);

        challengeModsPanel.SetActive(false);
    }

    private void ApplyChallengeCard(ChallengeMod[] challengeMods, ChallengeReward challengeReward, ChallengeCondition challengeCondition)
    {
        SetChallengeCondition(challengeCondition);

        ApplyChoosenChallangeMods(challengeMods);

        SetChoosenReward(challengeReward);

        HideChallenges();

        GameFlowManager.Instance.OnChallengeChoosen();
    }

    public void OnConditionPass()
    {
        GiveReward();

        RemovePreviousChallengeMods();

        ShowChallenges();

        GameFlowManager.Instance.OnChallangeComplete();
    }

    private void SetChallengeCondition(ChallengeCondition challengeCondition)
    {
        challengeConditionsManager.ApplyCondition(challengeCondition);
    }

    private void SetChoosenReward(ChallengeReward challengeReward)
    {
        currentReward = challengeReward;
    }

    private void GiveReward()
    {
        if (currentReward == null || currentReward.GiveReward == null) { return; }

        currentReward.GiveReward(Stats, currentReward);
    }

    public void SelectCard(int index)
    {
        UnselectCards(index);

        challenge_UI_Elements[index].BGImage.SetTransparency(showAlpha);

        selectedCardIndex = index;
    }

    private void UnselectCards(int exceptIndex)
    {
        foreach (var card in challenge_UI_Elements)
        {
            if (card.Index == exceptIndex)
                continue;

            card.BGImage.SetTransparency(hideAlpha);
        }
    }

    private void AcceptCard()
    {
        if (selectedCardIndex == null) { return; }

        ApplyChallengeCard(challenge_UI_Elements[(int)selectedCardIndex].ChallengeMods, challenge_UI_Elements[(int)selectedCardIndex].ChallengeReward, challenge_UI_Elements[(int)selectedCardIndex].ChallengeCondition);

        selectedCardIndex = null;
    }

    private void ApplyChoosenChallangeMods(ChallengeMod[] challengeMods)
    {
        GlobalEnemyModifiers.ResetStatsChanges();

        for (int i = 0; i < currentChallengeMods.Length; i++)
        {
            currentChallengeMods[i] = null;
        }

        for (int i = 0; i < challengeMods.Length; i++)
        {
            if (challengeMods[i] == null) { continue; }

            currentChallengeMods[i] = challengeMods[i];

            currentChallengeMods[i].ApplyMod?.Invoke(Stats, GlobalEnemyModifiers);
        }

        SetChallengeModsText();

        GlobalEnemyModifiers.ApplyNewStatsToAliveEnemies();
    }

    private void RemovePreviousChallengeMods()
    {
        GlobalEnemyModifiers.ResetStatsChanges();

        for (int i = 0; i < currentChallengeMods.Length; i++)
        {
            if (currentChallengeMods[i] == null) { continue; }

            currentChallengeMods[i].RemoveMod?.Invoke(Stats, GlobalEnemyModifiers);

            currentChallengeMods[i] = null;
        }

        GlobalEnemyModifiers.RemovePreviousStatsFromAliveEnemies();
    }    

    private void GenerateChallanges()
    {
        TestModsGenerating();
    }

    public void ShowChallenges()
    {
        GenerateChallanges();

        challengeModsPanel.SetActive(true);
    }

    private void HideChallenges()
    {
        challengeModsPanel.SetActive(false);
    }

    private void SetChallengeModsText()
    {
        leftTextField.text = $"{currentChallengeMods[0].Description?.Invoke(currentChallengeMods[0])}\n {currentChallengeMods[1].Description?.Invoke(currentChallengeMods[1])}\n {currentChallengeMods[2].Description?.Invoke(currentChallengeMods[2])}";

        rightTextField.text = $"{currentChallengeMods[3].Description?.Invoke(currentChallengeMods[3])}\n {currentChallengeMods[4].Description?.Invoke(currentChallengeMods[4])}\n {currentChallengeMods[5].Description?.Invoke(currentChallengeMods[5])}";
    }

    private float GetCurrentModsValuePercent(ChallengeMod[] mods)
    {
        float value = GetTotalChallangeModsValue(mods) / GetMaxPosibleModValue();

        return value;
    }

    private float GetMaxPosibleModValue()
    {
        if (maxPosibleModValue != 0)
            return maxPosibleModValue;

        List<float> values = new();

        foreach (var mod in chalengeModsDatabase.ChallengeModsList)
        {
            if (mod.GetChallangeValue == null)
                continue;

            values.Add(mod.GetHightestChallengeValue());
        }

        maxPosibleModValue = values.Max() * currentChallengeMods.Length;

        return maxPosibleModValue;
    }

    private float GetTotalChallangeModsValue(ChallengeMod[] mods)
    {
        float totalValue = 0;
        foreach (var mod in mods)
        {
            if (mod.GetChallangeValue == null)
                continue;

            totalValue += mod.GetChallangeValue(mod.Tier);
        }
        return totalValue;
    }

    private void TestModsGenerating()
    {
        int cardsAmount = challenge_UI_Elements.Length;

        for (int i = 0; i < cardsAmount; i++)
        {
            if (i != 0)
            {
                ChallengeMod[] challengeMods = new ChallengeMod[currentChallengeMods.Length];

                for (int j = 0; j < challengeMods.Length; j++)
                {
                    //50% to take empty mod
                    if (UnityEngine.Random.value < 0.5f)
                    {
                        challengeMods[j] = chalengeModsDatabase.ChallengeMods["Empty"].GetCopy();
                    }
                    else
                    {
                        challengeMods[j] = chalengeModsDatabase.ChallengeModsList.PickRandom().GetCopy();
                    }

                    float oneTierPercent = (challengeMods[j].TierValues.Length > 0) ? 1 / (float)challengeMods[j].TierValues.Length : float.MaxValue;

                    byte index = 1;
                    for (int k = challengeMods[j].TierValues.Length - 1; k >= 0; k--)
                    {
                        if ((float)i / (float)cardsAmount < oneTierPercent * index)
                        {
                            int length = (challengeMods[j].TierValues.Length > 0) ? challengeMods[j].TierValues.Length : 1;

                            byte tier = (byte)Mathf.Clamp(UnityEngine.Random.Range(k - 2, k + 2), 0, length - 1);

                            challengeMods[j].SetTier(tier);
                            break;
                        }

                        index++;
                    }
                }

                ChallengeReward challengeReward = challengeRewardsDatabase.ChallengeRewardsList.PickRandom().GetCopy();

                if (challengeReward.TierValues != null)
                {

                    float currentModsValue = GetCurrentModsValuePercent(challengeMods);

                    float oneTierPercent = 1 / (float)challengeReward.TierValues.Length;

                    //Ќайти и установить тир дл€ награды
                    byte index = 1;
                    for (int j = challengeReward.TierValues.Length - 1; j >= 0; j--)
                    {
                        
                        if (currentModsValue < oneTierPercent * index)
                        {
                            byte tier = (byte)Mathf.Clamp(UnityEngine.Random.Range(j - 1, j + 1), 0, challengeReward.TierValues.Length - 1);

                            challengeReward.SetTier(tier);
                            break;
                        }

                        index++;
                    }
                }

                ChallengeCondition challengeCondition = GetCondition(i, cardsAmount);

                challenge_UI_Elements[i].SetUpChallenge(i, challengeMods, challengeReward, challengeCondition);
            }
            else
            {
                ChallengeMod[] challengeMods = new ChallengeMod[currentChallengeMods.Length];

                for (int j = 0; j < challengeMods.Length; j++)
                {
                    challengeMods[j] = chalengeModsDatabase.ChallengeMods["Empty"].GetCopy();
                }

                ChallengeReward challengeReward = challengeRewardsDatabase.ChallengeRewards["Empty"];

                ChallengeCondition challengeCondition;

                if (UnityEngine.Random.value < 0.5f)
                    challengeCondition = challengeConditionsDatabase.ChallengeConditions["TimeCondition"].GetCopy().SetTier((byte)Mathf.Abs(i - (cardsAmount - 1)));
                else
                    challengeCondition = challengeConditionsDatabase.ChallengeConditions["TimeCondition"].GetCopy().SetTier((byte)Mathf.Abs(i - (cardsAmount - 1)));

                challenge_UI_Elements[i].SetUpChallenge(i, challengeMods, challengeReward, challengeCondition);
            }
        }
    }

    private ChallengeCondition GetCondition(int currentCard, int cardsAmount)
    {
        //ChallengeCondition challengeCondition;

        //float rnd = UnityEngine.Random.value;

        //if (currentCard > 0 && currentCard < cardsAmount - 1)
        //{
        //    if (rnd < 0.333f)
        //        challengeCondition = challengeConditionsDatabase.ChallengeConditions["TimeCondition"].GetCopy().SetTier((byte)Mathf.Abs(currentCard - (cardsAmount - 1)));
        //    else if (rnd < 0.666f)
        //        challengeCondition = challengeConditionsDatabase.ChallengeConditions["KillsCondition"].GetCopy().SetTier((byte)Mathf.Abs(currentCard - (cardsAmount - 1)));
        //    else
        //        challengeCondition = challengeConditionsDatabase.ChallengeConditions["TimeAndKillsCondition"].GetCopy().SetTier((byte)Mathf.Abs(currentCard - 1 - (cardsAmount - 1)));
        //}
        //else
        //{
        //    if (rnd < 0.333f)
        //        challengeCondition = challengeConditionsDatabase.ChallengeConditions["TimeCondition"].GetCopy().SetTier((byte)Mathf.Abs(currentCard - (cardsAmount - 1)));
        //    else if (rnd < 0.666f)
        //        challengeCondition = challengeConditionsDatabase.ChallengeConditions["KillsCondition"].GetCopy().SetTier((byte)Mathf.Abs(currentCard - (cardsAmount - 1)));
        //    else
        //        challengeCondition = challengeConditionsDatabase.ChallengeConditions["TimeAndKillsCondition"].GetCopy().SetTier((byte)Mathf.Abs(currentCard - (cardsAmount - 1)));
        //}

        //return challengeCondition;
        return challengeConditionsDatabase.ChallengeConditions["TimeCondition"].GetCopy().SetTier((byte)Mathf.Abs(currentCard - (cardsAmount - 1)));
    }
}
