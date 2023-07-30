using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemSpawnWeights", menuName = "ScriptableObjects/Weights/ItemSpawnWeights")]
public class ItemSpawnWeights : ScriptableObject
{
    [field: SerializeField] public float PercentChanceToDropItem { get; set; } = 10f;


    [field: SerializeField, Space(10)] public int WeaponDropWeight { get; private set; } = 0;
    [field: SerializeField] public int LeftHandDropWeight { get; private set; } = 0;
    [field: SerializeField] public int BodyArmorDropWeight { get; private set; } = 0;
    [field: SerializeField] public int HelmetDropWeight { get; private set; } = 0;
    [field: SerializeField] public int GlovesDropWeight { get; private set; } = 0;
    [field: SerializeField] public int BootsDropWeight { get; private set; } = 0;
    [field: SerializeField] public int AbilityGemDropWeight { get; private set; } = 0;
    [field: SerializeField] public int SuperAbilityGemDropWeight { get; private set; } = 0;

    public float OverallItemsWeight { get; private set; }

    private float[] itemWeights;
    private float[] itemChances;

    public void SetUpChances()
    {
        itemWeights = new float[] { WeaponDropWeight, LeftHandDropWeight, BodyArmorDropWeight, HelmetDropWeight,
        GlovesDropWeight, BootsDropWeight, AbilityGemDropWeight, SuperAbilityGemDropWeight };

        OverallItemsWeight = itemWeights.Sum();

        itemChances = new float[itemWeights.Length];

        for (int i = 0; i < itemWeights.Length; i++)
        {
            itemChances[i] = itemWeights[i] / OverallItemsWeight;
        }
    }

    public EquipmentSlot GetWeightedItem()
    {
        float chance = UnityEngine.Random.value;

        float accum = 0f;
        for (int i = 0; i < itemChances.Length; i++)
        {
            accum += itemChances[i];
            if (chance < accum)
            {
                //Далее варианты расположены в том же порядке что и их веса в массиве весов
                return i switch
                {
                    0 => EquipmentSlot.Weapon,
                    1 => EquipmentSlot.LeftHand,
                    2 => EquipmentSlot.BodyArmor,
                    3 => EquipmentSlot.Helmet,
                    4 => EquipmentSlot.Gloves,
                    5 => EquipmentSlot.Boots,
                    6 => EquipmentSlot.AbilityGem,

                    _ => EquipmentSlot.SuperAbilityGem,
                };
            }
        }

        return EquipmentSlot.None;
    }
}
