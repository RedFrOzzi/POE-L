using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PistolSM", menuName = "ScriptableObjects/SignatureMods/Weapon/PistolSM")]
public class PistolSM : SignatureMod
{
    public override void ApplySignatureMod(IEquipmentItem equipmentItem)
    {
        if (equipmentItem is Weapon weapon)
        {           
            weapon.ProjectileSpawnBehavior = WeaponBehaviour.SpawnSimpleShot;
            weapon.ProjectileBehavior = WeaponBehaviour.ProjectileBehaviourBase;
            weapon.ReloadBehavior = WeaponBehaviour.ReloadSimple;
        }
    }
}
