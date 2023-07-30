using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CH_InitialStats", menuName = "ScriptableObjects/Stats")]
public class CH_InitialStats : ScriptableObject
{
    [field: SerializeField] public StatsValues InitialStats { get; set; }
    

    public StatsValues GetInitialStats()
    {
        StatsValues statsValues = new();

        statsValues.CharacterType = InitialStats.CharacterType;

        statsValues.BaseMaxDamage = InitialStats.BaseMaxDamage;
        statsValues.BaseMinDamage = InitialStats.BaseMinDamage;
        statsValues.BaseAccuracy = InitialStats.BaseAccuracy;
        statsValues.FlatAccuracyToPercent = InitialStats.FlatAccuracyToPercent;
        statsValues.BaseSpreadAngle = Mathf.Clamp(InitialStats.BaseSpreadAngle, 1, 10000);
        statsValues.BaseArmor = InitialStats.BaseArmor;
        statsValues.FlatArmorToPercent = InitialStats.FlatArmorToPercent;
        statsValues.BaseAttackSpeed = InitialStats.BaseAttackSpeed;
        statsValues.BaseCollectorRadius = InitialStats.BaseCollectorRadius;
        statsValues.BaseCritChance = InitialStats.BaseCritChance;
        statsValues.BaseCritMultiplier = Mathf.Clamp(InitialStats.BaseCritMultiplier, 1, 10000);
        statsValues.BaseSpellCritMultiplier = Mathf.Clamp(InitialStats.BaseSpellCritMultiplier, 1, 10000);
        statsValues.BaseHP = Mathf.Clamp(InitialStats.BaseHP, 1, 1000000);
        statsValues.BaseHPRegeneration = InitialStats.BaseHPRegeneration;
        statsValues.BaseMana = InitialStats.BaseMana;
        statsValues.BaseManaRegeneration = InitialStats.BaseManaRegeneration;
        statsValues.BaseMagicResist = InitialStats.BaseMagicResist;
        statsValues.FlatMResistToPercent = InitialStats.FlatMResistToPercent;
        statsValues.BaseMovementSpeed = InitialStats.BaseMovementSpeed;
        statsValues.BaseReloadSpeed = InitialStats.BaseReloadSpeed;
        statsValues.BaseAttackRange = InitialStats.BaseAttackRange;
        statsValues.BaseProjectileSpeed = InitialStats.BaseProjectileSpeed;
        statsValues.ChainsAmount = InitialStats.ChainsAmount;
        statsValues.PierceAmount = InitialStats.PierceAmount;
        statsValues.ProjectileAmount = Mathf.Clamp(InitialStats.ProjectileAmount, 1, 10000);
        statsValues.AddedSpellProjectileAmount = InitialStats.AddedSpellProjectileAmount;
        statsValues.BaseHealingAmplifier = Mathf.Clamp(InitialStats.BaseHealingAmplifier, 0.001f, 10000);
        statsValues.BaseBuffPower = Mathf.Clamp(InitialStats.BaseBuffPower, 0.001f, 10000);
        statsValues.BaseBuffDurationAmplifier = Mathf.Clamp(InitialStats.BaseBuffDurationAmplifier, 0.001f, 10000);
        statsValues.BaseAmmoCapacity = InitialStats.BaseAmmoCapacity;
        statsValues.BaseGlobalAOEMultiplier = InitialStats.BaseGlobalAOEMultiplier;
        statsValues.FlatArmorToPercent = InitialStats.FlatArmorToPercent;
        statsValues.FlatMResistToPercent = InitialStats.FlatMResistToPercent;
        statsValues.FlatAccuracyToPercent = InitialStats.FlatAccuracyToPercent;
        statsValues.BaseExperienceMultiplier = Mathf.Clamp(InitialStats.BaseExperienceMultiplier, 0.001f, 10000);
        statsValues.BaseGoldGainMultipler = Mathf.Clamp(InitialStats.BaseGoldGainMultipler, 0.001f, 10000);


        return statsValues;
    }
}