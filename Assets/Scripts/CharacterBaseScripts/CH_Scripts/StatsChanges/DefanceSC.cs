using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

public class DefanceSC
{
    public event Action OnStatsChange;

    private enum StatsChangesType { Flat, Increase, More, Less };
    private static readonly PropertyInfo[] props = typeof(DefanceSC).GetProperties(BindingFlags.Instance | BindingFlags.Public);
    private static readonly Dictionary<int, StatsChangesType> statsChangesType = new();
    private static bool isPropsSet = false;

    public DefanceSC()
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

    public void SwapChanges(DefanceSC changes)
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

    public void CombineChanges(DefanceSC changes)
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

    public void RemoveChanges(DefanceSC changes)
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

    public int FlatArmorValue { get; private set; }
    public float IncreaseArmorValue { get; private set; }
    public float MoreArmorValue { get; private set; } = 1f;
    public float LessArmorValue { get; private set; } = 1f;
    public void AddFlatArmor(int value)
    {
        FlatArmorValue += value;
        OnStatsChange?.Invoke();
    }
    public void IncreaseArmor(float percent)
    {
        IncreaseArmorValue += percent / 100;
        OnStatsChange?.Invoke();
    }
    public void MoreArmor(float percent)
    {
        if (percent > 0f)
            MoreArmorValue *= 1 + percent / 100;
        else
            MoreArmorValue /= 1 - percent / 100;

        OnStatsChange?.Invoke();
    }
    public void LessArmor(float percent)
    {
        if (percent > 0f)
            LessArmorValue *= 1 - percent / 100;
        else
            LessArmorValue /= 1 + percent / 100;

        OnStatsChange?.Invoke();
    }

    public int FlatHPValue { get; private set; }
    public float IncreaseHPValue { get; private set; }
    public float MoreHPValue { get; private set; } = 1f;
    public float LessHPValue { get; private set; } = 1f;
    public void AddFlatHP(int value)
    {
        FlatHPValue += value;
        OnStatsChange?.Invoke();
    }
    public void IncreaseHP(float percent)
    {
        IncreaseHPValue += percent / 100;
        OnStatsChange?.Invoke();
    }
    public void MoreHP(float percent)
    {
        if (percent > 0f)
            MoreHPValue *= 1 + percent / 100;
        else
            MoreHPValue /= 1 - percent / 100;

        OnStatsChange?.Invoke();
    }
    public void LessHP(float percent)
    {
        if (percent > 0f)
            LessHPValue *= 1 - percent / 100;
        else
            LessHPValue /= 1 + percent / 100;

        OnStatsChange?.Invoke();
    }

    public int FlatMagicResistValue { get; private set; }
    public float IncreaseMagicResistValue { get; private set; }
    public float MoreMagicResistValue { get; private set; } = 1f;
    public float LessMagicResistValue { get; private set; } = 1f;
    public void AddFlatMagicResist(int value)
    {
        FlatMagicResistValue += value;
        OnStatsChange?.Invoke();
    }
    public void IncreaseMagicResist(float percent)
    {
        IncreaseMagicResistValue += percent / 100;
        OnStatsChange?.Invoke();
    }
    public void MoreMagicResist(float percent)
    {
        if (percent > 0f)
            MoreMagicResistValue *= 1 + percent / 100;
        else
            MoreMagicResistValue /= 1 - percent / 100;

        OnStatsChange?.Invoke();
    }
    public void LessMagicResist(float percent)
    {
        if (percent > 0f)
            LessMagicResistValue *= 1 - percent / 100;
        else
            LessMagicResistValue /= 1 + percent / 100;

        OnStatsChange?.Invoke();
    }

    public float IncreaseHealingAmplifierValue { get; private set; }
    public float MoreHealingAmplifierValue { get; private set; } = 1f;
    public float LessHealingAmplifierValue { get; private set; } = 1f;
    public void IncreaseHealingAmplifier(float percent)
    {
        IncreaseHealingAmplifierValue += percent / 100;
        OnStatsChange?.Invoke();
    }
    public void MoreHealingAmplifier(float percent)
    {
        if (percent > 0f)
            MoreHealingAmplifierValue *= 1 + percent / 100;
        else
            MoreHealingAmplifierValue /= 1 - percent / 100;

        OnStatsChange?.Invoke();
    }
    public void LessHealingAmplifier(float percent)
    {
        if (percent > 0f)
            LessHealingAmplifierValue *= 1 - percent / 100;
        else
            LessHealingAmplifierValue /= 1 + percent / 100;

        OnStatsChange?.Invoke();
    }

    public float FlatHPRegenerationValue { get; private set; }
    public float IncreaseHPRegenerationValue { get; private set; }
    public float MoreHPRegenerationValue { get; private set; } = 1f;
    public float LessHPRegenerationValue { get; private set; } = 1f;
    public void AddFlatHPRegeneration(float value)
    {
        FlatHPRegenerationValue += value;
        OnStatsChange?.Invoke();
    }
    public void IncreaseHPRegeneration(float percent)
    {
        IncreaseHPRegenerationValue += percent / 100;
        OnStatsChange?.Invoke();
    }
    public void MoreHPRegeneration(float percent)
    {
        if (percent > 0f)
            MoreHPRegenerationValue *= 1 + percent / 100;
        else
            MoreHPRegenerationValue /= 1 - percent / 100;

        OnStatsChange?.Invoke();
    }
    public void LessHPRegeneration(float percent)
    {
        if (percent > 0f)
            LessHPRegenerationValue *= 1 - percent / 100;
        else
            LessHPRegenerationValue /= 1 + percent / 100;

        OnStatsChange?.Invoke();
    }

}
