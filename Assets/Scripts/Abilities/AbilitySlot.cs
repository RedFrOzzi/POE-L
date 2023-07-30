using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Database;
using System;
using System.Linq;

public class AbilitySlot
{
    public byte SlotIndex { get; private set; }

    public readonly CH_Stats Stats;

    public readonly CH_AbilitiesManager AbilitiesManager;

    public readonly StatsChanges LSC;

    public Ability Ability { get; private set; } = new();
    public AbilityType AbilityType { get; private set; }

    private bool isActivationTypeChanged;

    public float NextActivationTime { get; private set; }

    private const int minSeekRadius = 4;
    private const int maxSeekRadius = 15;
    private const int seekStep = 2;

    public AbilitySlot(CH_Stats stats, byte slotIndex, StatsChanges statsChanges, CH_AbilitiesManager abilitiesManager)
    {
        AbilitiesManager = abilitiesManager;
        SlotIndex = slotIndex;
        Stats = stats;
        LSC = statsChanges;
        AbilityType = AbilityType.Automatic;
        Ability = CH_AbilitiesManager.VoidAbility;
        NextActivationTime = 0;
    }

    
    public void ActivateAbility(Vector2 optionalTargetPosition = default)
    {
        if (CanBeActivated() == false)
        {
            Debug.Log("Can not be activated");
            return;
        }

        if (AbilityType == AbilityType.Automatic)
        {
            HandleAutomaticAbility();
        }
        else if (AbilityType == AbilityType.Activatable)
        {
            HandleActivatableAbility(optionalTargetPosition);
        }
        else if (AbilityType == AbilityType.Trigger)
        {
            HandleTriggerAbility(optionalTargetPosition);
        }
    }

    public void OnAbilityButtonUp()
    {

    }

    public void SetUpAbility(Ability ability)
    {
        var abilityType = ability.GetType();
        Ability = Activator.CreateInstance(abilityType) as Ability;

        if (isActivationTypeChanged == false)
        {
            AbilityType = Ability.AbilityType;
        }

        Ability.SetAbilitySlot(this);
        Ability.OnAbilityEquip(Stats);
    }

    public void RemoveAbility()
    {
        Ability.OnAbilityUnEquip(Stats);
        Ability = CH_AbilitiesManager.VoidAbility;
    }

    public void SetUpActivationType(AbilityType type)
    {
        AbilityType = type;

        isActivationTypeChanged = true;
    }

    public void RemoveActivationType()
    {
        if (Ability != null)
        {
            AbilityType = Ability.AbilityType;
        }

        isActivationTypeChanged = false;
    }

    public void SetAbilityBaseCooldown(float newCooldown) => Ability.SetBaseCooldown(newCooldown);
    public void SetAbilityBaseCooldownToOriginal() => Ability.SetBaseCooldownToOriginal();

    public void PutAbilityOnCooldown(float cooldown)
    {
        NextActivationTime = Time.time + cooldown;
    }

    public bool CanBeActivated()
    {
        if (AbilityType == AbilityType.Activatable)
        {
            if (NextActivationTime > Time.time)
                return false;

            if (Stats.CurrentMana < GetAbilityManacost())
                return false;

            if (Ability.CanBeActivated(Stats).isActivatable == false)
                return false;
        }
        else if (AbilityType == AbilityType.Automatic)
        {
            if (NextActivationTime > Time.time)
                return false;

            if (Ability.CanBeActivated(Stats).isActivatable == false)
                return false;
        }
        else if (AbilityType == AbilityType.Trigger)
        {
            if (NextActivationTime > Time.time)
                return false;
        }

        return true;
    }

    public void PeriodicCall(float periodicCallCD)
    {
        Ability.OnPeriodicCall(Stats, periodicCallCD);
    }

    private void HandleAutomaticAbility()
    {
        NextActivationTime = Time.time + GetAbilityCooldown();

        Ability.OnAbilityActivation(Stats, GetAimDirectionForAutomaticAbility(), true);
    }
    
    private void HandleActivatableAbility(Vector2 position = default)
    {
        NextActivationTime = Time.time + GetAbilityCooldown();

        Stats.ManaComponent.SpendMana(GetAbilityManacost());

        if (position == default)
        {
            Ability.OnAbilityActivation(Stats, GetAimPositionForActivatableAbility(), false);
        }
        else
        {
            Ability.OnAbilityActivation(Stats, position, false);
        }
    }

    private void HandleTriggerAbility(Vector2 position = default)
    {
        NextActivationTime = Time.time + GetAbilityCooldown();

        if (position == default)
        {
            Ability.OnAbilityActivation(Stats, GetAimDirectionForAutomaticAbility(), true);
        }
        else
        {
            Ability.OnAbilityActivation(Stats, position, true);
        }
    }

    private Vector2 GetAimPositionForActivatableAbility()
    {
        return Stats.Aim.transform.position;
    }

    private Vector2 GetAimDirectionForAutomaticAbility()
    {
        for (int i = minSeekRadius; i <= maxSeekRadius; i += seekStep)
        {
            var targets = Physics2D.OverlapCircleAll(Stats.transform.position, i, WeaponBehaviour.EnemyLayerMask);
            if (targets.Length <= 0)
            {
                continue;
            }
            else
            {
                return GetClosestTarget(targets);
            }
        }

        return Stats.transform.position + Quaternion.Euler(new Vector3(0, 0, UnityEngine.Random.Range(0, 360))) * Vector2.right;
    }

    private Vector2 GetClosestTarget(Collider2D[] colliders)
    {
        return colliders.OrderBy(x => Vector2.Distance(Stats.transform.position, x.transform.position)).First().transform.position;
    }

    public int GetAbilityManacost()
    {
        return (int)(Ability.Manacost *
            (1 + LSC.MagicSC.IncreaseManacostValue + Stats.GSC.MagicSC.IncreaseManacostValue) *
            (LSC.MagicSC.MoreManacostValue * Stats.GSC.MagicSC.MoreManacostValue) *
            (LSC.MagicSC.LessManacostValue * Stats.GSC.MagicSC.LessManacostValue) -
            (LSC.MagicSC.FlatManacostValue + Stats.GSC.MagicSC.FlatManacostValue)
            );
    }

    public float GetAbilityCooldown()
    {
        float value = (1 + LSC.MagicSC.IncreaseCooldownReductionValue + Stats.GSC.MagicSC.IncreaseCooldownReductionValue) *
            (LSC.MagicSC.MoreCooldownReductionValue * Stats.GSC.MagicSC.MoreCooldownReductionValue) *
            (LSC.MagicSC.LessCooldownReductionValue * Stats.GSC.MagicSC.LessCooldownReductionValue);

        if (value <= 0) { return float.MaxValue; }

        float returnValue = Ability.Cooldown * (1 / value) + (LSC.MagicSC.FlatCooldownValue + Stats.GSC.MagicSC.FlatCooldownValue);

        if (returnValue < 0)
            return 0.001f;
        else
            return returnValue;
    }

    public float GetAbilityAOE(float baseAOE)
    {
        return baseAOE *
            (1 + LSC.UtilitySC.IncreaseAreaValue + Stats.GSC.UtilitySC.IncreaseAreaValue) *
            (LSC.UtilitySC.MoreAreaValue * Stats.GSC.UtilitySC.MoreAreaValue) *
            (LSC.UtilitySC.LessAreaValue * Stats.GSC.UtilitySC.LessAreaValue);
    }

    public float GetAbilityDuration(float baseDuration)
    {
        return baseDuration * (1 + LSC.UtilitySC.IncreaseEffectDurationValue + Stats.GSC.UtilitySC.IncreaseEffectDurationValue)
                * (LSC.UtilitySC.MoreEffectDurationValue * Stats.GSC.UtilitySC.MoreEffectDurationValue)
                * (LSC.UtilitySC.LessEffectDurationValue * Stats.GSC.UtilitySC.LessEffectDurationValue);
    }

    public (Damage damage, bool isCritical) GetAbilityDamage(Damage baseDamage, float baseCritChance, params ModTag[] modTags)
    {
        if (modTags.Contains(ModTag.Projectile))
        {
            return (MagicBehaviour.UtilityDamageCalculationForProjectiles(this, baseDamage, baseCritChance, out bool _isCritical), _isCritical);
        }
        
        if (modTags.Contains(ModTag.Area))
        {
            return (MagicBehaviour.UtilityDamageCalculationForArea(this, baseDamage, baseCritChance, out bool _isCritical), _isCritical);
        }

        Debug.Log("No Tags matched for damage");

        return (new(0, 0, 0), false);
    }

    public Damage GetAbilityDamageNONCRIT(Damage baseDamage, params ModTag[] modTags)
    {
        if (modTags.Contains(ModTag.Projectile))
        {
            return MagicBehaviour.UtilityDamageCalculationForProjectilesNONCRIT(this, baseDamage);
        }

        if (modTags.Contains(ModTag.Area))
        {
            return MagicBehaviour.UtilityDamageCalculationForAreaNONCRIT(this, baseDamage);
        }

        Debug.Log("No Tags matched for damage");

        return new(0, 0, 0);
    }

    public Damage GetAbilityPerSecDOTDamage(Damage baseDamage, params ModTag[] modTags)
    {
        if (modTags.Contains(ModTag.Projectile))
        {
            return MagicBehaviour.UtilityDamageOverTimeCalculation(this, MagicBehaviour.UtilityDamageCalculationForProjectilesNONCRIT(this, baseDamage));
        }

        if (modTags.Contains(ModTag.Area))
        {
            return MagicBehaviour.UtilityDamageOverTimeCalculation(this, MagicBehaviour.UtilityDamageCalculationForAreaNONCRIT(this, baseDamage));
        }

        Debug.Log("No Tags matched for damage");

        return new(0, 0, 0);
    }

    public Damage GetTipAvarageDamage(Damage baseDamage, float baseCritChance, params ModTag[] modTags)
    {
        if (modTags.Contains(ModTag.Projectile))
        {
            return MagicBehaviour.UtilityAverageDamageCalculation(this, MagicBehaviour.UtilityDamageCalculationForProjectilesNONCRIT(this, baseDamage), baseCritChance);
        }

        if (modTags.Contains(ModTag.Area))
        {
            return MagicBehaviour.UtilityAverageDamageCalculation(this, MagicBehaviour.UtilityDamageCalculationForAreaNONCRIT(this, baseDamage), baseCritChance);
        }

        if (modTags.Contains(ModTag.DamageOverTime))
        {
            return GetAbilityPerSecDOTDamage(baseDamage, modTags);
        }

        Debug.Log("No Tags matched for damage");

        return new(0, 0, 0);
    }
}
