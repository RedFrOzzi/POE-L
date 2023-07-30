using UnityEngine;

public class MinigunExtraAS : Weapon
{
    public override void Shoot(Equipment equipment)
    {
        if (IsReloading) { return; }

        if (equipment.Stats.CurrentAmmo <= 0) { Reload(equipment); }

        if (attackCD > Time.time) { return; }

        attackCD = Time.time + ((1 / equipment.Stats.CurrentAttackSpeed) / (equipment.Stats.WeaponProjectileAmount + equipment.Stats.WeaponChainsAmount + equipment.Stats.WeaponPierceAmount));

        ProjectileSpawnBehavior(equipment, ProjectileBehavior);

        equipment.WeaponShot();
    }
}
