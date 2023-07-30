using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnChancesDatabase", menuName = "ScriptableObjects/SpawnChancesDatabase/Database")]
public class SpawnChancesDatabase : ScriptableObject
{
    [SerializeField] private ItemSpawnWeights itemSpawnWeights;
    [SerializeField] private EquipmentTypeWeights equipmentTypeWeights;


    public void Initialize()
    {
        itemSpawnWeights.SetUpChances();
        equipmentTypeWeights.SetUpChances();
    }

    public float GetPercentChanceToDropItem()
    {
        return itemSpawnWeights.PercentChanceToDropItem;
    }

    public EquipmentSlot GetWeightedEquipmentSlotType()
    {
        return itemSpawnWeights.GetWeightedItem();
    }

    public EquipmentType GetWeightedEquipmentTypeForSlot(EquipmentSlot equipmentSlot)
    {
        return equipmentSlot switch
        {
            EquipmentSlot.Weapon => equipmentTypeWeights.GetWeightedWeaponEquipmentType(),

            EquipmentSlot.AbilityGem => equipmentTypeWeights.GetWeightedAbilityGemEquipmentType(),

            EquipmentSlot.SuperAbilityGem => equipmentTypeWeights.GetWeightedSuperAbilityGemEquipmentType(),

            EquipmentSlot.BodyArmor => equipmentTypeWeights.GetWeightedBodyArmorEquipmentType(),

            EquipmentSlot.Gloves => equipmentTypeWeights.GetWeightedGlovesEquipmentType(),

            EquipmentSlot.Boots => equipmentTypeWeights.GetWeightedBootsEquipmentType(),

            EquipmentSlot.Helmet => equipmentTypeWeights.GetWeightedHelmetEquipmentType(),

            EquipmentSlot.LeftHand => equipmentTypeWeights.GetWeightedLeftHandEquipmentType(),

            _ => equipmentTypeWeights.GetWeightedWeaponEquipmentType(),
        };
    }
}
