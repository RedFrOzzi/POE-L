using UnityEngine;
using Database;

[CreateAssetMenu(fileName = "CastOnCriticalHitSM", menuName = "ScriptableObjects/SignatureMods/AbilityGem/CastOnCriticalHitSM")]
public class CastOnCriticalHitSM : AbilityGemSignatureMod
{
    [SerializeField, Range(0, 9)] private byte ImplicitModTier;
    [SerializeField, Space(10), Range(0.1f, 100000)] private float activationCooldown;

    public override void ApplySignatureMod(IEquipmentItem item)
    {
        if (item is AbilityGem abilityGem)
        {
            abilityGem.Description += $"\nSpell got {ModsDatabase.AbilityGemMods["MoreLocalSpellDamage"].TierValues[ImplicitModTier]}% MORE spell damage";

            var implicitMod = ModsDatabase.AbilityGemMods["MoreLocalSpellDamage"].GetCopy();
            implicitMod.SetTier(ImplicitModTier);

            abilityGem.ModsHolder.SetImplicits(implicitMod);

            abilityGem.OnEquipAction = () =>
            {
                abilityGem.AbilitiesManager.SetUpActivationTypeWithAbilityGem(abilityGem.AbilitySlotIndex, AbilityType.Trigger);

                abilityGem.AbilitiesManager.SetAbilityBaseCooldown(abilityGem.AbilitySlotIndex, activationCooldown);

                var onHit = new CastOnCritEffect
                {
                    AbilityIndex = abilityGem.AbilitySlotIndex,
                    GeneratedID = abilityGem.ID
                };
                abilityGem.AbilitiesManager.Stats.OnHit.AddOnHit_GivingHit(onHit);
            };

            abilityGem.OnUnEquipAction = () =>
            {
                abilityGem.AbilitiesManager.RemoveActivationTypeWithAbilityGem(abilityGem.AbilitySlotIndex);

                abilityGem.AbilitiesManager.SetAbilityBaseCooldownToOriginal(abilityGem.AbilitySlotIndex);

                abilityGem.AbilitiesManager.Stats.OnHit.RemoveOnHit_GivingHit(abilityGem.ID);
            };
        }
    }
}
