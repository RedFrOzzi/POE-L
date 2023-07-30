using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Database;

[CreateAssetMenu(fileName = "ChargingWeaponProjectilesSM", menuName = "ScriptableObjects/SignatureMods/Weapon/ChargingWeaponProjectilesSM")]
public class ChargingWeaponProjectilesSM : SignatureMod
{
    public override void ApplySignatureMod(IEquipmentItem equipmentItem)
    {
        if (equipmentItem is Weapon weapon)
        {
            weapon.ProjectileSpawnBehavior = WeaponBehaviour.SpawnChargedShotProjAmount;
            weapon.ProjectileBehavior = WeaponBehaviour.ProjectileBehaviourBase;
            weapon.ReloadBehavior = WeaponBehaviour.ReloadSimple;
        }
    }
}
