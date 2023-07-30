using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Database;
using System.Threading.Tasks;
using UnityEngine.AI;
using System.Linq;

public static class WeaponBehaviour
{
    public static Dictionary<string, GameObject> ProjectilesGameObjects { get; private set; }
    public static Dictionary<string, Material> Materials { get; private set; }

    public static int IndexOfProjectiles { get; set; } = 0;

    public static LayerMask PlayerLayerMask { get; private set; }
    public static LayerMask EnemyLayerMask { get; private set; }
    public static LayerMask WallLayerMask { get; private set; }
    public const int PlayerProjectileLayer = 8;
    public const int EnemyLayer = 7;
    public const int WallLayer = 11;

    public const float ProjChainSeekRadius = 6f;

    public const string WallHitAnimation = "FireSparks_01";
    public static readonly Vector2 WallHitAnimationScale = new(0.4f, 0.4f);

    static WeaponBehaviour()
    {
        PlayerLayerMask = LayerMask.GetMask("Player");
        EnemyLayerMask = LayerMask.GetMask("Enemy");
        WallLayerMask = LayerMask.GetMask("Wall");

        var projs = Resources.LoadAll<GameObject>("Projectiles/PhysicalProjectiles");
        ProjectilesGameObjects = new();
        foreach (GameObject go in projs)
            ProjectilesGameObjects.Add(go.name, go);

        var materials = Resources.LoadAll<Material>("Materials");
        Materials = new();
        foreach (Material mat in materials)
            Materials.Add(mat.name, mat);
    }

    /// <summary>
    /// Утилита для подсчета урона в соответствии со статами
    /// </summary>    
    private static Damage AttackDamageCalculation(CH_Stats stats, out bool isCritical)
    {
        if (stats.CurrentCritChance >= UnityEngine.Random.Range(0f, 100f))
        {
            isCritical = true;
            return new Damage(UnityEngine.Random.Range(stats.CurrentMinDamage, stats.CurrentMaxDamage) * stats.CurrentAttackCritMultiplier, 0, 0);            
        }
        else
        {
            isCritical = false;
            return new Damage(UnityEngine.Random.Range(stats.CurrentMinDamage, stats.CurrentMaxDamage), 0, 0);            
        }
    }

    //--------------------PROJECTILE-SPAWN------------------------

    /// <summary>
    /// Простой выстрел
    /// </summary>    
    public static void SpawnSimpleShot(Equipment equipment, Action<BaseProjectile, Collider2D> projBehavior)
    {
        if (equipment.Stats.CurrentAmmo <= 0)
        {
            //on 0 ammo effect
            return;
        }
        else
        {
            equipment.Stats.AmmoChange(-1);
        }

        Vector2 firstAngle = new();
        Rigidbody2D rigidbody2D;
        BaseProjectile baseProjectile;
        Damage damage = AttackDamageCalculation(equipment.Stats, out bool isCritical);

        for (int i = 0; i < equipment.Stats.WeaponProjectileAmount; i++)
        {

            GameObject go = GameObject.Instantiate(ProjectilesGameObjects["SimpleBullet"], equipment.Player.transform.position, Quaternion.identity, equipment.ProjectileParentObject.transform);

            rigidbody2D = go.GetComponent<Rigidbody2D>();
            baseProjectile = go.GetComponent<BaseProjectile>();            

            go.layer = PlayerProjectileLayer;
           
            if (i == 0)
            {
                firstAngle = Quaternion.Euler(new Vector3
                    (0, 0, UnityEngine.Random.Range(equipment.Stats.PercentAccuracy * (equipment.Stats.CurrentSpreadAngle / 2) - (equipment.Stats.CurrentSpreadAngle / 2),
                    (equipment.Stats.CurrentSpreadAngle / 2) - equipment.Stats.PercentAccuracy * (equipment.Stats.CurrentSpreadAngle / 2)))) *
                    (equipment.AimGO.transform.position - equipment.Player.transform.position).normalized;

                rigidbody2D.velocity = firstAngle * equipment.Stats.CurrentProjectileSpeed;

                go.transform.rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(new Vector3(0, 0, 90)) * firstAngle);
            }
            else if (i % 2 == 0)
            {
                rigidbody2D.velocity = Quaternion.Euler(new Vector3(0, 0, i * 2)) * firstAngle * equipment.Stats.CurrentProjectileSpeed;

                go.transform.rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(new Vector3(0, 0, 90 + i * 2)) * firstAngle);
            }
            else
            {
                rigidbody2D.velocity = Quaternion.Euler(new Vector3(0, 0, i * -2)) * firstAngle * equipment.Stats.CurrentProjectileSpeed;

                go.transform.rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(new Vector3(0, 0, 90 + i * -2)) * firstAngle);
            }

            baseProjectile.ShooterStats = equipment.Stats;

            baseProjectile.ProjectileBehavior = projBehavior;
            baseProjectile.ChainsLeft = equipment.Stats.WeaponChainsAmount;
            baseProjectile.PierceLeft = equipment.Stats.WeaponPierceAmount;            

            baseProjectile.ProjDestroyTime = Time.time + equipment.ProjLiveTime;
            baseProjectile.ProjSpeed = equipment.Stats.CurrentProjectileSpeed;
            baseProjectile.ProjIndex = IndexOfProjectiles;
            baseProjectile.IsMultipleProjWork = false;
            baseProjectile.LayerMask = EnemyLayerMask;
            baseProjectile.ShooterInitialPosition = equipment.transform.position;

            baseProjectile.ImpactAnimation = "BloodSmall_01";
            baseProjectile.ImpactAnimationRotation = -90;
            baseProjectile.ImpactAnimationScale = Vector3.one;
            baseProjectile.TrailRenderer.enabled = true;
            baseProjectile.TrailRenderer.material = Materials["PistolBulletTrailMaterial"];

            baseProjectile.Damage = damage;
            baseProjectile.IsCritical = isCritical;
        }

        IndexOfProjectiles++;
    }

    public static void SpawnChargedShotProjAmount(Equipment equipment, Action<BaseProjectile, Collider2D> projBehavior)
    {
        if (equipment.EqupipmentList[EquipmentSlot.Weapon] is not ChargingWeapon chargingWeapon) { return; }

        if (equipment.Stats.CurrentAmmo <= 0)
        {
            //on 0 ammo effect
            return;
        }
        else
        {
            equipment.Stats.AmmoChange(-1);
        }

        Vector2 firstAngle = new();
        Rigidbody2D rigidbody2D;
        BaseProjectile baseProjectile;
        int modifiedProjAmount = (int)(equipment.Stats.WeaponProjectileAmount + chargingWeapon.Charge - 1);
        Damage damage = AttackDamageCalculation(equipment.Stats, out bool isCritical);

        for (int i = 0; i < modifiedProjAmount; i++)
        {
            GameObject go = GameObject.Instantiate(ProjectilesGameObjects["SimpleBullet"], equipment.Player.transform.position, Quaternion.identity, equipment.ProjectileParentObject.transform);

            rigidbody2D = go.GetComponent<Rigidbody2D>();
            baseProjectile = go.GetComponent<BaseProjectile>();

            go.layer = PlayerProjectileLayer;

            if (i == 0)
            {
                firstAngle = Quaternion.Euler(new Vector3
                    (0, 0, UnityEngine.Random.Range(equipment.Stats.PercentAccuracy * (equipment.Stats.CurrentSpreadAngle / 2) - (equipment.Stats.CurrentSpreadAngle / 2),
                    (equipment.Stats.CurrentSpreadAngle / 2) - equipment.Stats.PercentAccuracy * (equipment.Stats.CurrentSpreadAngle / 2)))) *
                    (equipment.AimGO.transform.position - equipment.Player.transform.position).normalized *
                    equipment.Stats.CurrentProjectileSpeed;                                                                       

                rigidbody2D.velocity = firstAngle;

                go.transform.rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(new Vector3(0, 0, 90)) * firstAngle);
            }
            else if (i % 2 == 0)
            {
                rigidbody2D.velocity = Quaternion.Euler(new Vector3(0, 0, i * 2)) * firstAngle;

                go.transform.rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(new Vector3(0, 0, 90 + i * 2)) * firstAngle);
            }
            else
            {
                rigidbody2D.velocity = Quaternion.Euler(new Vector3(0, 0, i * -2)) * firstAngle;

                go.transform.rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(new Vector3(0, 0, 90 + i * -2)) * firstAngle);
            }

            baseProjectile.ShooterStats = equipment.Stats;

            baseProjectile.ProjectileBehavior = projBehavior;
            baseProjectile.ChainsLeft = equipment.Stats.WeaponChainsAmount;
            baseProjectile.PierceLeft = equipment.Stats.WeaponPierceAmount;

            baseProjectile.ProjDestroyTime = Time.time + equipment.ProjLiveTime;
            baseProjectile.ProjSpeed = equipment.Stats.CurrentProjectileSpeed;
            baseProjectile.ProjIndex = IndexOfProjectiles;
            baseProjectile.IsMultipleProjWork = false;
            baseProjectile.LayerMask = EnemyLayerMask;
            baseProjectile.ShooterInitialPosition = equipment.transform.position;

            baseProjectile.ImpactAnimation = "BloodSmall_01";
            baseProjectile.ImpactAnimationRotation = -90;
            baseProjectile.ImpactAnimationScale = Vector3.one;
            baseProjectile.TrailRenderer.enabled = true;
            baseProjectile.TrailRenderer.material = Materials["PistolBulletTrailMaterial"];

            baseProjectile.Damage = damage;
            baseProjectile.IsCritical = isCritical;
        }

        IndexOfProjectiles++;
    }

    public static void SpawnChargedShotDamage(Equipment equipment, Action<BaseProjectile, Collider2D> projBehavior)
    {
        if (equipment.EqupipmentList[EquipmentSlot.Weapon] is not ChargingWeapon chargingWeapon) { return; }

        if (equipment.Stats.CurrentAmmo <= 0)
        {
            //on 0 ammo effect
            return;
        }
        else
        {
            equipment.Stats.AmmoChange(-1);
        }

        Vector2 firstAngle = new();
        Rigidbody2D rigidbody2D;
        BaseProjectile baseProjectile;
        Damage damage = new(AttackDamageCalculation(equipment.Stats, out bool isCritical).Physical * chargingWeapon.Charge * chargingWeapon.ChargeDamageModifier, 0, 0);

        for (int i = 0; i < equipment.Stats.WeaponProjectileAmount; i++)
        {
            GameObject go = GameObject.Instantiate(ProjectilesGameObjects["SimpleBullet"], equipment.Player.transform.position, Quaternion.identity, equipment.ProjectileParentObject.transform);

            rigidbody2D = go.GetComponent<Rigidbody2D>();
            baseProjectile = go.GetComponent<BaseProjectile>();

            go.layer = PlayerProjectileLayer;

            if (i == 0)
            {
                firstAngle = Quaternion.Euler(new Vector3
                    (0, 0, UnityEngine.Random.Range(equipment.Stats.PercentAccuracy * (equipment.Stats.CurrentSpreadAngle / 2) - (equipment.Stats.CurrentSpreadAngle / 2),
                    (equipment.Stats.CurrentSpreadAngle / 2) - equipment.Stats.PercentAccuracy * (equipment.Stats.CurrentSpreadAngle / 2)))) *
                    (equipment.AimGO.transform.position - equipment.Player.transform.position).normalized *
                    equipment.Stats.CurrentProjectileSpeed;

                rigidbody2D.velocity = firstAngle;

                go.transform.rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(new Vector3(0, 0, 90)) * firstAngle);
            }
            else if (i % 2 == 0)
            {
                rigidbody2D.velocity = Quaternion.Euler(new Vector3(0, 0, i * 2)) * firstAngle;

                go.transform.rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(new Vector3(0, 0, 90 + i * 2)) * firstAngle);
            }
            else
            {
                rigidbody2D.velocity = Quaternion.Euler(new Vector3(0, 0, i * -2)) * firstAngle;

                go.transform.rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(new Vector3(0, 0, 90 + i * -2)) * firstAngle);
            }

            baseProjectile.ShooterStats = equipment.Stats;

            baseProjectile.ProjectileBehavior = projBehavior;
            baseProjectile.ChainsLeft = equipment.Stats.WeaponChainsAmount;
            baseProjectile.PierceLeft = equipment.Stats.WeaponPierceAmount;

            baseProjectile.ProjDestroyTime = Time.time + equipment.ProjLiveTime;
            baseProjectile.ProjSpeed = equipment.Stats.CurrentProjectileSpeed;
            baseProjectile.ProjIndex = IndexOfProjectiles;
            baseProjectile.IsMultipleProjWork = false;
            baseProjectile.LayerMask = EnemyLayerMask;
            baseProjectile.ShooterInitialPosition = equipment.transform.position;

            baseProjectile.ImpactAnimation = "BloodSmall_01";
            baseProjectile.ImpactAnimationRotation = -90;
            baseProjectile.ImpactAnimationScale = Vector3.one;
            baseProjectile.TrailRenderer.enabled = true;
            baseProjectile.TrailRenderer.material = Materials["PistolBulletTrailMaterial"];

            baseProjectile.Damage = damage;
            baseProjectile.IsCritical = isCritical;
        }

        IndexOfProjectiles++;
    }

    public static void SpawnShotgunShot(Equipment equipment, Action<BaseProjectile, Collider2D> _)
    {
        if (equipment.Stats.CurrentAmmo <= 0)
        {
            //on 0 ammo effect
            return;
        }
        else
        {
            equipment.Stats.AmmoChange(-1);
        }

        Vector2 angle = new();
        List<CH_Stats> hitTargets = new();
        Vector3 direction = (equipment.AimGO.transform.position - equipment.Player.position).normalized;
        Damage damage = AttackDamageCalculation(equipment.Stats, out bool isCritical);
        DamageArgs damageArgs = new(damage, isCritical, equipment.Stats, null, DamageArgs.DamageSource.SingleTarget);

        for (int i = 0; i < equipment.Stats.WeaponProjectileAmount * 5; i++) //увеличиваем колличество ударов в 5 раз
        {
            Vector3 rotation = new(0, 0, UnityEngine.Random.Range(-equipment.Stats.CurrentSpreadAngle / 2, equipment.Stats.CurrentSpreadAngle / 2));

            angle = Quaternion.Euler(rotation) * direction;

            RaycastHit2D[] hits = Physics2D.RaycastAll(equipment.Player.position, angle, equipment.Stats.CurrentAttackRange, EnemyLayerMask);

            //Pierce logic: Shotgun pierce count = pierceAmount + chainsAmount. OnHit only on 1st target for each reycast array
            for (int j = 0; j < hits.Length && j <= equipment.Stats.WeaponPierceAmount + equipment.Stats.WeaponChainsAmount; j++)
            {
                if (hits[j] == true && hits[j].collider.TryGetComponent(out CH_Stats enemyStats))
                {
                    damageArgs.EnemyStats = enemyStats;

                    equipment.Stats.DamageFilter.OutgoingDAMAGE(damageArgs);

                    if (j == 0 && hitTargets.Contains(enemyStats) == false)
                        hitTargets.Add(enemyStats);

                    //Visuals
                    AnimationPlayer.Instance.Play("BloodSmall_04", hits[j].point, Quaternion.identity, Vector3.one, 1.5f);
                }
            }

            AnimationPlayer.Instance.Play("BulletTracer_03", equipment.transform.position, Quaternion.FromToRotation(Vector3.right, angle), new Vector3(equipment.Stats.CurrentAttackRange * 0.65f, 1, 1), 3f);
        }

        AnimationPlayer.Instance.Play("MuzleEffect_02", direction * 0.2f + equipment.transform.position, Quaternion.FromToRotation(Vector3.right, angle), Vector3.one, 2);

        

        foreach (var enmySts in hitTargets)
        {
            damageArgs = new(damage, isCritical, equipment.Stats, enmySts, DamageArgs.DamageSource.Projectile);

            equipment.Stats.DamageFilter.OutgoingAttackEFFECT(damageArgs);
        }
    }

    public static void SpawnPistolShot(Equipment equipment, Action<BaseProjectile, Collider2D> projBehavior)
    {
        if (equipment.Stats.CurrentAmmo <= 0)
        {
            //on 0 ammo effect
            return;
        }
        else
        {
            equipment.Stats.AmmoChange(-1);
        }

        Vector2 firstAngle = new();
        Rigidbody2D rigidbody2D;
        BaseProjectile baseProjectile;
        Damage damage = AttackDamageCalculation(equipment.Stats, out bool isCritical);

        for (int i = 0; i < equipment.Stats.WeaponProjectileAmount; i++)
        {

            GameObject go = GameObject.Instantiate(ProjectilesGameObjects["SimpleBullet"], equipment.Player.transform.position, Quaternion.identity, equipment.ProjectileParentObject.transform);

            rigidbody2D = go.GetComponent<Rigidbody2D>();
            baseProjectile = go.GetComponent<BaseProjectile>();

            go.layer = PlayerProjectileLayer;

            if (i == 0)
            {
                firstAngle = Quaternion.Euler(new Vector3
                    (0, 0, UnityEngine.Random.Range(equipment.Stats.PercentAccuracy * (equipment.Stats.CurrentSpreadAngle / 2) - (equipment.Stats.CurrentSpreadAngle / 2),
                    (equipment.Stats.CurrentSpreadAngle / 2) - equipment.Stats.PercentAccuracy * (equipment.Stats.CurrentSpreadAngle / 2)))) *
                    (equipment.AimGO.transform.position - equipment.Player.transform.position).normalized;

                rigidbody2D.velocity = firstAngle * equipment.Stats.CurrentProjectileSpeed;

                go.transform.rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(new Vector3(0, 0, 90)) * firstAngle);
            }
            else if (i % 2 == 0)
            {
                rigidbody2D.velocity = Quaternion.Euler(new Vector3(0, 0, i * 2)) * firstAngle * equipment.Stats.CurrentProjectileSpeed;

                go.transform.rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(new Vector3(0, 0, 90 + i * 2)) * firstAngle);
            }
            else
            {
                rigidbody2D.velocity = Quaternion.Euler(new Vector3(0, 0, i * -2)) * firstAngle * equipment.Stats.CurrentProjectileSpeed;

                go.transform.rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(new Vector3(0, 0, 90 + i * -2)) * firstAngle);
            }

            baseProjectile.ShooterStats = equipment.Stats;

            baseProjectile.ProjectileBehavior = projBehavior;
            baseProjectile.ChainsLeft = equipment.Stats.WeaponChainsAmount;
            baseProjectile.PierceLeft = equipment.Stats.WeaponPierceAmount;

            baseProjectile.ProjDestroyTime = Time.time + equipment.ProjLiveTime;
            baseProjectile.ProjSpeed = equipment.Stats.CurrentProjectileSpeed;
            baseProjectile.ProjIndex = IndexOfProjectiles;
            baseProjectile.IsMultipleProjWork = false;
            baseProjectile.LayerMask = EnemyLayerMask;
            baseProjectile.ShooterInitialPosition = equipment.transform.position;

            baseProjectile.ImpactAnimation = "FireExplosion_01";
            baseProjectile.ImpactAnimationRotation = -90f;
            baseProjectile.ImpactAnimationScale = Vector3.one;
            baseProjectile.TrailRenderer.enabled = true;
            baseProjectile.TrailRenderer.material = Materials["PistolBulletTrailMaterial"];

            baseProjectile.Damage = damage;
            baseProjectile.IsCritical = isCritical;
        }

        IndexOfProjectiles++;
    }

    public static void SpawnAssaultRifleShot(Equipment equipment, Action<BaseProjectile, Collider2D> projBehavior)
    {
        if (equipment.Stats.CurrentAmmo <= 0)
        {
            //on 0 ammo effect
            return;
        }
        else
        {
            equipment.Stats.AmmoChange(-1);
        }

        Vector2 firstAngle = new();
        Rigidbody2D rigidbody2D;
        BaseProjectile baseProjectile;
        Damage damage = AttackDamageCalculation(equipment.Stats, out bool isCritical);

        for (int i = 0; i < equipment.Stats.WeaponProjectileAmount; i++)
        {

            GameObject go = GameObject.Instantiate(ProjectilesGameObjects["SimpleBullet"], equipment.Player.transform.position, Quaternion.identity, equipment.ProjectileParentObject.transform);

            rigidbody2D = go.GetComponent<Rigidbody2D>();
            baseProjectile = go.GetComponent<BaseProjectile>();

            go.layer = PlayerProjectileLayer;

            if (i == 0)
            {
                firstAngle = Quaternion.Euler(new Vector3
                    (0, 0, UnityEngine.Random.Range(equipment.Stats.PercentAccuracy * (equipment.Stats.CurrentSpreadAngle / 2) - (equipment.Stats.CurrentSpreadAngle / 2),
                    (equipment.Stats.CurrentSpreadAngle / 2) - equipment.Stats.PercentAccuracy * (equipment.Stats.CurrentSpreadAngle / 2)))) *
                    (equipment.AimGO.transform.position - equipment.Player.transform.position).normalized;

                rigidbody2D.velocity = firstAngle * equipment.Stats.CurrentProjectileSpeed;

                go.transform.rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(new Vector3(0, 0, 90)) * firstAngle);
            }
            else if (i % 2 == 0)
            {
                rigidbody2D.velocity = Quaternion.Euler(new Vector3(0, 0, i * 2)) * firstAngle * equipment.Stats.CurrentProjectileSpeed;

                go.transform.rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(new Vector3(0, 0, 90 + i * 2)) * firstAngle);
            }
            else
            {
                rigidbody2D.velocity = Quaternion.Euler(new Vector3(0, 0, i * -2)) * firstAngle * equipment.Stats.CurrentProjectileSpeed;

                go.transform.rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(new Vector3(0, 0, 90 + i * -2)) * firstAngle);
            }

            baseProjectile.ShooterStats = equipment.Stats;

            baseProjectile.ProjectileBehavior = projBehavior;
            baseProjectile.ChainsLeft = equipment.Stats.WeaponChainsAmount;
            baseProjectile.PierceLeft = equipment.Stats.WeaponPierceAmount;

            baseProjectile.ProjDestroyTime = Time.time + equipment.ProjLiveTime;
            baseProjectile.ProjSpeed = equipment.Stats.CurrentProjectileSpeed;
            baseProjectile.ProjIndex = IndexOfProjectiles;
            baseProjectile.IsMultipleProjWork = false;
            baseProjectile.LayerMask = EnemyLayerMask;
            baseProjectile.ShooterInitialPosition = equipment.transform.position; ;

            baseProjectile.ImpactAnimation = "BloodSmall_01";
            baseProjectile.ImpactAnimationRotation = -90;
            baseProjectile.ImpactAnimationScale = Vector3.one;
            baseProjectile.TrailRenderer.enabled = true;
            baseProjectile.TrailRenderer.material = Materials["PistolBulletTrailMaterial"];

            baseProjectile.Damage = damage;
            baseProjectile.IsCritical = isCritical;
        }

        IndexOfProjectiles++;
    }

    public static void SpawnMinigunShot(Equipment equipment, Action<BaseProjectile, Collider2D> projBehavior)
    {
        if (equipment.Stats.CurrentAmmo <= 0)
        {
            //on 0 ammo effect
            return;
        }
        else
        {
            equipment.Stats.AmmoChange(-1);
        }

        Vector2 angle;
        Rigidbody2D rigidbody2D;
        BaseProjectile baseProjectile;
        Damage damage = AttackDamageCalculation(equipment.Stats, out bool isCritical);

        GameObject go = GameObject.Instantiate(ProjectilesGameObjects["SimpleBullet"], equipment.Player.transform.position, Quaternion.identity, equipment.ProjectileParentObject.transform);

        rigidbody2D = go.GetComponent<Rigidbody2D>();
        baseProjectile = go.GetComponent<BaseProjectile>();

        go.layer = PlayerProjectileLayer;

        angle = Quaternion.Euler(new Vector3
                    (0, 0, UnityEngine.Random.Range(equipment.Stats.PercentAccuracy * (equipment.Stats.CurrentSpreadAngle / 2) - (equipment.Stats.CurrentSpreadAngle / 2),
                    (equipment.Stats.CurrentSpreadAngle / 2) - equipment.Stats.PercentAccuracy * (equipment.Stats.CurrentSpreadAngle / 2)))) *
                    (equipment.AimGO.transform.position - equipment.Player.transform.position).normalized;

        rigidbody2D.velocity = angle * equipment.Stats.CurrentProjectileSpeed;

        go.transform.rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(new Vector3(0, 0, 90)) * angle);

        baseProjectile.ShooterStats = equipment.Stats;

        baseProjectile.ProjectileBehavior = projBehavior;
        baseProjectile.ChainsLeft = equipment.Stats.WeaponChainsAmount;
        baseProjectile.PierceLeft = equipment.Stats.WeaponPierceAmount;

        baseProjectile.ProjDestroyTime = Time.time + equipment.ProjLiveTime;
        baseProjectile.ProjSpeed = equipment.Stats.CurrentProjectileSpeed;
        baseProjectile.ProjIndex = IndexOfProjectiles;
        baseProjectile.IsMultipleProjWork = false;
        baseProjectile.LayerMask = EnemyLayerMask;
        baseProjectile.ShooterInitialPosition = equipment.transform.position;

        baseProjectile.ImpactAnimation = "BloodSmall_01";
        baseProjectile.ImpactAnimationRotation = -90;
        baseProjectile.ImpactAnimationScale = Vector3.one;
        baseProjectile.TrailRenderer.enabled = true;
        baseProjectile.TrailRenderer.material = Materials["PistolBulletTrailMaterial"];

        baseProjectile.Damage = damage;
        baseProjectile.IsCritical = isCritical;

        IndexOfProjectiles++;
    }

    public static void SpawnRockedLauncherShot(Equipment equipment, Action<BaseProjectile, Collider2D> projBehavior)
    {
        if (equipment.Stats.CurrentAmmo <= 0)
        {
            //on 0 ammo effect
            return;
        }
        else
        {
            equipment.Stats.AmmoChange(-1);
        }

        Vector2 firstAngle = new();
        Rigidbody2D rigidbody2D;
        BaseProjectile baseProjectile;
        Damage damage = AttackDamageCalculation(equipment.Stats, out bool isCritical);

        for (int i = 0; i < equipment.Stats.WeaponProjectileAmount; i++)
        {

            GameObject go = GameObject.Instantiate(ProjectilesGameObjects["BasicProjectile"], equipment.Player.transform.position, Quaternion.identity, equipment.ProjectileParentObject.transform);

            rigidbody2D = go.GetComponent<Rigidbody2D>();
            baseProjectile = go.GetComponent<BaseProjectile>();

            go.layer = PlayerProjectileLayer;

            if (i == 0)
            {
                firstAngle = Quaternion.Euler(new Vector3
                    (0, 0, UnityEngine.Random.Range(equipment.Stats.PercentAccuracy * (equipment.Stats.CurrentSpreadAngle / 2) - (equipment.Stats.CurrentSpreadAngle / 2),
                    (equipment.Stats.CurrentSpreadAngle / 2) - equipment.Stats.PercentAccuracy * (equipment.Stats.CurrentSpreadAngle / 2)))) *
                    (equipment.AimGO.transform.position - equipment.Player.transform.position).normalized;

                rigidbody2D.velocity = firstAngle * equipment.Stats.CurrentProjectileSpeed;

                go.transform.rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(new Vector3(0, 0, 90)) * firstAngle);
            }
            else if (i % 2 == 0)
            {
                rigidbody2D.velocity = Quaternion.Euler(new Vector3(0, 0, i * 2)) * firstAngle * equipment.Stats.CurrentProjectileSpeed;

                go.transform.rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(new Vector3(0, 0, 90 + i * 2)) * firstAngle);
            }
            else
            {
                rigidbody2D.velocity = Quaternion.Euler(new Vector3(0, 0, i * -2)) * firstAngle * equipment.Stats.CurrentProjectileSpeed;

                go.transform.rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(new Vector3(0, 0, 90 + i * -2)) * firstAngle);
            }


            baseProjectile.ShooterStats = equipment.Stats;

            baseProjectile.ProjectileBehavior = projBehavior;
            baseProjectile.ChainsLeft = equipment.Stats.WeaponChainsAmount;
            baseProjectile.PierceLeft = equipment.Stats.WeaponPierceAmount;

            baseProjectile.ProjDestroyTime = Time.time + equipment.ProjLiveTime;
            baseProjectile.ProjSpeed = equipment.Stats.CurrentProjectileSpeed;
            baseProjectile.ProjIndex = IndexOfProjectiles;
            baseProjectile.IsMultipleProjWork = false;
            baseProjectile.LayerMask = EnemyLayerMask;
            baseProjectile.ShooterInitialPosition = equipment.transform.position;

            baseProjectile.ImpactAnimation = "Empty";
            baseProjectile.ImpactAnimationRotation = -90;
            baseProjectile.ImpactAnimationScale = Vector3.one;

            try
            {
                baseProjectile.SecondaryExplosionRadius = ((RocketLauncher)equipment.EqupipmentList[EquipmentSlot.Weapon]).SecondaryExplosionRadius *
                    (1 + equipment.Stats.GSC.UtilitySC.IncreaseAreaValue) *
                    equipment.Stats.GSC.UtilitySC.MoreAreaValue *
                    equipment.Stats.GSC.UtilitySC.LessAreaValue;

                baseProjectile.PercentOfDamageToExplosionDamage = ((RocketLauncher)equipment.EqupipmentList[EquipmentSlot.Weapon]).PercentOfDamageToExplosionDamage;
            }
            catch
            {
                Debug.Log("It's not a Rocked Launcher");
            }

            baseProjectile.Damage = damage;
            baseProjectile.IsCritical = isCritical;

            IndexOfProjectiles++;
        }
    }

    public static void SpawnGrenadeLauncherShot(Equipment equipment, Action<BaseProjectile, Collider2D> projBehavior)
    {
        if (equipment.Stats.CurrentAmmo <= 0)
        {
            //on 0 ammo effect
            return;
        }
        else
        {
            equipment.Stats.AmmoChange(-1);
        }

        Vector2 firstTargetPos = new();
        GrenadeProjectile grenadeProjectile;
        Damage damage = AttackDamageCalculation(equipment.Stats, out bool isCritical);

        for (int i = 0; i < equipment.Stats.WeaponProjectileAmount; i++)
        {

            GameObject go = GameObject.Instantiate(ProjectilesGameObjects["Grenade"], equipment.Player.transform.position, Quaternion.identity, equipment.ProjectileParentObject.transform);

            grenadeProjectile = go.GetComponent<GrenadeProjectile>();

            go.layer = PlayerProjectileLayer;

            if (i == 0)
            {
                firstTargetPos = (Vector2)grenadeProjectile.transform.position + Vector2.ClampMagnitude(equipment.AimGO.transform.position + (Vector3)UnityEngine.Random.insideUnitCircle - grenadeProjectile.transform.position, equipment.Stats.CurrentAttackRange);

                grenadeProjectile.TargetLocation = firstTargetPos;
            }
            else if (i % 2 == 0)
            {
                grenadeProjectile.TargetLocation = firstTargetPos + (Vector2.Perpendicular(firstTargetPos - (Vector2)equipment.Player.transform.position)).normalized * i;
            }
            else
            {
                grenadeProjectile.TargetLocation = firstTargetPos + (Vector2.Perpendicular((Vector2)equipment.Player.transform.position - firstTargetPos)).normalized * i;
            }


            grenadeProjectile.ShooterStats = equipment.Stats;

            grenadeProjectile.ProjectileBehavior = projBehavior;
            grenadeProjectile.ChainsLeft = equipment.Stats.WeaponChainsAmount;
            grenadeProjectile.PierceLeft = equipment.Stats.WeaponPierceAmount;

            grenadeProjectile.ProjSpeed = equipment.Stats.CurrentProjectileSpeed;
            grenadeProjectile.ProjIndex = IndexOfProjectiles;
            grenadeProjectile.IsMultipleProjWork = false;
            grenadeProjectile.LayerMask = EnemyLayerMask;
            grenadeProjectile.ShooterInitialPosition = equipment.transform.position;

            grenadeProjectile.ImpactAnimation = "Explosion_02";
            grenadeProjectile.ImpactAnimationRotation = 0;
            grenadeProjectile.ImpactAnimationScale = Vector3.one;

            try
            {
                grenadeProjectile.SecondaryExplosionRadius = ((GrenadeLauncher)equipment.EqupipmentList[EquipmentSlot.Weapon]).SecondaryExplosionRadius *
                    (1 + equipment.Stats.GSC.UtilitySC.IncreaseAreaValue) *
                    equipment.Stats.GSC.UtilitySC.MoreAreaValue *
                    equipment.Stats.GSC.UtilitySC.LessAreaValue;

                grenadeProjectile.PercentOfDamageToExplosionDamage = ((GrenadeLauncher)equipment.EqupipmentList[EquipmentSlot.Weapon]).PercentOfDamageToExplosionDamage;
                grenadeProjectile.ProjDestroyTime = Time.time + ((GrenadeLauncher)equipment.EqupipmentList[EquipmentSlot.Weapon]).ProjectileTimeToReachTarget;
                grenadeProjectile.ImpactAnimationScale = Vector3.one * ((GrenadeLauncher)equipment.EqupipmentList[EquipmentSlot.Weapon]).SecondaryExplosionRadius;
            }
            catch
            {
                Debug.Log("It's not a Grenade Launcher");
            }

            grenadeProjectile.Damage = damage;
            grenadeProjectile.IsCritical = isCritical;

            IndexOfProjectiles++;
        }
    }

    public static void SpawnGrenadeLauncherMK2Shot(Equipment equipment, Action<BaseProjectile, Collider2D> projBehavior)
    {
        if (equipment.Stats.CurrentAmmo <= 0)
        {
            //on 0 ammo effect
            return;
        }
        else
        {
            equipment.Stats.AmmoChange(-1);
        }

        Vector2 firstTargetPos = new();
        GrenadeProjectile grenadeProjectile;
        Damage damage = AttackDamageCalculation(equipment.Stats, out bool isCritical);

        for (int i = 0; i < equipment.Stats.WeaponProjectileAmount; i++)
        {

            GameObject go = GameObject.Instantiate(ProjectilesGameObjects["Grenade"], equipment.Player.transform.position, Quaternion.identity, equipment.ProjectileParentObject.transform);

            grenadeProjectile = go.GetComponent<GrenadeProjectile>();

            go.layer = PlayerProjectileLayer;

            if (i == 0)
            {
                firstTargetPos = (Vector2)grenadeProjectile.transform.position + Vector2.ClampMagnitude(equipment.AimGO.transform.position + (Vector3)UnityEngine.Random.insideUnitCircle - grenadeProjectile.transform.position, equipment.Stats.CurrentAttackRange);

                grenadeProjectile.TargetLocation = firstTargetPos;
            }
            else if (i % 2 == 0)
            {
                grenadeProjectile.TargetLocation = firstTargetPos + (Vector2.Perpendicular(firstTargetPos - (Vector2)equipment.Player.transform.position)).normalized * i;
            }
            else
            {
                grenadeProjectile.TargetLocation = firstTargetPos + (Vector2.Perpendicular((Vector2)equipment.Player.transform.position - firstTargetPos)).normalized * i;
            }


            grenadeProjectile.ShooterStats = equipment.Stats;

            grenadeProjectile.ProjectileBehavior = projBehavior;
            grenadeProjectile.ChainsLeft = equipment.Stats.WeaponChainsAmount;
            grenadeProjectile.PierceLeft = equipment.Stats.WeaponPierceAmount;

            grenadeProjectile.ProjSpeed = equipment.Stats.CurrentProjectileSpeed;
            grenadeProjectile.ProjIndex = IndexOfProjectiles;
            grenadeProjectile.IsMultipleProjWork = false;
            grenadeProjectile.LayerMask = EnemyLayerMask;
            grenadeProjectile.ShooterInitialPosition = equipment.transform.position;

            grenadeProjectile.ImpactAnimation = "Explosion_02";
            grenadeProjectile.ImpactAnimationRotation = 0;
            grenadeProjectile.ImpactAnimationScale = Vector3.one;

            try
            {
                grenadeProjectile.SecondaryExplosionRadius = ((GrenadeLauncher)equipment.EqupipmentList[EquipmentSlot.Weapon]).SecondaryExplosionRadius *
                    (1 + equipment.Stats.GSC.UtilitySC.IncreaseAreaValue) *
                    equipment.Stats.GSC.UtilitySC.MoreAreaValue *
                    equipment.Stats.GSC.UtilitySC.LessAreaValue;

                grenadeProjectile.PercentOfDamageToExplosionDamage = ((GrenadeLauncher)equipment.EqupipmentList[EquipmentSlot.Weapon]).PercentOfDamageToExplosionDamage;
                grenadeProjectile.ProjDestroyTime = Time.time + ((GrenadeLauncher)equipment.EqupipmentList[EquipmentSlot.Weapon]).ProjectileTimeToReachTarget;
                grenadeProjectile.ImpactAnimationScale = Vector3.one * ((GrenadeLauncher)equipment.EqupipmentList[EquipmentSlot.Weapon]).SecondaryExplosionRadius;
                grenadeProjectile.IsBouncing = true;
            }
            catch
            {
                Debug.LogWarning("It's not a Grenade Launcher");
            }

            grenadeProjectile.Damage = damage;
            grenadeProjectile.IsCritical = isCritical;

            IndexOfProjectiles++;
        }
    }

    //--------------------------PROJ-BEHAVIOUR---------------------
    public static void ProjectileBehaviourBase(BaseProjectile baseProjectile, Collider2D collision)
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
                    Vector3 dir = (colliders[l].transform.position - baseProjectile.transform.position).normalized;

                    baseProjectile.RB.velocity = dir * baseProjectile.ProjSpeed;

                    baseProjectile.transform.rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(new Vector3(0, 0, 90)) * dir);

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

    public static void ProjectileBehaviourWallHit(BaseProjectile baseProjectile, Collider2D collision)
    {
        if (baseProjectile.ChainsLeft > 0)
        {
            baseProjectile.ChainsLeft = (baseProjectile.ChainsLeft == 1) ? 0 : baseProjectile.ChainsLeft / 2;
            Vector2 v = -baseProjectile.RB.velocity.normalized + (Vector2)baseProjectile.transform.position;
            RaycastHit2D hit = Physics2D.Raycast(v, collision.transform.position, 10, WallLayerMask);
            
            if (hit.collider != null)
            {
                Vector2 dir = Vector2.Reflect(baseProjectile.RB.velocity, hit.normal);

                baseProjectile.RB.velocity = dir;

                baseProjectile.transform.rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(new Vector3(0, 0, 90)) * dir);
            }
        }
        else
        {
            GameObject.Destroy(baseProjectile.gameObject);
        }
    }

    public static void ProjectileBehaviourGrenadeWallHit(BaseProjectile baseProjectile, Collider2D collision)
    {
        Vector2 v = -baseProjectile.RB.velocity.normalized + (Vector2)baseProjectile.transform.position;
        RaycastHit2D hit = Physics2D.Raycast(v, collision.transform.position, 10, WallLayerMask);

        if (hit.collider != null)
        {
            Vector2 dir = Vector2.Reflect(baseProjectile.RB.velocity, hit.normal);

            baseProjectile.RB.velocity = dir;

            baseProjectile.transform.rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(new Vector3(0, 0, 90)) * dir);
        }
    }

    public static void ProjectileBehaviourDestroy(BaseProjectile baseProjectile, Collider2D collision)
    {
        GameObject.Destroy(baseProjectile.gameObject);
    }

    public static void ProjectileBehaviourHitBehind(BaseProjectile baseProjectile, Collider2D collision)
    {
        int raysAmount = baseProjectile.ShooterStats.WeaponChainsAmount + baseProjectile.ShooterStats.WeaponPierceAmount;

        if (raysAmount <= 0)
        {
            GameObject.Destroy(baseProjectile.gameObject);
            return;
        }

        Vector2 angle;
        Vector3 direction = (collision.transform.position - baseProjectile.ShooterInitialPosition).normalized;

        for (int i = 0; i < raysAmount; i++)
        {
            Vector3 rotation = new(0, 0, UnityEngine.Random.Range(-20f, 20f)); //secondary bullets angle

            angle = Quaternion.Euler(rotation) * direction;

            RaycastHit2D[] hits = Physics2D.RaycastAll(collision.transform.position, angle, baseProjectile.ShooterStats.CurrentAttackRange * 0.2f, EnemyLayerMask);

            //LineDraw.Instance.DrawLine(collision.transform.position, (Vector2)collision.transform.position + angle * (baseProjectile.ShooterStats.CurrentAttackRange * 0.2f), 0.1f, LineDraw.ShotgunPelletLine, 0.2f);

            for (int j = 0; j < hits.Length && j < 2; j++)
            {
                if (GameObject.ReferenceEquals(hits[j].transform, collision.transform))
                {
                    continue;
                }

                if (hits[j] == true && hits[j].collider.TryGetComponent(out CH_Stats enemyStats))
                {
                    DamageArgs damageArgs = new(baseProjectile.Damage, baseProjectile.IsCritical, baseProjectile.ShooterStats, enemyStats, DamageArgs.DamageSource.Projectile);

                    baseProjectile.ShooterStats.DamageFilter.OutgoingDAMAGE(damageArgs);

                    //Visuals
                    AnimationPlayer.Instance.Play("BloodSmall_04", hits[j].point, Quaternion.identity, Vector3.one, 1.5f);
                }
            }
        }

        GameObject.Destroy(baseProjectile.gameObject);
    }

    public static void ProjectileBehaviourRockedLauncher(BaseProjectile baseProjectile, Collider2D collision)
    {
        if (collision == null || baseProjectile == null) { return; }

        DamageArgs damageArgs = new(baseProjectile.PercentOfDamageToExplosionDamage * baseProjectile.Damage, baseProjectile.IsCritical, baseProjectile.ShooterStats, null, DamageArgs.DamageSource.AOE);

        Effects.ChainOfExplosionsEffect.Start(collision.ClosestPoint(baseProjectile.transform.position),
            baseProjectile.SecondaryExplosionRadius, damageArgs, baseProjectile.ChainsLeft + baseProjectile.PierceLeft + 1,
            0.1f , baseProjectile.LayerMask, "Explosion_02", "BloodSmall_01");

        GameObject.Destroy(baseProjectile.gameObject);
    }

    public static void ProjectileBehaviourGrenadeLauncher(BaseProjectile baseProjectile, Collider2D collision)
    {
        if (collision == null || baseProjectile == null) { return; }

        DamageArgs damageArgs = new(baseProjectile.PercentOfDamageToExplosionDamage * baseProjectile.Damage, baseProjectile.IsCritical, baseProjectile.ShooterStats, null, DamageArgs.DamageSource.AOE);

        Effects.ChainOfExplosionsEffect.Start(collision.ClosestPoint(baseProjectile.transform.position),
            baseProjectile.SecondaryExplosionRadius, damageArgs, baseProjectile.ChainsLeft + baseProjectile.PierceLeft,
            0.1f, baseProjectile.LayerMask, "Explosion_02", "BloodSmall_01");

        GameObject.Destroy(baseProjectile.gameObject);
    }

    public static void ProjectileBehaviourGrenadeLauncherBounce(BaseProjectile baseProjectile, Collider2D _)
    {
        if (baseProjectile == null) { return; }

        var Gp = baseProjectile as GrenadeProjectile;

        if (Gp.PierceLeft > 0)
        {
            Gp.ResetProjectile(Gp.RB.velocity.normalized + (Vector2)Gp.transform.position, 0.2f);
            Gp.PierceLeft--;
            return;
        }
        else if (Gp.ChainsLeft > 0)
        {
            float chainRadius = 2f;
            var colls = Physics2D.OverlapCircleAll(Gp.transform.position, chainRadius, Gp.LayerMask).Where(x => !Gp.Colliders.Contains(x)).ToArray();

            if (colls.Length <= 0)
            {
                GameObject.Destroy(Gp.gameObject);
                return;
            }

            var rnd = UnityEngine.Random.Range(0, colls.Length);
            Gp.ResetProjectile(colls[rnd].transform.position, 0.2f);
            Gp.Colliders.Add(colls[rnd]);
            Gp.ChainsLeft--;
            return;
        }

        GameObject.Destroy(Gp.gameObject);
    }

    //-------------------RELOAD-------------------------
    public static void ReloadSimple(CH_Stats stats)
    {
        stats.ReplanishAmmo();
    }

    public static void ReloadShotgun(CH_Stats stats)
    {
        stats.AmmoChange(1);
    }
}