using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

public class UtilitySC
{
    public event Action OnStatsChange;

    private enum StatsChangesType { Flat, Increase, More, Less };
    private static readonly PropertyInfo[] props = typeof(UtilitySC).GetProperties(BindingFlags.Instance | BindingFlags.Public);
    private static readonly Dictionary<int, StatsChangesType> statsChangesType = new();
    private static bool isPropsSet = false;

    public UtilitySC()
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

    public void SwapChanges(UtilitySC changes)
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

    public void CombineChanges(UtilitySC changes)
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

    public void RemoveChanges(UtilitySC changes)
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


    public int FlatMovementSpeedValue { get; private set; }
    public float IncreaseMovementSpeedValue { get; private set; }
    public float MoreMovementSpeedValue { get; private set; } = 1f;
    public float LessMovementSpeedValue { get; private set; } = 1f;
    public void AddFlatMovementSpeed(int value)
    {
        FlatMovementSpeedValue += value;
        OnStatsChange?.Invoke();
    }
    public void IncreaseMovementSpeed(float percent)
    {
        IncreaseMovementSpeedValue += percent / 100;
        OnStatsChange?.Invoke();
    }
    public void MoreMovementSpeed(float percent)
    {
        if (percent > 0f)
            MoreMovementSpeedValue *= 1 + percent / 100;
        else
            MoreMovementSpeedValue /= 1 - percent / 100;

        OnStatsChange?.Invoke();
    }
    public void LessMovementSpeed(float percent)
    {
        if (percent > 0f)
            LessMovementSpeedValue *= 1 - percent / 100;
        else
            LessMovementSpeedValue /= 1 + percent / 100;

        OnStatsChange?.Invoke();
    }

    public float IncreaseCollectorRadiusValue { get; private set; }
    public float MoreCollectorRadiusValue { get; private set; } = 1f;
    public float LessCollectorRadiusValue { get; private set; } = 1f;
    public void IncreaseCollectorRadius(float percent)
    {
        IncreaseCollectorRadiusValue += percent / 100;
        OnStatsChange?.Invoke();
    }
    public void MoreCollectorRadius(float percent)
    {
        if (percent > 0f)
            MoreCollectorRadiusValue *= 1 + percent / 100;
        else
            MoreCollectorRadiusValue /= 1 - percent / 100;

        OnStatsChange?.Invoke();
    }
    public void LessCollectorRadius(float percent)
    {
        if (percent > 0f)
            LessCollectorRadiusValue *= 1 - percent / 100;
        else
            LessCollectorRadiusValue /= 1 + percent / 100;

        OnStatsChange?.Invoke();
    }

    public float IncreaseBuffPowerValue { get; private set; }
    public float MoreBuffPowerValue { get; private set; } = 1f;
    public float LessBuffPowerValue { get; private set; } = 1f;
    public void IncreaseBuffPower(float percent)
    {
        IncreaseBuffPowerValue += percent / 100;
        OnStatsChange?.Invoke();
    }
    public void MoreBuffPower(float percent)
    {
        if (percent > 0f)
            MoreBuffPowerValue *= 1 + percent / 100;
        else
            MoreBuffPowerValue /= 1 - percent / 100;

        OnStatsChange?.Invoke();
    }
    public void LessBuffPower(float percent)
    {
        if (percent > 0f)
            LessBuffPowerValue *= 1 - percent / 100;
        else
            LessBuffPowerValue /= 1 + percent / 100;

        OnStatsChange?.Invoke();
    }

    public float IncreaseEffectDurationValue { get; private set; }
    public float MoreEffectDurationValue { get; private set; } = 1f;
    public float LessEffectDurationValue { get; private set; } = 1f;
    public void IncreaseEffectDuration(float percent)
    {
        IncreaseEffectDurationValue += percent / 100;
        OnStatsChange?.Invoke();
    }
    public void MoreEffectDuration(float percent)
    {
        if (percent > 0f)
            MoreEffectDurationValue *= 1 + percent / 100;
        else
            MoreEffectDurationValue /= 1 - percent / 100;

        OnStatsChange?.Invoke();
    }
    public void LessEffectDuration(float percent)
    {
        if (percent > 0f)
            LessEffectDurationValue *= 1 - percent / 100;
        else
            LessEffectDurationValue /= 1 + percent / 100;

        OnStatsChange?.Invoke();
    }

    public float IncreaseAreaValue { get; private set; }
    public float MoreAreaValue { get; private set; } = 1f;
    public float LessAreaValue { get; private set; } = 1f;
    public void IncreaseArea(float percent)
    {
        IncreaseAreaValue += percent / 100;
        OnStatsChange?.Invoke();
    }
    public void MoreArea(float percent)
    {
        if (percent > 0f)
            MoreAreaValue *= 1 + percent / 100;
        else
            MoreAreaValue /= 1 - percent / 100;

        OnStatsChange?.Invoke();
    }
    public void LessArea(float percent)
    {
        if (percent > 0f)
            LessAreaValue *= 1 - percent / 100;
        else
            LessAreaValue /= 1 + percent / 100;

        OnStatsChange?.Invoke();
    }

    public float IncreaseExperienceValue { get; private set; }
    public float MoreExperienceValue { get; private set; } = 1f;
    public float LessExperienceValue { get; private set; } = 1f;
    public void IncreaseExperience(float percent)
    {
        IncreaseExperienceValue += percent / 100;
        OnStatsChange?.Invoke();
    }
    public void MoreExperience(float percent)
    {
        if (percent > 0f)
            MoreExperienceValue *= 1 + percent / 100;
        else
            MoreExperienceValue /= 1 - percent / 100;

        OnStatsChange?.Invoke();
    }
    public void LessExperience(float percent)
    {
        if (percent > 0f)
            LessExperienceValue *= 1 - percent / 100;
        else
            LessExperienceValue /= 1 + percent / 100;

        OnStatsChange?.Invoke();
    }

    public float IncreaseGoldGainValue { get; private set; }
    public float MoreGoldGainValue { get; private set; } = 1f;
    public float LessGoldGainValue { get; private set; } = 1f;
    public void IncreaseGoldGain(float percent)
    {
        IncreaseGoldGainValue += percent / 100;
        OnStatsChange?.Invoke();
    }
    public void MoreGoldGain(float percent)
    {
        if (percent > 0f)
            MoreGoldGainValue *= 1 + percent / 100;
        else
            MoreGoldGainValue /= 1 - percent / 100;

        OnStatsChange?.Invoke();
    }
    public void LessGoldGain(float percent)
    {
        if (percent > 0f)
            LessGoldGainValue *= 1 - percent / 100;
        else
            LessGoldGainValue /= 1 + percent / 100;

        OnStatsChange?.Invoke();
    }

    public int FlatProjectileSpeedValue { get; private set; }
    public float IncreaseProjectileSpeedValue { get; private set; }
    public float MoreProjectileSpeedValue { get; private set; } = 1f;
    public float LessProjectileSpeedValue { get; private set; } = 1f;
    public void AddFlatProjectileSpeed(int value)
    {
        FlatProjectileSpeedValue += value;
        OnStatsChange?.Invoke();
    }
    public void IncreaseProjectileSpeed(float percent)
    {
        IncreaseProjectileSpeedValue += percent / 100;
        OnStatsChange?.Invoke();
    }
    public void MoreProjectileSpeed(float percent)
    {
        if (percent > 0f)
            MoreProjectileSpeedValue *= 1 + percent / 100;
        else
            MoreProjectileSpeedValue /= 1 - percent / 100;

        OnStatsChange?.Invoke();
    }
    public void LessProjectileSpeed(float percent)
    {
        if (percent > 0f)
            LessProjectileSpeedValue *= 1 - percent / 100;
        else
            LessProjectileSpeedValue /= 1 + percent / 100;

        OnStatsChange?.Invoke();
    }
}
