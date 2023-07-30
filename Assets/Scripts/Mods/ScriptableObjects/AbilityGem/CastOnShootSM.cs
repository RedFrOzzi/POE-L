using UnityEngine;
using Database;

[CreateAssetMenu(fileName = "CastOnShootSM", menuName = "ScriptableObjects/SignatureMods/AbilityGem/CastOnShootSM")]
public class CastOnShootSM : AbilityGemSignatureMod
{
    [SerializeField, Range(0, 9)] private byte lessAttackDamageTier;
    [SerializeField, Space(10), Range(0.1f, 100000)] private float activationCooldown;

    private AbilityGem gem;

    public override void ApplySignatureMod(IEquipmentItem item)
    {
        if (item is AbilityGem abilityGem)
        {
            abilityGem.Description += $"\n{ModsDatabase.GlobalMods["LessGlobalAttackDamage"].TierValues[lessAttackDamageTier]} LESS attack damage";

            var implicitMod = ModsDatabase.GlobalMods["LessGlobalAttackDamage"].GetCopy();
            implicitMod.SetTier(lessAttackDamageTier);

            abilityGem.ModsHolder.SetImplicits(implicitMod);

            abilityGem.OnEquipAction = () =>
            {
                gem = abilityGem;

                abilityGem.AbilitiesManager.SetUpActivationTypeWithAbilityGem(abilityGem.AbilitySlotIndex, AbilityType.Trigger);

                abilityGem.AbilitiesManager.SetAbilityBaseCooldown(abilityGem.AbilitySlotIndex, activationCooldown);

                abilityGem.AbilitiesManager.Stats.Equipment.OnWeaponShot += OnShot;
            };

            abilityGem.OnUnEquipAction = () =>
            {
                abilityGem.AbilitiesManager.RemoveActivationTypeWithAbilityGem(abilityGem.AbilitySlotIndex);

                abilityGem.AbilitiesManager.SetAbilityBaseCooldownToOriginal(abilityGem.AbilitySlotIndex);

                abilityGem.AbilitiesManager.Stats.Equipment.OnWeaponShot -= OnShot;
            };
        }
    }

    private void OnShot()
    {
        gem.AbilitiesManager.ActivateAbilityByTrigger(gem.AbilitySlotIndex, gem.AbilitiesManager.Stats.Aim.position);
    }
}
