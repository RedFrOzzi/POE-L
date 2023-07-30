using System;

[Serializable]
public class AbilityModifierStats
{
    public const float BaseCritMultiplier = 1.5f;

    public int AddedLevel;
    public float AddedFlatMagicDamage;
    public float MagicDamageMultiplier;
    public int AddedCooldownReduction; //used only in calculations. Use CooldownMultiplier as final CDR
    public float CooldownMultipler;
    public float AddedFlatCritChance;
    public float CritChanceMultiplier;
    public float CritMultipler;
    public float AreaMultipler;
    public int FlatManacostReduction;
    public float ManacostMultipler;
    public int AddedProjectiles;

    public AbilityModifierStats()
    {
        MagicDamageMultiplier = 1;
        CooldownMultipler = 1;
        CritChanceMultiplier = 1;
        CritMultipler = 1;
        AreaMultipler = 1;
        ManacostMultipler = 1;
    }
}