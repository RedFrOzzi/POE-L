using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

public class MagicSC
{
    public event Action OnStatsChange;

    private enum StatsChangesType { Flat, Increase, More, Less };
    private static readonly PropertyInfo[] props = typeof(MagicSC).GetProperties(BindingFlags.Instance | BindingFlags.Public);
    private static readonly Dictionary<int, StatsChangesType> statsChangesType = new();
    private static bool isPropsSet = false;

    public MagicSC()
    {
        if (isPropsSet) { return; }

        for (int i = 0; i < props.Length; i++)
        {
            string[] split = Regex.Split(props[i].Name, @"(?<!^)(?=[A-Z])");

            switch (split[0])
            {
                case "Flat":
                    statsChangesType.Add(i, StatsChangesType.Flat);
                    break;
                case "Increase":
                    statsChangesType.Add(i, StatsChangesType.Increase);
                    break;
                case "More":
                    statsChangesType.Add(i, StatsChangesType.More);
                    break;
                case "Less":
                    statsChangesType.Add(i, StatsChangesType.Less);
                    break;
                default:
                    throw new Exception("There is unexpected property name");
            }
        }

        isPropsSet = true;
    }

    public void SwapChanges(MagicSC changes)
    {
        if (changes == null) { return; }
        if (changes == this) { throw new Exception("Can not copy from self"); }

        foreach (var prop in props)
        {
            if (prop.PropertyType == typeof(int))
            {
                prop.SetValue(this, (int)prop.GetValue(changes));
            }
            else
            {
                prop.SetValue(this, (float)prop.GetValue(changes));
            }
        }
    }

    public void CombineChanges(MagicSC changes)
    {
        if (changes == null) { return; }
        if (changes == this) { throw new Exception("Can not copy from self"); }

        for (int i = 0; i < props.Length; i++)
        {
            switch (statsChangesType[i])
            {
                case StatsChangesType.Flat:

                    if (props[i].PropertyType == typeof(int))
                        props[i].SetValue(this, (int)props[i].GetValue(this) + (int)props[i].GetValue(changes));
                    else
                        props[i].SetValue(this, (float)props[i].GetValue(this) + (float)props[i].GetValue(changes));

                    break;
                case StatsChangesType.Increase:
                    props[i].SetValue(this, (float)props[i].GetValue(this) + (float)props[i].GetValue(changes));
                    break;
                case StatsChangesType.More:
                    props[i].SetValue(this, (float)props[i].GetValue(this) * (float)props[i].GetValue(changes));
                    break;
                case StatsChangesType.Less:
                    props[i].SetValue(this, (float)props[i].GetValue(this) * (float)props[i].GetValue(changes));
                    break;
                default:
                    throw new Exception("There is unexpected property name");
            }
        }
    }

    public void RemoveChanges(MagicSC changes)
    {
        if (changes == null) { return; }
        if (changes == this) { throw new Exception("Can not copy from self"); }

        for (int i = 0; i < props.Length; i++)
        {
            switch (statsChangesType[i])
            {
                case StatsChangesType.Flat:

                    if (props[i].PropertyType == typeof(int))
                        props[i].SetValue(this, (int)props[i].GetValue(this) - (int)props[i].GetValue(changes));
                    else
                        props[i].SetValue(this, (float)props[i].GetValue(this) - (float)props[i].GetValue(changes));

                    break;
                case StatsChangesType.Increase:
                    props[i].SetValue(this, (float)props[i].GetValue(this) - (float)props[i].GetValue(changes));
                    break;
                case StatsChangesType.More:
                    props[i].SetValue(this, (float)props[i].GetValue(this) / (float)props[i].GetValue(changes));
                    break;
                case StatsChangesType.Less:
                    props[i].SetValue(this, (float)props[i].GetValue(this) / (float)props[i].GetValue(changes));
                    break;
                default:
                    throw new Exception("There is unexpected property name");
            }
        }
    }


    public int FlatAbilityLevelValue { get; private set; }
    public void AddFlatAbilityLevel(int value)
    {
        FlatAbilityLevelValue += value;
        OnStatsChange?.Invoke();
    }

    public int FlatSpellCritChanceValue { get; private set; }
    public float IncreaseSpellCritChanceValue { get; private set; }
    public float MoreSpellCritChanceValue { get; private set; } = 1f;
    public float LessSpellCritChanceValue { get; private set; } = 1f;
    public void AddSpellCritChanceFlat(int value)
    {
        FlatSpellCritChanceValue += value;
        OnStatsChange?.Invoke();
    }
    public void IncreaseSpellCritChance(float percent)
    {
        IncreaseSpellCritChanceValue += percent / 100;
        OnStatsChange?.Invoke();
    }
    public void MoreSpellCritChance(float percent)
    {
        if (percent > 0f)
            MoreSpellCritChanceValue *= 1 + percent / 100;
        else
            MoreSpellCritChanceValue /= 1 - percent / 100;

        OnStatsChange?.Invoke();
    }
    public void LessSpellCritChance(float percent)
    {
        if (percent > 0f)
            LessSpellCritChanceValue *= 1 - percent / 100;
        else
            LessSpellCritChanceValue /= 1 + percent / 100;

        OnStatsChange?.Invoke();
    }

    public int FlatSpellCritMultiplierValue { get; private set; }
    public float IncreaseSpellCritMultiplierValue { get; private set; }
    public float MoreSpellCritMultiplierValue { get; private set; } = 1f;
    public float LessSpellCritMultiplierValue { get; private set; } = 1f;
    public void AddFlatSpellCritMultiplier(int value)
    {
        FlatSpellCritMultiplierValue += value;
        OnStatsChange?.Invoke();
    }
    public void IncreaseSpellCritMultiplier(float percent)
    {
        IncreaseSpellCritMultiplierValue += percent / 100;
        OnStatsChange?.Invoke();
    }
    public void MoreSpellCritMultiplier(float percent)
    {
        if (percent > 0f)
            MoreSpellCritMultiplierValue *= 1 + percent / 100;
        else
            MoreSpellCritMultiplierValue /= 1 - percent / 100;

        OnStatsChange?.Invoke();
    }
    public void LessSpellCritMultiplier(float percent)
    {
        if (percent > 0f)
            LessSpellCritMultiplierValue *= 1 - percent / 100;
        else
            LessSpellCritMultiplierValue /= 1 + percent / 100;

        OnStatsChange?.Invoke();
    }

    public int FlatManaValue { get; private set; }
    public float IncreaseManaValue { get; private set; }
    public float MoreManaValue { get; private set; } = 1f;
    public float LessManaValue { get; private set; } = 1f;
    public void AddFlatMana(int value)
    {
        FlatManaValue += value;
        OnStatsChange?.Invoke();
    }
    public void IncreaseMana(float percent)
    {
        IncreaseManaValue += percent / 100;
        OnStatsChange?.Invoke();
    }
    public void MoreMana(float percent)
    {
        if (percent > 0f)
            MoreManaValue *= 1 + percent / 100;
        else
            MoreManaValue /= 1 - percent / 100;

        OnStatsChange?.Invoke();
    }
    public void LessMana(float percent)
    {
        if (percent > 0f)
            LessManaValue *= 1 - percent / 100;
        else
            LessManaValue /= 1 + percent / 100;

        OnStatsChange?.Invoke();
    }

    public int FlatCooldownValue { get; private set; }
    public float IncreaseCooldownReductionValue { get; private set; }
    public float MoreCooldownReductionValue { get; private set; } = 1f;
    public float LessCooldownReductionValue { get; private set; } = 1f;
    public void AddFlatCooldown(int value)
    {
        FlatCooldownValue += value;
        OnStatsChange?.Invoke();
    }
    public void IncreaseCooldownReduction(float percent)
    {
        IncreaseCooldownReductionValue += percent / 100;
        OnStatsChange?.Invoke();
    }
    public void MoreCooldownReduction(float percent)
    {
        if (percent > 0f)
            MoreCooldownReductionValue *= 1 + percent / 100;
        else
            MoreCooldownReductionValue /= 1 - percent / 100;

        OnStatsChange?.Invoke();
    }
    public void LessCooldownReduction(float percent)
    {
        if (percent > 0f)
            LessCooldownReductionValue *= 1 - percent / 100;
        else
            LessCooldownReductionValue /= 1 + percent / 100;

        OnStatsChange?.Invoke();
    }

    public int FlatManaRegenerationValue { get; private set; }
    public float IncreaseManaRegenerationValue { get; private set; }
    public float MoreManaRegenerationValue { get; private set; } = 1f;
    public float LessManaRegenerationValue { get; private set; } = 1f;
    public void AddFlatManaRegeneration(int value)
    {
        FlatManaRegenerationValue += value;
        OnStatsChange?.Invoke();
    }
    public void IncreaseManaRegeneration(float percent)
    {
        IncreaseManaRegenerationValue += percent / 100;
        OnStatsChange?.Invoke();
    }
    public void MoreManaRegeneration(float percent)
    {
        if (percent > 0f)
            MoreManaRegenerationValue *= 1 + percent / 100;
        else
            MoreManaRegenerationValue /= 1 - percent / 100;

        OnStatsChange?.Invoke();
    }
    public void LessManaRegeneration(float percent)
    {
        if (percent > 0f)
            LessManaRegenerationValue *= 1 - percent / 100;
        else
            LessManaRegenerationValue /= 1 + percent / 100;

        OnStatsChange?.Invoke();
    }

    public int FlatManacostValue { get; private set; }
    public float IncreaseManacostValue { get; private set; }
    public float MoreManacostValue { get; private set; } = 1f;
    public float LessManacostValue { get; private set; } = 1f;
    public void AddFlatManacost(int value)
    {
        FlatManacostValue += value;
        OnStatsChange?.Invoke();
    }
    public void IncreaseManacost(float percent)
    {
        IncreaseManacostValue += percent / 100;
        OnStatsChange?.Invoke();
    }
    public void MoreManacost(float percent)
    {
        if (percent > 0f)
            MoreManacostValue *= 1 + percent / 100;
        else
            MoreManacostValue /= 1 - percent / 100;

        OnStatsChange?.Invoke();
    }
    public void LessManacost(float percent)
    {
        if (percent > 0f)
            LessManacostValue *= 1 - percent / 100;
        else
            LessManacostValue /= 1 + percent / 100;

        OnStatsChange?.Invoke();
    }

    public int FlatSpellProjectileAmountValue { get; private set; }
    public void AddFlatSpellProjectileAmount(int value)
    {
        FlatSpellProjectileAmountValue += value;
        OnStatsChange?.Invoke();
    }

    public int FlatSpellDamageValue { get; private set; }
    public float IncreaseSpellDamageValue { get; private set; }
    public float MoreSpellDamageValue { get; private set; } = 1f;
    public float LessSpellDamageValue { get; private set; } = 1f;
    public void AddFlatSpellDamage(int value)
    {
        FlatSpellDamageValue += value;
        OnStatsChange?.Invoke();
    }
    public void IncreaseSpellDamage(float percent)
    {
        IncreaseSpellDamageValue += percent / 100;
        OnStatsChange?.Invoke();
    }
    public void MoreSpellDamage(float percent)
    {
        if (percent > 0f)
            MoreSpellDamageValue *= 1 + percent / 100;
        else
            MoreSpellDamageValue /= 1 - percent / 100;

        OnStatsChange?.Invoke();
    }
    public void LessSpellDamage(float percent)
    {
        if (percent > 0f)
            LessSpellDamageValue *= 1 - percent / 100;
        else
            LessSpellDamageValue /= 1 + percent / 100;

        OnStatsChange?.Invoke();
    }
}
