using UnityEngine;

public class Minigun : Weapon
{
    public override void Shoot(Equipment equipment)
    {
        if (IsReloading) { return; }

        if (equipment.Stats.CurrentAmmo <= 0) { Reload(equipment); }

        if (attackCD > Time.time) { return; }

        attackCD = Time.time + (1 / equipment.Stats.CurrentAttackSpeed) / equipment.Stats.WeaponProjectileAmount;

        ProjectileSpawnBehavior(equipment, ProjectileBehavior);

        equipment.WeaponShot();
    }
}
