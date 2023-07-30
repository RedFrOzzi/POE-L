using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH_AbilitiesEquipment : MonoBehaviour
{
    public CH_Stats Stats { get; private set; }
    private Inventory inventory;

    public CH_AbilitiesManager AbilitiesManager { get; private set; }

    public AbilityGem[] Ability01EquipedGems { get; private set; } = new AbilityGem[5];
    public AbilityGem[] Ability02EquipedGems { get; private set; } = new AbilityGem[5];
    public AbilityGem[] Ability03EquipedGems { get; private set; } = new AbilityGem[5];
    public AbilityGem[] Ability04EquipedGems { get; private set; } = new AbilityGem[5];
    public AbilityGem[] Ability05EquipedGems { get; private set; } = new AbilityGem[5];
    public AbilityGem[] Ability06EquipedGems { get; private set; } = new AbilityGem[5];

    public AbilityGem[][] AbilitiesEquipment { get; private set; }


    /// <summary>
    /// номер абилки (слева направо) \ номер слота(снизу вверх)
    /// </summary>
    public event Action<byte, byte, AbilityGem> OnAbilityGemChange;

    private void Awake()
    {
        Stats = GetComponent<CH_Stats>();
        inventory = GetComponent<Inventory>();
        AbilitiesManager = GetComponent<CH_AbilitiesManager>();

        AbilitiesEquipment = new AbilityGem[6][] { Ability01EquipedGems, Ability02EquipedGems,
        Ability03EquipedGems, Ability04EquipedGems, Ability05EquipedGems, Ability06EquipedGems};
    }

    public void EquipGem(byte abilityNum, byte gemSlotNum, AbilityGem abilityGem)
    {
        if (abilityGem == null || !inventory.InventoryList.Contains(abilityGem)) { return; }

        if (AbilitiesEquipment[abilityNum][gemSlotNum] == null)
        {
            AbilitiesEquipment[abilityNum][gemSlotNum] = abilityGem;
            abilityGem.AbilitiesEquipment = this;
            abilityGem.AbilitiesManager = AbilitiesManager;
            abilityGem.AbilitySlotIndex = abilityNum;
            inventory.RemoveItem(abilityGem);
            abilityGem.OnEquipAction();

            ApplyStatsFromNewItem(abilityNum, abilityGem, null);
        }
        else
        {
            SwapGem(abilityNum, gemSlotNum, abilityGem);
        }

        OnAbilityGemChange?.Invoke(abilityNum, gemSlotNum, abilityGem);
    }

    public void SwapGem(byte abilityNum, byte gemSlotNum, AbilityGem abilityGem)
    {
        if (abilityGem == null || !inventory.InventoryList.Contains(abilityGem) || AbilitiesEquipment[abilityNum][gemSlotNum] == null) { return; }

        AbilityGem oldGem = AbilitiesEquipment[abilityNum][gemSlotNum];

        //перемещаем предмет из экипировки в инвентарь
        inventory.AddItem(oldGem);
        oldGem.OnUnEquipAction();
        //добовляем предмет в экипировку
        AbilitiesEquipment[abilityNum][gemSlotNum] = abilityGem;
        abilityGem.AbilitiesEquipment = this;
        abilityGem.AbilitiesManager = AbilitiesManager;
        abilityGem.AbilitySlotIndex = abilityNum;
        abilityGem.OnEquipAction();
        //убераем экипированный предмет из инвенеторя
        inventory.RemoveItem(abilityGem);

        ApplyStatsFromNewItem(abilityNum, abilityGem, oldGem);

        OnAbilityGemChange?.Invoke(abilityNum, gemSlotNum, abilityGem);
    }

    public void UnEquipGem(byte abilityNum, byte gemSlotNum)
    {

        AbilityGem oldGem = AbilitiesEquipment[abilityNum][gemSlotNum];

        if (oldGem == null) { return; }

        inventory.AddItem(oldGem);
        oldGem.OnUnEquipAction();
        AbilitiesEquipment[abilityNum][gemSlotNum] = null;

        ApplyStatsFromNewItem(abilityNum, null, oldGem);

        OnAbilityGemChange?.Invoke(abilityNum, gemSlotNum, null);
    }

    public void UnequipEveryGemForChoosenAbilitySlot(byte abilityIndex)
    {
        for (byte i = 0; i < AbilitiesEquipment[abilityIndex].Length; i++)
        {
            UnEquipGem(abilityIndex, i);
        }
    }

    private void ApplyStatsFromNewItem(byte abilityNum, AbilityGem newGem, AbilityGem oldGem)
    {
        if (oldGem != null)
        {
            AbilitiesManager.RemoveStatsFromOldGem(abilityNum, oldGem); //Локальные моды

            oldGem.ModsHolder.RemoveGlobalModsModifiers(); //Глобальные моды

            Stats.EvaluateStats();
        }

        if (newGem != null)
        {
            AbilitiesManager.ApplyStatsFromNewGem(abilityNum, newGem); //Локальные моды

            newGem.ModsHolder.ApplyGlobalModsModifiers(Stats);  //Глобальные моды

            Stats.EvaluateStats();
        }
    }
}
