using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnProbabilityCalculator
{
    private readonly SpawnChancesDatabase sC;

    public ItemSpawnProbabilityCalculator(SpawnChancesDatabase spawnChances)
    {
        sC = spawnChances;
    }

    public float GetPercentChanceToDropItem() => sC.GetPercentChanceToDropItem();

    public bool ShouldSpawnItem()
    {
        if (UnityEngine.Random.Range(0, 100f) > sC.GetPercentChanceToDropItem())        
            return false;
        
        return true;
    }

    public EquipmentSlot GetWeightedEquipmentSlotType()
    {
        return sC.GetWeightedEquipmentSlotType();
    }

    public EquipmentType GetWeightedEquipmentType(EquipmentSlot slot)
    {
        return sC.GetWeightedEquipmentTypeForSlot(slot);
    }
}
