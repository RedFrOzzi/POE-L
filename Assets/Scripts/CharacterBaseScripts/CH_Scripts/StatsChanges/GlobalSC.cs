using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

public class GlobalSC
{
    public event Action OnStatsChange;

    private enum StatsChangesType { Flat, Increase, More, Less };
    private static readonly PropertyInfo[] props = typeof(GlobalSC).GetProperties(BindingFlags.Instance | BindingFlags.Public);
    private static readonly Dictionary<int, StatsChangesType> statsChangesType = new();
    private static bool isPropsSet = false;

    public GlobalSC()
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

    public void SwapChanges(GlobalSC changes)
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

    public void CombineChanges(GlobalSC changes)
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

    public void RemoveChanges(GlobalSC changes)
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

    public float IncreaseDamageValue { get; private set; }
    public float MoreDamageValue { get; private set; } = 1f;
    public float LessDamageValue { get; private set; } = 1f;
    public void IncreaseDamage(float percent)
    {
        IncreaseDamageValue += percent / 100;
        OnStatsChange?.Invoke();
    }
    public void MoreDamage(float percent)
    {
        if (percent > 0f)
            MoreDamageValue *= 1 + percent / 100;
        else
            MoreDamageValue /= 1 - percent / 100;

        OnStatsChange?.Invoke();
    }
    public void LessDamage(float percent)
    {
        if (percent > 0f)
            LessDamageValue *= 1 - percent / 100;
        else
            LessDamageValue /= 1 + percent / 100;

        OnStatsChange?.Invoke();
    }

    public float IncreasePhysicalDamageValue { get; private set; }
    public float MorePhysicalDamageValue { get; private set; } = 1f;
    public float LessPhysicalDamageValue { get; private set; } = 1f;
    public void IncreasePhysicalDamage(float percent)
    {
        IncreasePhysicalDamageValue += percent / 100;
        OnStatsChange?.Invoke();
    }
    public void MorePhysicalDamage(float percent)
    {
        if (percent > 0f)
            MorePhysicalDamageValue *= 1 + percent / 100;
        else
            MorePhysicalDamageValue /= 1 - percent / 100;

        OnStatsChange?.Invoke();
    }
    public void LessPhysicalDamage(float percent)
    {
        if (percent > 0f)
            LessPhysicalDamageValue *= 1 - percent / 100;
        else
            LessPhysicalDamageValue /= 1 + percent / 100;

        OnStatsChange?.Invoke();
    }

    public float IncreaseMagicDamageValue { get; private set; }
    public float MoreMagicDamageValue { get; private set; } = 1f;
    public float LessMagicDamageValue { get; private set; } = 1f;
    public void IncreaseMagicDamage(float percent)
    {
        IncreaseMagicDamageValue += percent / 100;
        OnStatsChange?.Invoke();
    }
    public void MoreMagicDamage(float percent)
    {
        if (percent > 0f)
            MoreMagicDamageValue *= 1 + percent / 100;
        else
            MoreMagicDamageValue /= 1 - percent / 100;

        OnStatsChange?.Invoke();
    }
    public void LessMagicDamage(float percent)
    {
        if (percent > 0f)
            LessMagicDamageValue *= 1 - percent / 100;
        else
            LessMagicDamageValue /= 1 + percent / 100;

        OnStatsChange?.Invoke();
    }

    public int FlatCritMultiplierValue { get; private set; }
    public float IncreaseCritMultiplierValue { get; private set; }
    public float MoreCritMultiplierValue { get; private set; } = 1f;
    public float LessCritMultiplierValue { get; private set; } = 1f;
    public void AddFlatCritMultiplier(int value)
    {
        FlatCritMultiplierValue += value;
        OnStatsChange?.Invoke();
    }
    public void IncreaseCritMultiplier(float percent)
    {
        IncreaseCritMultiplierValue += percent / 100;
        OnStatsChange?.Invoke();
    }
    public void MoreCritMultiplier(float percent)
    {
        if (percent > 0f)
            MoreCritMultiplierValue *= 1 + percent / 100;
        else
            MoreCritMultiplierValue /= 1 - percent / 100;

        OnStatsChange?.Invoke();
    }
    public void LessCritMultiplier(float percent)
    {
        if (percent > 0f)
            LessCritMultiplierValue *= 1 - percent / 100;
        else
            LessCritMultiplierValue /= 1 + percent / 100;

        OnStatsChange?.Invoke();
    }

    public float IncreaseAreaDamageValue { get; private set; }
    public float MoreAreaDamageValue { get; private set; } = 1f;
    public float LessAreaDamageValue { get; private set; } = 1f;
    public void IncreaseAreaDamage(float percent)
    {
        IncreaseAreaDamageValue += percent / 100;
        OnStatsChange?.Invoke();
    }
    public void MoreAreaDamage(float percent)
    {
        if (percent > 0f)
            MoreAreaDamageValue *= 1 + percent / 100;
        else
            MoreAreaDamageValue /= 1 - percent / 100;

        OnStatsChange?.Invoke();
    }
    public void LessAreaDamage(float percent)
    {
        if (percent > 0f)
            LessAreaDamageValue *= 1 - percent / 100;
        else
            LessAreaDamageValue /= 1 + percent / 100;

        OnStatsChange?.Invoke();
    }

    public int FlatEvasionValue { get; private set; }
    public float IncreaseEvasionValue { get; private set; }
    public float MoreEvasionValue { get; private set; } = 1f;
    public float LessEvasionValue { get; private set; } = 1f;
    public void AddFlatEvasion(int value)
    {
        FlatEvasionValue += value;
        OnStatsChange?.Invoke();
    }
    public void IncreaseEvasion(float percent)
    {
        IncreaseEvasionValue += percent / 100;
        OnStatsChange?.Invoke();
    }
    public void MoreEvasion(float percent)
    {
        if (percent > 0f)
            MoreEvasionValue *= 1 + percent / 100;
        else
            MoreEvasionValue /= 1 - percent / 100;

        OnStatsChange?.Invoke();
    }
    public void LessEvasion(float percent)
    {
        if (percent > 0f)
            LessEvasionValue *= 1 - percent / 100;
        else
            LessEvasionValue /= 1 + percent / 100;

        OnStatsChange?.Invoke();
    }

    public int FlatReflectDamageValue { get; private set; }
    public float IncreaseReflectDamageValue { get; private set; }
    public float MoreReflectDamageValue { get; private set; } = 1f;
    public float LessReflectDamageValue { get; private set; } = 1f;
    public void AddFlatReflectDamage(int value)
    {
        FlatReflectDamageValue += value;
        OnStatsChange?.Invoke();
    }
    public void IncreaseReflectDamage(float percent)
    {
        IncreaseReflectDamageValue += percent / 100;
        OnStatsChange?.Invoke();
    }
    public void MoreReflectDamage(float percent)
    {
        if (percent > 0f)
            MoreReflectDamageValue *= 1 + percent / 100;
        else
            MoreReflectDamageValue /= 1 - percent / 100;

        OnStatsChange?.Invoke();
    }
    public void LessReflectDamage(float percent)
    {
        if (percent > 0f)
            LessReflectDamageValue *= 1 - percent / 100;
        else
            LessReflectDamageValue /= 1 + percent / 100;

        OnStatsChange?.Invoke();
    }

    public int FlatTenacityValue { get; private set; }
    public float IncreaseTenacityValue { get; private set; }
    public float MoreTenacityValue { get; private set; } = 1f;
    public float LessTenacityValue { get; private set; } = 1f;
    public void AddFlatTenacity(int value)
    {
        FlatTenacityValue += value;
        OnStatsChange?.Invoke();
    }
    public void IncreaseTenacity(float percent)
    {
        IncreaseTenacityValue += percent / 100;
        OnStatsChange?.Invoke();
    }
    public void MoreTenacity(float percent)
    {
        if (percent > 0f)
            MoreTenacityValue *= 1 + percent / 100;
        else
            MoreTenacityValue /= 1 - percent / 100;

        OnStatsChange?.Invoke();
    }
    public void LessTenacity(float percent)
    {
        if (percent > 0f)
            LessTenacityValue *= 1 - percent / 100;
        else
            LessTenacityValue /= 1 + percent / 100;

        OnStatsChange?.Invoke();
    }

    public int FlatPhysDOTMultiValue { get; private set; }
    public void AddFlatPhysDOTMulti(int percent)
    {
        FlatPhysDOTMultiValue += percent / 100;
        OnStatsChange?.Invoke();
    }

    public int FlatMagicDOTMultiValue { get; private set; }
    public void AddFlatMagicDOTMulti(int percent)
    {
        FlatMagicDOTMultiValue += percent / 100;
        OnStatsChange?.Invoke();
    }

}
