using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class EnemyWeaponBehaviour
{
    public static int IndexOfProjectiles = 0;

    private const float spreadAngle = 10f;
    private const int EnemyProjectileLayer = 9; //layer for enemy projectiles
    private const int playerLayer = 6;

    public static LayerMask playerLayerMask;
    public static LayerMask enemyLayerMask;

    public const float ProjChainSeekRadius = 6f;

    static EnemyWeaponBehaviour()
    {
        playerLayerMask = LayerMask.GetMask("Player");
        enemyLayerMask = LayerMask.GetMask("Enemy");
    }

    private static Damage UtilityDamageCalculation(CH_Stats stats, out bool isCritical)
    {
        if (stats.CurrentCritChance >= UnityEngine.Random.Range(0f, 100f))
        {
            isCritical = true;
            return new Damage(UnityEngine.Random.Range(stats.CurrentMinDamage, stats.CurrentMaxDamage + 1) * stats.CurrentAttackCritMultiplier, 0, 0);
        }
        else
        {
            isCritical = false;
            return new Damage(UnityEngine.Random.Range(stats.CurrentMinDamage, stats.CurrentMaxDamage + 1), 0, 0);
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------

    public static void SpawnSimpleShot(CH_Stats stats, GameObject projectile, Transform projParent, float projSpeed, Vector2 direction, Action<BaseProjectile, Collider2D> projBehavior)
    {
        if (stats.CurrentAmmo <= 0)
        {
            ReloadSimple(stats);
            return;
        }
        else
        {
            stats.AmmoChange(-1);
        }

        Vector2 firstAngle = new();
        Rigidbody2D rigidbody2D;
        BaseProjectile baseProjectile;
        Damage damage = UtilityDamageCalculation(stats, out bool isCritical);
        bool _isCritical = isCritical;

        for (int i = 0; i < stats.WeaponProjectileAmount; i++)
        {

            GameObject go = GameObject.Instantiate(projectile, stats.transform.position, Quaternion.identity, projParent);

            rigidbody2D = go.GetComponent<Rigidbody2D>();
            baseProjectile = go.GetComponent<BaseProjectile>();

            go.layer = EnemyProjectileLayer;

            if (i == 0)
            {
                firstAngle = Quaternion.Euler(new Vector3
                    (0, 0, UnityEngine.Random.Range(stats.PercentAccuracy * (spreadAngle / 2) - (spreadAngle / 2),
                    (spreadAngle / 2) - stats.PercentAccuracy * (spreadAngle / 2)))) *                                 //добавляем угол к направлению (угол между min и max angle)
                    direction.normalized *                                                      //задаем направление в сторону цели
                    projSpeed;

                rigidbody2D.velocity = firstAngle;
            }
            else if (i % 2 == 0)
            {
                rigidbody2D.velocity = Quaternion.Euler(new Vector3(0, 0, i * 2)) * firstAngle;
            }
            else
            {
                rigidbody2D.velocity = Quaternion.Euler(new Vector3(0, 0, i * -2)) * firstAngle;
            }

            baseProjectile.ShooterStats = stats;

            baseProjectile.ProjectileBehavior = projBehavior;
            baseProjectile.ChainsLeft = stats.WeaponChainsAmount;
            baseProjectile.PierceLeft = stats.WeaponPierceAmount;

            baseProjectile.ProjDestroyTime = Time.time + 15;
            baseProjectile.ProjSpeed = projSpeed;
            baseProjectile.ProjIndex = IndexOfProjectiles;
            baseProjectile.IsMultipleProjWork = false;
            baseProjectile.LayerMask = playerLayerMask;

            baseProjectile.Damage = damage;
            baseProjectile.IsCritical = _isCritical;
        }

        IndexOfProjectiles++;
    }

    //---------------------------------------------------------------------------------------------------------------------------

    public static void ProjectileBehaviorEnemy(BaseProjectile baseProjectile, Collider2D collision)
    {
        if (baseProjectile.PierceLeft > 0)
        {
            baseProjectile.PierceLeft--;
        }
        else if (baseProjectile.ChainsLeft > 0)
        {
            baseProjectile.ChainsLeft--;

            baseProjectile.Colliders.Add(collision);

            Vector2 position = baseProjectile.transform.position;

            var colliders = Physics2D.OverlapCircleAll(position, ProjChainSeekRadius, baseProjectile.LayerMask);

            if (colliders.Length <= 0)
            {
                GameObject.Destroy(baseProjectile.gameObject);
                return;
            }

            colliders = colliders.OrderBy(col => ((Vector2)col.transform.position - position).magnitude).ToArray();

            for (int l = 0; l < colliders.Length; l++)
            {
                if (colliders[l] == null || baseProjectile.Colliders.Contains(colliders[l]))
                {
                    if (l == colliders.Length - 1)
                    {
                        GameObject.Destroy(baseProjectile.gameObject);
                        break;
                    }

                    continue;
                }
                else
                {
                    baseProjectile.RB.velocity = (colliders[l].transform.position - baseProjectile.transform.position).normalized * baseProjectile.ProjSpeed;
                    baseProjectile.Colliders.Add(colliders[l]);
                    break;
                }
            }
        }
        else
        {
            GameObject.Destroy(baseProjectile.gameObject);
        }
    }

    //---------------------------------------------------------------------------------------------------------------------------

    public static void ReloadSimple(CH_Stats stats)
    {
        stats.AdditionalEffects.FullControllLoss(1 / stats.CurrentReloadSpeed);
        stats.ReplanishAmmo();
    }
}
