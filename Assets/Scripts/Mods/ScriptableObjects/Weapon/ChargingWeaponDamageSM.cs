using UnityEngine;

[CreateAssetMenu(fileName = "ChargingWeaponDamageSM", menuName = "ScriptableObjects/SignatureMods/Weapon/ChargingWeaponDamageSM")]
public class ChargingWeaponDamageSM : SignatureMod
{
    public override void ApplySignatureMod(IEquipmentItem equipmentItem)
    {
        if (equipmentItem is Weapon weapon)
        {
            weapon.ProjectileSpawnBehavior = WeaponBehaviour.SpawnChargedShotDamage;
            weapon.ProjectileBehavior = WeaponBehaviour.ProjectileBehaviourBase;
            weapon.ReloadBehavior = WeaponBehaviour.ReloadSimple;
        }
    }
}
