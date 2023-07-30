using System.Collections;
using UnityEngine;

public class Revolver : Weapon
{
    private Coroutine shootRoutine;
    private bool canShootContinuously;
    private Coroutine reloadRoutine;

    public override void Shoot(Equipment equipment)
    {
        if (IsReloading)
        {
            StopCoroutine(reloadRoutine);
            IsReloading = false;
            reloadRoutine = null;
        }

        if (canShootContinuously == false) { return; }

        if (equipment.Stats.CurrentAmmo <= 0) { Reload(equipment); }

        if (attackCD > Time.time) { return; }

        attackCD = Time.time + 1 / equipment.Stats.CurrentAttackSpeed;

        ProjectileSpawnBehavior(equipment, ProjectileBehavior);

        equipment.WeaponShot();
    }

    public override void ShootOnce(Equipment equipment)
    {
        if (IsReloading) { return; }

        if (equipment.Stats.CurrentAmmo <= 0) { Reload(equipment); }

        shootRoutine = StartCoroutine(ShootCoroutine(equipment));

        ProjectileSpawnBehavior(equipment, ProjectileBehavior);

        equipment.WeaponShot();
    }

    public override void OnShootButtonUp(Equipment equipment)
    {
        canShootContinuously = false;

        if (shootRoutine != null)
            StopCoroutine(shootRoutine);
    }

    private IEnumerator ShootCoroutine(Equipment equipment)
    {
        yield return new WaitForSeconds(1 / equipment.Stats.CurrentAttackSpeed);

        canShootContinuously = true;
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
