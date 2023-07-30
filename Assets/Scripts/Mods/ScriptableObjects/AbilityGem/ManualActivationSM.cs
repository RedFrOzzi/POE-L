using Database;
using UnityEngine;


[CreateAssetMenu(fileName = "ManualActivationSM", menuName = "ScriptableObjects/SignatureMods/AbilityGem/ManualActivationSM")]
public class ManualActivationSM : AbilityGemSignatureMod
{
    [SerializeField, Range(0, 9)] private byte spellDamageModTier;
    [SerializeField, Range(0, 9)] private byte decreaseCDModTier;

    public override void ApplySignatureMod(IEquipmentItem item)
    {
        if (item is AbilityGem abilityGem)
        {
            abilityGem.Description += $"\nIncrease spell damage for linked ability by {ModsDatabase.AbilityGemMods["IncreaseLocalSpellDamage"].TierValues[spellDamageModTier]}%." +
                $"\nDecrease cooldown reduction for linked ability by {ModsDatabase.AbilityGemMods["DecreaseLocalCooldownRecovery"].TierValues[decreaseCDModTier]}%";

            abilityGem.EquipmentSlot = EquipmentSlot.SuperAbilityGem;
            abilityGem.Description = Description;
            abilityGem.Name = Name;

            var implicitMod = ModsDatabase.AbilityGemMods["IncreaseLocalSpellDamage"].GetCopy();
            implicitMod.SetTier(spellDamageModTier);

            var implicitMod2 = ModsDatabase.AbilityGemMods["DecreaseLocalCooldownRecovery"].GetCopy();
            implicitMod2.SetTier(decreaseCDModTier);

            abilityGem.ModsHolder.SetImplicits(implicitMod, implicitMod2);

            abilityGem.OnEquipAction = () =>
            {
                abilityGem.AbilitiesManager.SetUpActivationTypeWithAbilityGem(abilityGem.AbilitySlotIndex, AbilityType.Activatable);
            };

            abilityGem.OnUnEquipAction = () =>
            {
                abilityGem.AbilitiesManager.RemoveActivationTypeWithAbilityGem(abilityGem.AbilitySlotIndex);
            };
        }
    }
}
