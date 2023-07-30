using System;
using UnityEngine;

public class WeaponGem : Item, IEquipmentItem, IWeaponItem
{
    public StatsChanges LSC { get; } = new();

    [field: SerializeField] public EquipmentSlot EquipmentSlot { get; set; }
    [field: SerializeField] public EquipmentType EquipmentType { get; set; }
    [field: SerializeField] public SignatureMod SignatureMod { get; set; }
    public ModsHolder ModsHolder { get; set; }
    public Action OnEquipAction { get; set; }
    public Action OnUnEquipAction { get; set; }

    public Equipment Equipment { get; set; }
    public byte WeaponIndex { get; set; }

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
