using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Database;
using TMPro;

public class ChallengeConditionsManager : MonoBehaviour
{
    [SerializeField] ChallengesManager challengesManager;
    [SerializeField] TextMeshProUGUI killsText;
    [SerializeField] TextMeshProUGUI timerText;

    private EnemySpawner enemySpawner;
    private GameFlowManager gameFlowManager;
    private GameStatistics gameStatistics;

    private readonly Timer timer = new();

    private bool conditionTimeIsSet;
    private bool conditionTimePass = true;

    private int conditionKillsCount;
    private bool conditionKillsIsSet;
    private bool conditionKillsPass = true;

    private void Start()
    {
        enemySpawner = EnemySpawner.Instance;
        gameFlowManager = GameFlowManager.Instance;
        gameStatistics = GameStatistics.instance;

        gameStatistics.OnEnemyKill += OnEnemyKill;
        timer.OnCountdownEnd += OnCountdownEnd;
    }

    private void OnDestroy()
    {
        gameStatistics.OnEnemyKill -= OnEnemyKill;
        timer.OnCountdownEnd -= OnCountdownEnd;
    }

    private void Update()
    {
        if (gameFlowManager.IsGamePaused) { return; }

        if (conditionTimeIsSet)
        {
            timer.UpdateTime();

            timerText.text = $"Survive {timer.GetCountdownInString()}";
        }
    }

    public void ApplyCondition(ChallengeCondition condition)
    {
        condition.SetCondition(this, condition);
    }

    private void OnConditionPass()
    {
        challengesManager.OnConditionPass();
    }

    private void ConditionsCheck()
    {
        if (conditionKillsPass && conditionTimePass)
        {
            OnConditionPass();
        }
    }

    public void SetConditionTime(float time)
    {
        timer.SetCountdouwnActive(time);
        conditionTimeIsSet = true;
        conditionTimePass = false;
    }

    public void SetConditionKills(int kills)
    {
        conditionKillsCount = kills;
        conditionKillsIsSet = true;
        conditionKillsPass = false;
        killsText.text = $"Kills {conditionKillsCount}";
    }

    private void OnEnemyKill()
    {
        if (conditionKillsIsSet)
        {
            conditionKillsCount--;
            killsText.text = $"Kills {conditionKillsCount}";

            if (conditionKillsCount <= 0)
            {
                conditionKillsPass = true;
                conditionKillsIsSet = false;
                ConditionsCheck();
            }
        }
    }

    private void OnCountdownEnd()
    {
        conditionTimePass = true;
        conditionTimeIsSet = false;
        ConditionsCheck();
    }
}
