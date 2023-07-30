using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

public class AttackSC
{
    public event Action OnStatsChange;

    private enum StatsChangesType { Flat, Increase, More, Less };
    private static readonly PropertyInfo[] props = typeof(AttackSC).GetProperties(BindingFlags.Instance | BindingFlags.Public);
    private static readonly Dictionary<int, StatsChangesType> statsChangesType = new();
    private static bool isPropsSet = false;

    public AttackSC()
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

    public void SwapChanges(AttackSC changes)
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

    public void CombineChanges(AttackSC changes)
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

    public void RemoveChanges(AttackSC changes)
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

    public float FlatAttackCritChanceValue { get; private set; }
    public float IncreaseAttackCritChanceValue { get; private set; }
    public float MoreAttackCritChanceValue { get; private set; } = 1f;
    public float LessAttackCritChanceValue { get; private set; } = 1f;
    public void AddFlatAttackCritChance(float value)
    {
        FlatAttackCritChanceValue += value;
        OnStatsChange?.Invoke();
    }
    public void IncreaseAttackCritChance(float percent)
    {
        IncreaseAttackCritChanceValue += percent / 100;
        OnStatsChange?.Invoke();
    }
    public void MoreAttackCritChance(float percent)
    {
        if (percent > 0f)
            MoreAttackCritChanceValue *= 1 + percent / 100;
        else
            MoreAttackCritChanceValue /= 1 - percent / 100;

        OnStatsChange?.Invoke();
    }
    public void LessAttackCritChance(float percent)
    {
        if (percent > 0f)
            LessAttackCritChanceValue *= 1 - percent / 100;
        else
            LessAttackCritChanceValue /= 1 + percent / 100;

        OnStatsChange?.Invoke();
    }

    public int FlatAttackCritMultiplierValue { get; private set; }
    public float IncreaseAttackCritMultiplierValue { get; private set; }
    public float MoreAttackCritMultiplierValue { get; private set; } = 1f;
    public float LessAttackCritMultiplierValue { get; private set; } = 1f;
    public void AddFlatAttackCritMultiplier(int value)
    {
        FlatAttackCritMultiplierValue += value;
        OnStatsChange?.Invoke();
    }
    public void IncreaseAttackCritMultiplier(float percent)
    {
        IncreaseAttackCritMultiplierValue += percent / 100;
        OnStatsChange?.Invoke();
    }
    public void MoreAttackCritMultiplier(float percent)
    {
        if (percent > 0f)
            MoreAttackCritMultiplierValue *= 1 + percent / 100;
        else
            MoreAttackCritMultiplierValue /= 1 - percent / 100;

        OnStatsChange?.Invoke();
    }
    public void LessAttackCritMultiplier(float percent)
    {
        if (percent > 0f)
            LessAttackCritMultiplierValue *= 1 - percent / 100;
        else
            LessAttackCritMultiplierValue /= 1 + percent / 100;

        OnStatsChange?.Invoke();
    }

    public int FlatAccuracyValue { get; private set; }
    public float IncreaseAccuracyValue { get; private set; }
    public float MoreAccuracyValue { get; private set; } = 1f;
    public float LessAccuracyValue { get; private set; } = 1f;
    public void AddFlatAccuracy(int value)
    {
        FlatAccuracyValue += value;
        OnStatsChange?.Invoke();
    }
    public void IncreaseAccuracy(float percent)
    {
        IncreaseAccuracyValue += percent / 100;
        OnStatsChange?.Invoke();
    }
    public void MoreAccuracy(float percent)
    {
        if (percent > 0f)
            MoreAccuracyValue *= 1 + percent / 100;
        else
            MoreAccuracyValue /= 1 - percent / 100;

        OnStatsChange?.Invoke();
    }
    public void LessAccuracy(float percent)
    {
        if (percent > 0f)
            LessAccuracyValue *= 1 - percent / 100;
        else
            LessAccuracyValue /= 1 + percent / 100;

        OnStatsChange?.Invoke();
    }

    public float IncreaseAttackSpeedValue { get; private set; }
    public float MoreAttackSpeedValue { get; private set; } = 1f;
    public float LessAttackSpeedValue { get; private set; } = 1f;
    public void IncreaseAttackSpeed(float percent)
    {
        IncreaseAttackSpeedValue += percent / 100;
        OnStatsChange?.Invoke();
    }
    public void MoreAttackSpeed(float percent)
    {
        if (percent > 0f)
            MoreAttackSpeedValue *= 1 + percent / 100;
        else
            MoreAttackSpeedValue /= 1 - percent / 100;

        OnStatsChange?.Invoke();
    }
    public void LessAttackSpeed(float percent)
    {
        if (percent > 0f)
            LessAttackSpeedValue *= 1 - percent / 100;
        else
            LessAttackSpeedValue /= 1 + percent / 100;

        OnStatsChange?.Invoke();
    }

    public float IncreaseReloadSpeedValue { get; private set; }
    public float MoreReloadSpeedValue { get; private set; } = 1f;
    public float LessReloadSpeedValue { get; private set; } = 1f;
    public void IncreaseReloadSpeed(float percent)
    {
        IncreaseReloadSpeedValue += percent / 100;
        OnStatsChange?.Invoke();
    }
    public void MoreReloadSpeed(float percent)
    {
        if (percent > 0f)
            MoreReloadSpeedValue *= 1 + percent / 100;
        else
            MoreReloadSpeedValue /= 1 - percent / 100;

        OnStatsChange?.Invoke();
    }
    public void LessReloadSpeed(float percent)
    {
        if (percent > 0f)
            LessReloadSpeedValue *= 1 - percent / 100;
        else
            LessReloadSpeedValue /= 1 + percent / 100;

        OnStatsChange?.Invoke();
    }

    public int FlatAmmoCapacityValue { get; private set; }
    public float IncreaseAmmoCapacityValue { get; private set; }
    public float MoreAmmoCapacityValue { get; private set; } = 1f;
    public float LessAmmoCapacityValue { get; private set; } = 1f;
    public void AddFlatAmmoCapacity(int value)
    {
        FlatAmmoCapacityValue += value;
        OnStatsChange?.Invoke();
    }
    public void IncreaseAmmoCapacity(float percent)
    {
        IncreaseAmmoCapacityValue += percent / 100;
        OnStatsChange?.Invoke();
    }
    public void MoreAmmoCapacity(float percent)
    {
        if (percent > 0f)
            MoreAmmoCapacityValue *= 1 + percent / 100;
        else
            MoreAmmoCapacityValue /= 1 - percent / 100;

        OnStatsChange?.Invoke();
    }
    public void LessAmmoCapacity(float percent)
    {
        if (percent > 0f)
            LessAmmoCapacityValue *= 1 - percent / 100;
        else
            LessAmmoCapacityValue /= 1 + percent / 100;

        OnStatsChange?.Invoke();
    }

    public float IncreaseAttackRangeValue { get; private set; }
    public float MoreAttackRangeValue { get; private set; } = 1f;
    public float LessAttackRangeValue { get; private set; } = 1f;
    public void IncreaseAttackRange(float percent)
    {
        IncreaseAttackRangeValue += percent / 100;
        OnStatsChange?.Invoke();
    }
    public void MoreAttackRange(float percent)
    {
        if (percent > 0f)
            MoreAttackRangeValue *= 1 + percent / 100;
        else
            MoreAttackRangeValue /= 1 - percent / 100;

        OnStatsChange?.Invoke();
    }
    public void LessAttackRange(float percent)
    {
        if (percent > 0f)
            LessAttackRangeValue *= 1 - percent / 100;
        else
            LessAttackRangeValue /= 1 + percent / 100;

        OnStatsChange?.Invoke();
    }

    public float IncreaseSpreadAngleValue { get; private set; }
    public float MoreSpreadAngleValue { get; private set; } = 1f;
    public float LessSpreadAngleValue { get; private set; } = 1f;
    public void IncreaseSpreadAngle(float percent)
    {
        IncreaseSpreadAngleValue += percent / 100;
        OnStatsChange?.Invoke();
    }
    public void MoreSpreadAngle(float percent)
    {
        if (percent > 0f)
            MoreSpreadAngleValue *= 1 + percent / 100;
        else
            MoreSpreadAngleValue /= 1 - percent / 100;

        OnStatsChange?.Invoke();
    }
    public void LessSpreadAngle(float percent)
    {
        if (percent > 0f)
            LessSpreadAngleValue *= 1 - percent / 100;
        else
            LessSpreadAngleValue /= 1 + percent / 100;

        OnStatsChange?.Invoke();
    }

    public int FlatAttackDamageValue { get; private set; }
    public float IncreaseAttackDamageValue { get; private set; }
    public float MoreAttackDamageValue { get; private set; } = 1f;
    public float LessAttackDamageValue { get; private set; } = 1f;
    public void AddFlatAttackDamage(int value)
    {
        FlatAttackDamageValue += value;
        OnStatsChange?.Invoke();
    }
    public void IncreaseAttackDamage(float percent)
    {
        IncreaseAttackDamageValue += percent / 100;
        OnStatsChange?.Invoke();
    }
    public void MoreAttackDamage(float percent)
    {
        if (percent > 0f)
            MoreAttackDamageValue *= 1 + percent / 100;
        else
            MoreAttackDamageValue /= 1 - percent / 100;

        OnStatsChange?.Invoke();
    }
    public void LessAttackDamage(float percent)
    {
        if (percent > 0f)
            LessAttackDamageValue *= 1 - percent / 100;
        else
            LessAttackDamageValue /= 1 + percent / 100;

        OnStatsChange?.Invoke();
    }

    public int FlatWeaponProjectileAmountValue { get; private set; }
    public void AddFlatWeaponProjectileAmount(int value)
    {
        FlatWeaponProjectileAmountValue += value;
        OnStatsChange?.Invoke();
    }

    public int FlatWeaponPierceAmountValue { get; private set; }
    public void AddFlatWeaponPierceAmount(int value)
    {
        FlatWeaponPierceAmountValue += value;
        OnStatsChange?.Invoke();
    }

    public int FlatWeaponChainsAmountValue { get; private set; }
    public void AddFlatWeaponChainAmount(int value)
    {
        FlatWeaponChainsAmountValue += value;
        OnStatsChange?.Invoke();
    }

    public int FlatChargeRateValue { get; private set; }
    public float IncreaseChargeRateValue { get; private set; }
    public float MoreChargeRateValue { get; private set; } = 1f;
    public float LessChargeRateValue { get; private set; } = 1f;
    public void AddFlatChargeRate(int value)
    {
        FlatChargeRateValue += value;
        OnStatsChange?.Invoke();
    }
    public void IncreaseChargeRate(float percent)
    {
        IncreaseChargeRateValue += percent / 100;
        OnStatsChange?.Invoke();
    }
    public void MoreChargeRate(float percent)
    {
        if (percent > 0f)
            MoreChargeRateValue *= 1 + percent / 100;
        else
            MoreChargeRateValue /= 1 - percent / 100;

        OnStatsChange?.Invoke();
    }
    public void LessChargeRate(float percent)
    {
        if (percent > 0f)
            LessChargeRateValue *= 1 - percent / 100;
        else
            LessChargeRateValue /= 1 + percent / 100;

        OnStatsChange?.Invoke();
    }

    public int FlatMaxChargeValue { get; private set; }
    public float IncreaseMaxChargeValue { get; private set; }
    public float MoreMaxChargeValue { get; private set; } = 1f;
    public float LessMaxChargeValue { get; private set; } = 1f;
    public void AddFlatMaxCharge(int value)
    {
        FlatMaxChargeValue += value;
        OnStatsChange?.Invoke();
    }
    public void IncreaseMaxCharge(float percent)
    {
        IncreaseMaxChargeValue += percent / 100;
        OnStatsChange?.Invoke();
    }
    public void MoreMaxCharge(float percent)
    {
        if (percent > 0f)
            MoreMaxChargeValue *= 1 + percent / 100;
        else
            MoreMaxChargeValue /= 1 - percent / 100;

        OnStatsChange?.Invoke();
    }
    public void LessMaxCharge(float percent)
    {
        if (percent > 0f)
            LessMaxChargeValue *= 1 - percent / 100;
        else
            LessMaxChargeValue /= 1 + percent / 100;

        OnStatsChange?.Invoke();
    }

    public int FlatChargeDamageModifierValue { get; private set; }
    public float IncreaseChargeDamageModifierValue { get; private set; }
    public float MoreChargeDamageModifierValue { get; private set; } = 1f;
    public float LessChargeDamageModifierValue { get; private set; } = 1f;
    public void AddFlatChargeDamageModifier(int value)
    {
        FlatChargeDamageModifierValue += value;
        OnStatsChange?.Invoke();
    }
    public void IncreaseChargeDamageModifier(float percent)
    {
        IncreaseChargeDamageModifierValue += percent / 100;
        OnStatsChange?.Invoke();
    }
    public void MoreChargeDamageModifier(float percent)
    {
        if (percent > 0f)
            MoreChargeDamageModifierValue *= 1 + percent / 100;
        else
            MoreChargeDamageModifierValue /= 1 - percent / 100;

        OnStatsChange?.Invoke();
    }
    public void LessChargeDamageModifier(float percent)
    {
        if (percent > 0f)
            LessChargeDamageModifierValue *= 1 - percent / 100;
        else
            LessChargeDamageModifierValue /= 1 + percent / 100;

        OnStatsChange?.Invoke();
    }

    public int FlatExplosionRadiusValue { get; private set; }
    public float IncreaseExplosionRadiusValue { get; private set; }
    public float MoreExplosionRadiusValue { get; private set; } = 1f;
    public float LessExplosionRadiusValue { get; private set; } = 1f;
    public void AddFlatExplosionRadius(int value)
    {
        FlatExplosionRadiusValue += value;
        OnStatsChange?.Invoke();
    }
    public void IncreaseExplosionRadius(float percent)
    {
        IncreaseExplosionRadiusValue += percent / 100;
        OnStatsChange?.Invoke();
    }
    public void MoreExplosionRadius(float percent)
    {
        if (percent > 0f)
            MoreExplosionRadiusValue *= 1 + percent / 100;
        else
            MoreExplosionRadiusValue /= 1 - percent / 100;

        OnStatsChange?.Invoke();
    }
    public void LessExplosionRadius(float percent)
    {
        if (percent > 0f)
            LessExplosionRadiusValue *= 1 - percent / 100;
        else
            LessExplosionRadiusValue /= 1 + percent / 100;

        OnStatsChange?.Invoke();
    }

    public int FlatTimeToReachValue { get; private set; }
    public float IncreaseTimeToReachValue { get; private set; }
    public float MoreTimeToReachValue { get; private set; } = 1f;
    public float LessTimeToReachValue { get; private set; } = 1f;
    public void AddFlatTimeToReach(int value)
    {
        FlatTimeToReachValue += value;
        OnStatsChange?.Invoke();
    }
    public void IncreaseTimeToReach(float percent)
    {
        IncreaseTimeToReachValue += percent / 100;
        OnStatsChange?.Invoke();
    }
    public void MoreTimeToReach(float percent)
    {
        if (percent > 0f)
            MoreTimeToReachValue *= 1 + percent / 100;
        else
            MoreTimeToReachValue /= 1 - percent / 100;

        OnStatsChange?.Invoke();
    }
    public void LessTimeToReach(float percent)
    {
        if (percent > 0f)
            LessTimeToReachValue *= 1 - percent / 100;
        else
            LessTimeToReachValue /= 1 + percent / 100;

        OnStatsChange?.Invoke();
    }

    public float IncreasePercentOfDamageToExplosionValue { get; private set; }
    public float MorePercentOfDamageToExplosionValue { get; private set; } = 1f;
    public float LessPercentOfDamageToExplosionValue { get; private set; } = 1f;
    public void IncreasePercentOfDamageToExplosion(float percent)
    {
        IncreasePercentOfDamageToExplosionValue += percent / 100;
        OnStatsChange?.Invoke();
    }
    public void MorePercentOfDamageToExplosion(float percent)
    {
        if (percent > 0f)
            MorePercentOfDamageToExplosionValue *= 1 + percent / 100;
        else
            MorePercentOfDamageToExplosionValue /= 1 - percent / 100;

        OnStatsChange?.Invoke();
    }
    public void LessPercentOfDamageToExplosion(float percent)
    {
        if (percent > 0f)
            LessPercentOfDamageToExplosionValue *= 1 - percent / 100;
        else
            LessPercentOfDamageToExplosionValue /= 1 + percent / 100;

        OnStatsChange?.Invoke();
    }
}
