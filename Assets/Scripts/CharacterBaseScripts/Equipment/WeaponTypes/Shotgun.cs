using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{
    private Coroutine reloadRoutine;

    public override void Shoot(Equipment equipment)
    {
        if (IsReloading)
        {
            StopCoroutine(reloadRoutine);
            IsReloading = false;
            reloadRoutine = null;
        }

        if (equipment.Stats.CurrentAmmo <= 0) { Reload(equipment); }

        if (attackCD > Time.time) { return; }

        attackCD = Time.time + 1 / equipment.Stats.CurrentAttackSpeed;

        ProjectileSpawnBehavior(equipment, ProjectileBehavior);

        equipment.WeaponShot();
    }

    public override void Reload(Equipment equipment)
    {
        if (reloadRoutine == null)
        {
            reloadRoutine = StartCoroutine(ReloadCoroutine(equipment));
        }
    }

    private IEnumerator ReloadCoroutine(Equipment equipment)
    {
        IsReloading = true;

        while (equipment.Stats.CurrentAmmo < equipment.Stats.CurrentAmmoCapacity)
        {
            yield return new WaitForSeconds(1 / (equipment.Stats.CurrentReloadSpeed * equipment.Stats.CurrentAmmoCapacity));

            ReloadBehavior(equipment.Stats);
        }

        IsReloading = false;
        reloadRoutine = null;
    }
}
