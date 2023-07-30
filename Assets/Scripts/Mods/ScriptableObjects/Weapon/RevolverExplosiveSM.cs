using UnityEngine;

[CreateAssetMenu(fileName = "RevolverExplosiveSM", menuName = "ScriptableObjects/SignatureMods/Weapon/RevolverExplosiveSM")]
public class RevolverExplosiveSM : SignatureMod
{
    public override void ApplySignatureMod(IEquipmentItem equipmentItem)
    {
        if (equipmentItem is Weapon weapon)
        {
            weapon.ProjectileSpawnBehavior = WeaponBehaviour.SpawnPistolShot;
            weapon.ProjectileBehavior = WeaponBehaviour.ProjectileBehaviourHitBehind;
            weapon.ReloadBehavior = WeaponBehaviour.ReloadShotgun;
        }
    }
}
