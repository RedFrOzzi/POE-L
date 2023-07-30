using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityGem : Item, IEquipmentItem
{
    public readonly StatsChanges LSC = new();


    [field: SerializeField] public EquipmentSlot EquipmentSlot { get; set; }
    [field: SerializeField] public EquipmentType EquipmentType { get; set; }

    public byte AbilitySlotIndex { get; set; }
    public CH_AbilitiesEquipment AbilitiesEquipment { get; set; }
    public CH_AbilitiesManager AbilitiesManager { get; set; }
    public Equipment Equipment { get; set; }

    [field: SerializeField] public SignatureMod SignatureMod { get; set; }
    public ModsHolder ModsHolder { get; set; }

    public Action OnEquipAction { get; set; }       //чтобы можно было переписывать в модах, а не создавать отдельные классы для каждого гема
    public Action OnUnEquipAction { get; set; }

    public void Initialize()
    {
        OnEquipAction = () => { return; };
        OnUnEquipAction = () => { return; };

        SignatureMod.Initialize(this);
        
        ModsHolder = new(EquipmentSlot, this);

        SignatureMod.ApplySignatureMod(this);

        ModsHolder.GenerateInitialMods();
    }

    public EquipmentType GetEquipmentType()
    {
        return EquipmentType;
    }
}
