using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Database;

public class CH_AbilitiesManager : MonoBehaviour
{
    public CH_Stats Stats { get; private set; }

    public CH_AbilitiesEquipment AbilitiesEquipment { get; private set; }

    public static VoidAbility VoidAbility { get; private set; }
    public static float CooldownReductionHardCap { get; private set; } = 0.1f;

    private const float periodicCallCD = 0.2f;
    private float nextPeriodicCallTime = 0;

    private readonly AbilityGenerator abilityGenerator = new();

    private readonly StatsChanges[] statsChanges = new StatsChanges[6];

    private readonly AbilitySlot[] abilitySlots  = new AbilitySlot[6];

    public Action<byte> OnAbilityChange;

    public event Action<int, float> CDOnAbilityActivation;

    

    private void Awake()
    {
        Stats = GetComponent<CH_Stats>();
        AbilitiesEquipment = GetComponent<CH_AbilitiesEquipment>();
        VoidAbility = new VoidAbility();
    }

    private void Start()
    {
        for (byte i = 0; i < abilitySlots.Length; i++)
        {
            statsChanges[i] = new();

            abilitySlots[i] = new AbilitySlot(Stats, i, statsChanges[i], this);
        }

        Invoke(nameof(TempAbilGenerator), 1);
        Invoke(nameof(TempAbilGen2), 3);
        Invoke(nameof(AbilRemover), 1.2f);
    }

    private void Update()
    {
        if (GameFlowManager.Instance.IsGamePaused) { return; }

        if (Stats.CanCast == false) { return; }

        ActivateAutomaticAbilities();

        if (nextPeriodicCallTime > Time.time) { return; }
        nextPeriodicCallTime = Time.time + periodicCallCD;

        PeriodicCall();
    }


    private void ActivateAutomaticAbilities()
    {
        foreach (var slot in abilitySlots)
        {
            if (slot.Ability != VoidAbility && slot.AbilityType == AbilityType.Automatic && slot.NextActivationTime < Time.time)
            {
                slot.ActivateAbility();

                HandleCooldownVisuals(slot.SlotIndex);
            }
        }
    }

    public void ActivateAbilityManualy(byte abilityIndex, Vector2 optionalTargetPosition = default)
    {
        if (abilitySlots[abilityIndex].Ability == VoidAbility)
        {
            Debug.Log($"{abilityIndex} Ability slot is empty");
            return;
        }

        if (abilitySlots[abilityIndex].AbilityType != AbilityType.Activatable)
        {
            Debug.Log($"{abilityIndex} Ability is not activatable manualy");
            return;
        }

        if (abilitySlots[abilityIndex].NextActivationTime > Time.time)
        {
            Debug.Log($"{abilityIndex} Ability is on cooldown");
            return;
        }

        if (Stats.CurrentMana < abilitySlots[abilityIndex].GetAbilityManacost())
        {
            Debug.Log($"{abilityIndex} Ability require more mana");
            return;
        }

        abilitySlots[abilityIndex].ActivateAbility(optionalTargetPosition);
        HandleCooldownVisuals(abilityIndex);
    }

    public void ActivateAbilityByTrigger(byte abilityIndex, Vector2 optionalTargetPosition = default)
    {
        if (abilitySlots[abilityIndex].NextActivationTime > Time.time)
        {
            Debug.Log($"{abilityIndex} Ability is on cooldown");
            return;
        }

        if (abilitySlots[abilityIndex].AbilityType != AbilityType.Trigger)
        {
            Debug.Log($"{abilityIndex} Ability is not triggered");
            return;
        }

        abilitySlots[abilityIndex].ActivateAbility(optionalTargetPosition);
        HandleCooldownVisuals(abilityIndex);
    }

    public void OnAbilityButtonUp(byte abilityIndex)
    {
        abilitySlots[abilityIndex].OnAbilityButtonUp();
    }

    public void StopAllCasting()
    {

    }

    public void SetUpAbility(byte slotIndex, Ability ability)
    {
        abilitySlots[slotIndex].SetUpAbility(ability);

        OnAbilityChange?.Invoke(slotIndex);
    }

    public void RemoveAbility(byte slotIndex)
    {
        AbilitiesEquipment.UnequipEveryGemForChoosenAbilitySlot(slotIndex);

        abilitySlots[slotIndex].RemoveAbility();

        OnAbilityChange?.Invoke(slotIndex);
    }

    public void SwapAbility(byte slotIndex, Ability newAbility)
    {
        AbilitiesEquipment.UnequipEveryGemForChoosenAbilitySlot(slotIndex);

        abilitySlots[slotIndex].RemoveAbility();

        abilitySlots[slotIndex].SetUpAbility(newAbility);

        OnAbilityChange?.Invoke(slotIndex);
    }

    public void SetUpActivationTypeWithAbilityGem(byte slotIndex, AbilityType type)
    {
        abilitySlots[slotIndex].SetUpActivationType(type);
    }

    public void RemoveActivationTypeWithAbilityGem(byte slotIndex)
    {
        abilitySlots[slotIndex].RemoveActivationType();
    }

    public void SetAbilityBaseCooldown(byte slotIndex, float newCooldown) => abilitySlots[slotIndex].SetAbilityBaseCooldown(newCooldown);

    public void SetAbilityBaseCooldownToOriginal(byte slotIndex) => abilitySlots[slotIndex].SetAbilityBaseCooldownToOriginal();

    public void PutAbilityOnCooldown(byte slotIndex, float cooldown)
    {
        abilitySlots[slotIndex].PutAbilityOnCooldown(cooldown);

        PutAbilityOnRegularCD(slotIndex, cooldown);
    }

    public Ability GetAbility(byte slotIndex)
    {
        return abilitySlots[slotIndex].Ability;
    }

    public void ChangeAbilitiesStats(Func<AbilitySlot[], ModTag[], StatsChanges[]> queryToChooseAbilities, Action<StatsChanges[]> changeStatsAction, params ModTag[] queryTags)
    {
        var LSCs = queryToChooseAbilities?.Invoke(abilitySlots, queryTags);

        changeStatsAction?.Invoke(LSCs);
    }

    public AbilitySlot[] GetAbilitySlotRefs()
    {
        return abilitySlots;
    }

    private void PeriodicCall()
    {
        for (int i = 0; i < abilitySlots.Length; i++)
        {            
            abilitySlots[i].PeriodicCall(periodicCallCD);
        }
    }

    private void HandleCooldownVisuals(byte index)
    {
        PutAbilityOnRegularCD(index, GetAbilityCooldown(index));
    }

    private void PutAbilityOnRegularCD(byte slotIndex, float cooldown)
    {
        CDOnAbilityActivation?.Invoke(slotIndex, cooldown);
    }

    public void RemoveStatsFromOldGem(byte abilityNum, AbilityGem abilityGem)
    {
        statsChanges[abilityNum].RemoveChanges(abilityGem.LSC);
    }

    public void ApplyStatsFromNewGem(byte abilityNum, AbilityGem abilityGem)
    {
        statsChanges[abilityNum].CombineChanges(abilityGem.LSC);
    }

    private float GetAbilityCooldown(byte index)
    {
        return abilitySlots[index].GetAbilityCooldown();
    }

    //-----------------------------------
    [ContextMenuItem("AddLevel", "AddLevel")]
    public int lvl = 0;
    public void AddLevel()
    {
        statsChanges[0].MagicSC.AddFlatAbilityLevel(lvl);
    }



    public void TempAbilGenerator()
    {
        //SetUpAbility(0, new FireBallAbility());
        //SetUpAbility(1, new LightningStormAbility());
        //SetUpAbility(0, new Molotov());
        //SetUpAbility(1, new DamageImmunity());
        //SetUpAbility(1, new FireStorm());
        //SetUpAbility(1, new Bomb());
        //SetUpAbility(1, new PoisonTrail());
        //SetUpAbility(2, new AuraOfDecayAbility());
        //SetUpAbility(3, new DashAbility());
        //SetUpAbility(4, new BlinkAbility());
        //SetUpAbility(4, new Repel());
        //SetUpAbility(4, new Shuriken());
        //SetUpAbility(4, new ChainLightning());
        //SetUpAbility(4, new Stomp());

    }

    private void TempAbilGen2()
    {
        //SetUpAbility(0, new BlinkAbility());
        //SetUpAbility(1, new BlinkAbility());
        //SetUpAbility(2, new BlinkAbility());
        //SetUpAbility(3, new BlinkAbility());
        //SetUpAbility(4, new BlinkAbility());
        //SetUpAbility(5, new BlinkAbility());
    }

    private void AbilRemover()
    {
        //RemoveAbility(1);
    }

    private void Calculate()
    {
        float minDamage = 20;
        float maxDamage = 20;
        float critChance = 2;
        float critDamage = 150;
        float attackSpeed = 1f;

        float min = minDamage * (100 - critChance) + minDamage * (critDamage * 0.01f) * critChance;
        float max = maxDamage * (100 - critChance) + maxDamage * (critDamage * 0.01f) * critChance;
        Debug.Log((min + max) / 2 * attackSpeed);
    }
}
