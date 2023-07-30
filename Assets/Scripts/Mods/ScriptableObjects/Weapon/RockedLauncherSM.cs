using UnityEngine;
using Database;

[CreateAssetMenu(fileName = "RockedLauncherSM", menuName = "ScriptableObjects/SignatureMods/RockedLauncherSM")]
public class RockedLauncherSM : SignatureMod
{
    [SerializeField] private float distance;

    private KnockbackEffect knockbackEffect;

    public override void ApplySignatureMod(IEquipmentItem equipmentItem)
    {
        knockbackEffect = new()
        {
            GeneratedID = equipmentItem.ID,
            Distance = distance
        };

        if (equipmentItem is Weapon weapon)
        {
            weapon.ProjectileSpawnBehavior = WeaponBehaviour.SpawnRockedLauncherShot;
            weapon.ProjectileBehavior = WeaponBehaviour.ProjectileBehaviourRockedLauncher;
            weapon.ReloadBehavior = WeaponBehaviour.ReloadSimple;

            weapon.OnEquipAction = () =>
            {
                weapon.Equipment.Stats.OnHit.AddOnHit_GivingHit(knockbackEffect);
            };

            weapon.OnUnEquipAction = () =>
            {
                weapon.Equipment.Stats.OnHit.RemoveOnHit_GivingHit(equipmentItem.ID);
            };
        }
    }
}
