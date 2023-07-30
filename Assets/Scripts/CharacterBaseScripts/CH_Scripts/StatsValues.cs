using UnityEngine;
using System;

[Serializable]
public struct StatsValues
{
    [Header("Type")]
    public CharacterType CharacterType;

    [Space(20)]
    [Header("Offensive")]
    public float BaseMaxDamage;
    public float BaseMinDamage;
    [Space(5)]
    public int BaseAmmoCapacity;
    public float BaseAttackSpeed;
    public float BaseCritChance;
    public float BaseCritMultiplier;
    public float BaseSpellCritMultiplier;
    [Space(5)]
    public float BaseBuffPower;
    public float BaseBuffDurationAmplifier;
    [Range(0, 1000)]
    public float BaseAccuracy;
    public AnimationCurve FlatAccuracyToPercent;
    public float BaseSpreadAngle;
    [Space(5)]
    public int ProjectileAmount;
    public int PierceAmount;
    public int ChainsAmount;
    [Space(5)]
    public int AddedSpellProjectileAmount;

    [Space(20)]
    [Header("Defansive")]
    public float BaseHP;
    public float BaseHPRegeneration;
    [Space(3)]
    public float BaseMana;
    public float BaseManaRegeneration;
    [Space(3)]
    public float BaseArmor;
    public float BaseMagicResist;
    public float BaseHealingAmplifier;
    public AnimationCurve FlatArmorToPercent;
    public AnimationCurve FlatMResistToPercent;

    [Space(20)]
    [Header("Utility")]
    public float BaseCollectorRadius;
    public float BaseMovementSpeed;
    public float BaseReloadSpeed;
    public float BaseAttackRange;
    public float BaseProjectileSpeed;
    public float BaseGlobalAOEMultiplier;
    [Space(3)]
    public float BaseExperienceMultiplier;
    [Space(3)]
    public float BaseGoldGainMultipler;
}
