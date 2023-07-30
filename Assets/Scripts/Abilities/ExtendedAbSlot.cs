using UnityEngine;
using Database;
using System;

public class ExtendedAbSlot
{
    //private readonly CH_Stats stats;
    //private readonly int slotIndex;

    //private readonly CH_AbilitiesManager abilitiesManager;

    //private readonly AbilityModifierStats abilityModifierStats;

    //public Ability Ability { get; private set; } = new();

    //public float NextActivationTime { get; private set; }

    //public float NextTimeToResetAbilityStages { get; private set; }

    //public ExtendedAbSlot(CH_Stats stats, int slotIndex, CH_AbilitiesManager abilitiesManager, AbilityModifierStats abilityModifierStats)
    //{
    //    this.stats = stats;
    //    this.slotIndex = slotIndex;
    //    this.abilitiesManager = abilitiesManager;
    //    this.abilityModifierStats = abilityModifierStats;
    //}


    //public void ActivateAbility()
    //{
    //    if (CanBeActivated().isActivatable == false)
    //    {
    //        Debug.Log("Can not be activated");
    //        return;
    //    }

    //    if (Ability is MultipleStagesAbility multipleStages)
    //    {
    //        HandleMultipleStagesAbility(multipleStages);
    //    }
    //    else if (Ability is ToggleAbility toggleAbility)
    //    {
    //        HandleToggleAbility(toggleAbility);
    //    }
    //    else if (Ability is PassiveAbility)
    //    {
    //        HandlePassiveAbility();
    //    }
    //    else if (Ability is ChannelingAbility channelingAbility)
    //    {
    //        HandleChannelingAbility(channelingAbility);
    //    }
    //    else if (Ability is Ability ability)
    //    {
    //        HandleStandartAbility(ability);
    //    }
    //}

    //public void DeactivateAbility()
    //{
    //    if (Ability is ChannelingAbility channelingAbility)
    //    {
    //        if (channelingAbility.IsChanneling == false) { return; }

    //        channelingAbility.IsChanneling = false;
    //        NextActivationTime = Time.time + channelingAbility.Cooldown * stats.CooldownMultiplier * abilityModifierStats.CooldownMultipler;
    //        //abilitiesManager.PutAbilityOnRegularCD(slotIndex, channelingAbility.Cooldown * stats.CooldownMultiplier * abilityModifierStats.CooldownMultipler);
    //        channelingAbility.OnAbilityDeactivation(stats);
    //    }
    //}

    //public void StopCasting()
    //{
    //    if (Ability is ChannelingAbility channelingAbility)
    //    {
    //        if (channelingAbility.IsChanneling == false) { return; }

    //        channelingAbility.IsChanneling = false;
    //        NextActivationTime = Time.time + channelingAbility.Cooldown * stats.CooldownMultiplier * abilityModifierStats.CooldownMultipler;
    //        //abilitiesManager.PutAbilityOnRegularCD(slotIndex, channelingAbility.Cooldown * stats.CooldownMultiplier * abilityModifierStats.CooldownMultipler);
    //        channelingAbility.OnAbilityDeactivation(stats);
    //    }
    //    else if (Ability is ToggleAbility toggleAbility)
    //    {
    //        if (toggleAbility.IsTurnedOn == false) { return; }

    //        toggleAbility.IsTurnedOn = false;
    //        NextActivationTime = Time.time + toggleAbility.Cooldown * stats.CooldownMultiplier * abilityModifierStats.CooldownMultipler;
    //        //abilitiesManager.PutAbilityOnRegularCD(slotIndex, toggleAbility.Cooldown * stats.CooldownMultiplier * abilityModifierStats.CooldownMultipler);
    //    }
    //}

    //public void SetUpAbility(Ability ability)
    //{
    //    Ability = ability.CopyV2();
    //    //Ability.SetAbilitySlot(this);

    //    Ability.OnAbilityEquip(stats);
    //}

    //public void RemoveAbility()
    //{
    //    Ability.OnAbilityUnEquip(stats);
    //    Ability = CH_AbilitiesManager.VoidAbility;
    //}

    //public (bool isActivatable, string reasons) CanBeActivated()
    //{
    //    if (NextActivationTime > Time.time)
    //        return (false, "On Cooldown");

    //    if (stats.CurrentMana < Ability.Manacost * abilityModifierStats.ManacostMultipler)
    //        return (false, "Not enough mana");

    //    if (Ability.CanBeActivated(stats).isActivatable == false)
    //        return (Ability.CanBeActivated(stats).isActivatable, Ability.CanBeActivated(stats).reason);

    //    return (true, "ok");
    //}

    //private void HandleStandartAbility(Ability ability)
    //{
    //    NextActivationTime = Time.time + ability.Cooldown * stats.CooldownMultiplier * abilityModifierStats.CooldownMultipler;

    //    //ability.OnAbilityActivation(stats);

    //    stats.ManaComponent.SpendMana(ability.Manacost * abilityModifierStats.ManacostMultipler);
    //}

    //private void HandlePassiveAbility()
    //{
    //    return;
    //}

    //private void HandleToggleAbility(ToggleAbility ability)
    //{
    //    if (ability.IsTurnedOn)
    //        NextActivationTime = Time.time + ability.Cooldown * stats.CooldownMultiplier * abilityModifierStats.CooldownMultipler;
    //    else
    //        NextActivationTime = Time.time + ability.ActivationMiniCooldown * stats.CooldownMultiplier * abilityModifierStats.CooldownMultipler;

    //    if (ability.IsTurnedOn == false)
    //    {
    //        //ability.OnAbilityActivation(stats);

    //        stats.ManaComponent.SpendMana(ability.Manacost * abilityModifierStats.ManacostMultipler);

    //        ability.IsTurnedOn = true;
    //    }
    //    else
    //    {
    //        ability.IsTurnedOn = false;
    //    }
    //}

    //private void HandleMultipleStagesAbility(MultipleStagesAbility ability)
    //{
    //    if (ability.Stage != ability.MaxStages - 1)
    //        NextActivationTime = Time.time + ability.CooldownBetweenStages * stats.CooldownMultiplier * abilityModifierStats.CooldownMultipler;
    //    else
    //        NextActivationTime = Time.time + ability.Cooldown * stats.CooldownMultiplier * abilityModifierStats.CooldownMultipler;

    //    NextTimeToResetAbilityStages = Time.time + ability.TimeToReset;

    //    ability.ShouldReset = true;

    //    //ability.OnAbilityActivation(stats);

    //    ability.Stage++;
    //    ability.Stage %= ability.MaxStages;

    //    stats.ManaComponent.SpendMana(ability.Manacost * abilityModifierStats.ManacostMultipler);
    //}

    //private void HandleChannelingAbility(ChannelingAbility ability)
    //{
    //    NextActivationTime = Time.time + ability.ActivationMiniCooldown * stats.CooldownMultiplier * abilityModifierStats.CooldownMultipler;

    //   // ability.OnAbilityActivation(stats);

    //    stats.ManaComponent.SpendMana(ability.Manacost * abilityModifierStats.ManacostMultipler);

    //    ability.IsChanneling = true;
    //}

    //public void PeriodicCall(float periodicCallCD)
    //{
    //    if (Ability is MultipleStagesAbility multipleStages)
    //    {
    //        if (multipleStages.Stage <= 0) { return; }

    //        if (multipleStages.ShouldReset && Time.time > NextTimeToResetAbilityStages)
    //        {
    //            multipleStages.Stage = 0;
    //            NextActivationTime = Time.time + multipleStages.Cooldown * stats.CooldownMultiplier * abilityModifierStats.CooldownMultipler;
    //            multipleStages.ShouldReset = false;
    //            //abilitiesManager.PutAbilityOnRegularCD(slotIndex, multipleStages.Cooldown * stats.CooldownMultiplier * abilityModifierStats.CooldownMultipler);
    //        }
    //    }
    //    else if (Ability is ToggleAbility toggleAbility)
    //    {
    //        if (toggleAbility.IsTurnedOn == false) { return; }

    //        if (stats.CurrentMana >= toggleAbility.ManacostPerSec * abilityModifierStats.ManacostMultipler * periodicCallCD)
    //        {
    //            toggleAbility.OnPeriodicCall(stats, periodicCallCD);
    //            stats.ManaComponent.SpendMana(toggleAbility.ManacostPerSec * abilityModifierStats.ManacostMultipler * periodicCallCD);
    //        }
    //        else
    //        {
    //            toggleAbility.IsTurnedOn = false;
    //        }
    //    }
    //    else if (Ability is ChannelingAbility channelingAbility)
    //    {
    //        if (channelingAbility.IsChanneling == false) { return; }

    //        if (stats.CurrentMana >= channelingAbility.ManacostPerSec * abilityModifierStats.ManacostMultipler * periodicCallCD)
    //        {
    //            channelingAbility.OnPeriodicCall(stats, periodicCallCD);
    //            stats.ManaComponent.SpendMana(channelingAbility.ManacostPerSec * abilityModifierStats.ManacostMultipler * periodicCallCD);
    //        }
    //        else
    //        {
    //            channelingAbility.IsChanneling = false;
    //        }
    //    }
    //    else
    //    {
    //        Ability.OnPeriodicCall(stats, periodicCallCD);
    //    }
    //}
}
