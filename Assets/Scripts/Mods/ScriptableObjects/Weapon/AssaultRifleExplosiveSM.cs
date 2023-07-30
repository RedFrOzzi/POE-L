using UnityEngine;

[CreateAssetMenu(fileName = "AssaultRifleExplosiveSM", menuName = "ScriptableObjects/SignatureMods/Weapon/AssaultRifleExplosiveSM")]
public class AssaultRifleExplosiveSM : SignatureMod
{
    public override void ApplySignatureMod(IEquipmentItem equipmentItem)
    {
        if (equipmentItem is Weapon weapon)
        {
            weapon.ProjectileSpawnBehavior = WeaponBehaviour.SpawnPistolShot;
            weapon.ProjectileBehavior = WeaponBehaviour.ProjectileBehaviourHitBehind;
            weapon.ReloadBehavior = WeaponBehaviour.ReloadSimple;
        }
    }
}
