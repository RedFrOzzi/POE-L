using UnityEngine;

[CreateAssetMenu(fileName = "RevolverSM", menuName = "ScriptableObjects/SignatureMods/Weapon/RevolverSM")]
public class RevolverSM : SignatureMod
{
    public override void ApplySignatureMod(IEquipmentItem equipmentItem)
    {
        if (equipmentItem is Weapon weapon)
        {
            weapon.ProjectileSpawnBehavior = WeaponBehaviour.SpawnSimpleShot;
            weapon.ProjectileBehavior = WeaponBehaviour.ProjectileBehaviourBase;
            weapon.ReloadBehavior = WeaponBehaviour.ReloadShotgun;
        }
    }
}
