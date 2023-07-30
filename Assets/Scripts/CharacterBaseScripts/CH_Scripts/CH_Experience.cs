using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH_Experience : MonoBehaviour
{
    [field:SerializeField] public float Experience { get; private set; }
    [SerializeField] private float ExpTillNextLevel;
    private const float expGraphMultiplier = 20f;
    private const float baseExpTillNextLevelValue = 20f;

    private float previousLvlExp;

    [SerializeField, ContextMenuItem("TakeExp", "CheatExpGain")] private float TakeThisExp;

    private CH_Stats stats;

    public event Action<float, float, float> OnExpGain;

    public void Initialize(CH_Stats stats)
    {
        this.stats = stats;
        SetNextLevelUpExp();
    }

    public void GainExp(float amount)
    {
        var value = amount * stats.CurrentExperienceMultiplier;

        Experience += value;

        ExpTillNextLevel -= value;

        if (ExpTillNextLevel <= 0f)
        {
            stats.LevelUp();
            SetNextLevelUpExp();
        }

        OnExpGain?.Invoke(Experience, ExpTillNextLevel, previousLvlExp);
    }

    private void SetNextLevelUpExp()
    {
        ExpTillNextLevel = ((stats.CurrentLevel + 1) * (stats.CurrentLevel + 1) * expGraphMultiplier) - Experience;

        previousLvlExp = (stats.CurrentLevel == 0) ? 0 : stats.CurrentLevel * stats.CurrentLevel * expGraphMultiplier;
    }

    private void CheatExpGain()
    {
        GainExp(TakeThisExp);
    }
}
