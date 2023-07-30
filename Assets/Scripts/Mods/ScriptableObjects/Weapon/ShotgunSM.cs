using Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ShotgunSM", menuName = "ScriptableObjects/SignatureMods/Weapon/ShotgunSM")]
public class ShotgunSM : SignatureMod
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
            weapon.Description += $"\n{knockbackEffect.Description()}";

            weapon.ProjectileSpawnBehavior = WeaponBehaviour.SpawnShotgunShot;
            weapon.ProjectileBehavior = WeaponBehaviour.ProjectileBehaviourBase;
            weapon.ReloadBehavior = WeaponBehaviour.ReloadShotgun;

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
