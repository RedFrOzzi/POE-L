using Database;
using UnityEngine;

[CreateAssetMenu(fileName = "ShotgunBleedSM", menuName = "ScriptableObjects/SignatureMods/Weapon/ShotgunBleedSM")]
public class ShotgunBleedSM : SignatureMod
{
    [SerializeField] private int maxStacks;
    [SerializeField, Range(0f, 1f)] private float PercentDamageToBleed;
    [SerializeField] private float duration;

    private BleedEffect bleedEffect;

    public override void ApplySignatureMod(IEquipmentItem equipmentItem)
    {
        bleedEffect = new()
        {
            GeneratedID = equipmentItem.ID,
            MaxStacks = maxStacks,
            PercentDamageToBleed = PercentDamageToBleed,
            Duration = duration
        };

        equipmentItem.Description += $"\n{bleedEffect.Description()}";

        if (equipmentItem is Weapon weapon)
        {
            weapon.ProjectileSpawnBehavior = WeaponBehaviour.SpawnShotgunShot;
            weapon.ProjectileBehavior = WeaponBehaviour.ProjectileBehaviourBase;
            weapon.ReloadBehavior = WeaponBehaviour.ReloadShotgun;

            weapon.OnEquipAction = () =>
            {
                weapon.Equipment.Stats.OnHit.AddOnHit_GivingHit(bleedEffect);
            };

            weapon.OnUnEquipAction = () =>
            {
                weapon.Equipment.Stats.OnHit.RemoveOnHit_GivingHit(equipmentItem.ID);
            };
        }
    }
}
