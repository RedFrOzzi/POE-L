using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EquipmentItem : Item, IEquipmentItem
{    
    [field: SerializeField] public EquipmentSlot EquipmentSlot { get; set; }
    [field: SerializeField] public EquipmentType EquipmentType { get; set; }
    public Equipment Equipment { get; set; }
    [field: SerializeField] public SignatureMod SignatureMod { get; set; }
    public ModsHolder ModsHolder { get; set; }

    public StatsChanges LSC { get; } = new();
    public Action OnEquipAction { get; set; } //чтобы можно было переписывать в модах, а не создавать отдельные классы для каждого оружия
    public Action OnUnEquipAction { get; set; }

    public virtual void Initialize() { }

    public virtual void EvaluateLocalStats() { }

    public EquipmentType GetEquipmentType()
    {
        return EquipmentType;
    }
}
