using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MagicBehaviour
{
    public static Dictionary<string, GameObject> ProjectilesGameObjects { get; private set; }
    public static Dictionary<string, GameObject> AbilitiesGameObjects { get; private set; }

    public static GameObject ProjectileParentObject { get; private set; }

    public static LayerMask PlayerLayerMask { get; private set; }
    public static LayerMask EnemyLayerMask { get; private set; }
    public static LayerMask WallLayerMask { get; private set; }
    public const int PlayerProjectileLayer = 8;
    public const int EnemyLayer = 7;

    public const float ProjChainSeekRadius = 6f;

    public const string WallHitAnimation = "Explosion_01";
    public static readonly Vector2 WallHitAnimationScale = new(0.4f, 0.4f);

    static MagicBehaviour()
    {
        PlayerLayerMask = LayerMask.GetMask("Player");
        EnemyLayerMask = LayerMask.GetMask("Enemy");
        WallLayerMask = LayerMask.GetMask("Wall");

        ProjectileParentObject = GameObject.FindGameObjectWithTag("ProjectilesParent");

        var projs = Resources.LoadAll<GameObject>("Projectiles/MagicProjectiles");
        ProjectilesGameObjects = new();
        foreach (GameObject go in projs)
            ProjectilesGameObjects.Add(go.name, go);

        var abilitiesGO = Resources.LoadAll<GameObject>("AbilitiesGameObjects");
        AbilitiesGameObjects = new();
        foreach (var go in abilitiesGO)
            AbilitiesGameObjects.Add(go.name, go);
    }

    //-------------------------------------------------------

    public static Damage UtilityDamageCalculationForArea(AbilitySlot slot, Damage baseDamage, float baseCritChance, out bool isCritical)
    {
        var magicDmg = (baseDamage.Magic + slot.LSC.MagicSC.FlatSpellDamageValue + slot.Stats.GSC.MagicSC.FlatSpellDamageValue) *
            (1 + slot.LSC.GlobalSC.IncreaseMagicDamageValue + slot.Stats.GSC.GlobalSC.IncreaseMagicDamageValue + slot.LSC.MagicSC.IncreaseSpellDamageValue + slot.Stats.GSC.MagicSC.IncreaseSpellDamageValue + slot.LSC.GlobalSC.IncreaseDamageValue + slot.Stats.GSC.GlobalSC.IncreaseDamageValue + slot.LSC.GlobalSC.IncreaseAreaDamageValue + slot.Stats.GSC.GlobalSC.IncreaseAreaDamageValue) * 
            (slot.LSC.GlobalSC.MoreMagicDamageValue * slot.Stats.GSC.GlobalSC.MoreMagicDamageValue * slot.LSC.MagicSC.MoreSpellDamageValue * slot.Stats.GSC.MagicSC.MoreSpellDamageValue * slot.LSC.GlobalSC.MoreDamageValue * slot.Stats.GSC.GlobalSC.MoreDamageValue * slot.LSC.GlobalSC.MoreAreaDamageValue * slot.Stats.GSC.GlobalSC.MoreAreaDamageValue) *
            (slot.LSC.GlobalSC.LessMagicDamageValue * slot.Stats.GSC.GlobalSC.LessMagicDamageValue * slot.LSC.MagicSC.LessSpellDamageValue * slot.Stats.GSC.MagicSC.LessSpellDamageValue * slot.LSC.GlobalSC.LessDamageValue * slot.Stats.GSC.GlobalSC.LessDamageValue * slot.LSC.GlobalSC.LessAreaDamageValue * slot.Stats.GSC.GlobalSC.LessAreaDamageValue);

        var physDmg = (baseDamage.Physical + slot.LSC.MagicSC.FlatSpellDamageValue + slot.Stats.GSC.MagicSC.FlatSpellDamageValue) *
            (1 + slot.LSC.GlobalSC.IncreasePhysicalDamageValue + slot.Stats.GSC.GlobalSC.IncreasePhysicalDamageValue + slot.LSC.MagicSC.IncreaseSpellDamageValue + slot.Stats.GSC.MagicSC.IncreaseSpellDamageValue + slot.LSC.GlobalSC.IncreaseDamageValue + slot.Stats.GSC.GlobalSC.IncreaseDamageValue + slot.LSC.GlobalSC.IncreaseAreaDamageValue + slot.Stats.GSC.GlobalSC.IncreaseAreaDamageValue) *
            (slot.LSC.GlobalSC.MorePhysicalDamageValue * slot.Stats.GSC.GlobalSC.MorePhysicalDamageValue * slot.LSC.MagicSC.MoreSpellDamageValue * slot.Stats.GSC.MagicSC.MoreSpellDamageValue * slot.LSC.GlobalSC.MoreDamageValue * slot.Stats.GSC.GlobalSC.MoreDamageValue * slot.LSC.GlobalSC.MoreAreaDamageValue * slot.Stats.GSC.GlobalSC.MoreAreaDamageValue) *
            (slot.LSC.GlobalSC.LessPhysicalDamageValue * slot.Stats.GSC.GlobalSC.LessPhysicalDamageValue * slot.LSC.MagicSC.LessSpellDamageValue * slot.Stats.GSC.MagicSC.LessSpellDamageValue * slot.LSC.GlobalSC.LessDamageValue * slot.Stats.GSC.GlobalSC.LessDamageValue * slot.LSC.GlobalSC.LessAreaDamageValue * slot.Stats.GSC.GlobalSC.LessAreaDamageValue);

        var critChance = (baseCritChance + slot.LSC.MagicSC.FlatSpellCritChanceValue + slot.Stats.GSC.MagicSC.FlatSpellCritChanceValue) *
            (1 + slot.LSC.MagicSC.IncreaseSpellCritChanceValue + slot.Stats.GSC.MagicSC.IncreaseSpellCritChanceValue) *
            (slot.LSC.MagicSC.MoreSpellCritChanceValue * slot.Stats.GSC.MagicSC.MoreSpellCritChanceValue) *
            (slot.LSC.MagicSC.LessSpellCritChanceValue * slot.Stats.GSC.MagicSC.LessSpellCritChanceValue);

        var critMulti = (slot.Stats.BaseSpellCritMultiplier + slot.LSC.GlobalSC.FlatCritMultiplierValue + slot.Stats.GSC.GlobalSC.FlatCritMultiplierValue + slot.LSC.MagicSC.FlatSpellCritMultiplierValue + slot.Stats.GSC.MagicSC.FlatSpellCritMultiplierValue) *
            (1 + slot.LSC.GlobalSC.IncreaseCritMultiplierValue + slot.Stats.GSC.GlobalSC.IncreaseCritMultiplierValue + slot.LSC.MagicSC.IncreaseSpellCritMultiplierValue + slot.Stats.GSC.MagicSC.IncreaseSpellCritMultiplierValue) *
            (slot.LSC.GlobalSC.MoreCritMultiplierValue * slot.Stats.GSC.GlobalSC.MoreCritMultiplierValue * slot.LSC.MagicSC.MoreSpellCritMultiplierValue * slot.Stats.GSC.MagicSC.MoreSpellCritMultiplierValue) *
            (slot.LSC.GlobalSC.LessCritMultiplierValue * slot.Stats.GSC.GlobalSC.LessCritMultiplierValue * slot.LSC.MagicSC.LessSpellCritMultiplierValue * slot.Stats.GSC.MagicSC.LessSpellCritMultiplierValue);

        critMulti /= 100;

        if (critChance >= UnityEngine.Random.Range(0.01f, 100f))
        {
            isCritical = true;
            return new Damage(physDmg * critMulti, magicDmg * critMulti, baseDamage.True, baseDamage.PercentPhysical, baseDamage.PercentMagic, baseDamage.PercentTrue);
        }
        else
        {
            isCritical = false;
            return new Damage(physDmg, magicDmg, baseDamage.True, baseDamage.PercentPhysical, baseDamage.PercentMagic, baseDamage.PercentTrue);
        }
    }

    public static Damage UtilityDamageCalculationForAreaNONCRIT(AbilitySlot slot, Damage baseDamage)
    {
        var magicDmg = (baseDamage.Magic + slot.LSC.MagicSC.FlatSpellDamageValue + slot.Stats.GSC.MagicSC.FlatSpellDamageValue) *
            (1 + slot.LSC.GlobalSC.IncreaseMagicDamageValue + slot.Stats.GSC.GlobalSC.IncreaseMagicDamageValue + slot.LSC.MagicSC.IncreaseSpellDamageValue + slot.Stats.GSC.MagicSC.IncreaseSpellDamageValue + slot.LSC.GlobalSC.IncreaseDamageValue + slot.Stats.GSC.GlobalSC.IncreaseDamageValue + slot.LSC.GlobalSC.IncreaseAreaDamageValue + slot.Stats.GSC.GlobalSC.IncreaseAreaDamageValue) *
            (slot.LSC.GlobalSC.MoreMagicDamageValue * slot.Stats.GSC.GlobalSC.MoreMagicDamageValue * slot.LSC.MagicSC.MoreSpellDamageValue * slot.Stats.GSC.MagicSC.MoreSpellDamageValue * slot.LSC.GlobalSC.MoreDamageValue * slot.Stats.GSC.GlobalSC.MoreDamageValue * slot.LSC.GlobalSC.MoreAreaDamageValue * slot.Stats.GSC.GlobalSC.MoreAreaDamageValue) *
            (slot.LSC.GlobalSC.LessMagicDamageValue * slot.Stats.GSC.GlobalSC.LessMagicDamageValue * slot.LSC.MagicSC.LessSpellDamageValue * slot.Stats.GSC.MagicSC.LessSpellDamageValue * slot.LSC.GlobalSC.LessDamageValue * slot.Stats.GSC.GlobalSC.LessDamageValue * slot.LSC.GlobalSC.LessAreaDamageValue * slot.Stats.GSC.GlobalSC.LessAreaDamageValue);

        var physDmg = (baseDamage.Physical + slot.LSC.MagicSC.FlatSpellDamageValue + slot.Stats.GSC.MagicSC.FlatSpellDamageValue) *
            (1 + slot.LSC.GlobalSC.IncreasePhysicalDamageValue + slot.Stats.GSC.GlobalSC.IncreasePhysicalDamageValue + slot.LSC.MagicSC.IncreaseSpellDamageValue + slot.Stats.GSC.MagicSC.IncreaseSpellDamageValue + slot.LSC.GlobalSC.IncreaseDamageValue + slot.Stats.GSC.GlobalSC.IncreaseDamageValue + slot.LSC.GlobalSC.IncreaseAreaDamageValue + slot.Stats.GSC.GlobalSC.IncreaseAreaDamageValue) *
            (slot.LSC.GlobalSC.MorePhysicalDamageValue * slot.Stats.GSC.GlobalSC.MorePhysicalDamageValue * slot.LSC.MagicSC.MoreSpellDamageValue * slot.Stats.GSC.MagicSC.MoreSpellDamageValue * slot.LSC.GlobalSC.MoreDamageValue * slot.Stats.GSC.GlobalSC.MoreDamageValue * slot.LSC.GlobalSC.MoreAreaDamageValue * slot.Stats.GSC.GlobalSC.MoreAreaDamageValue) *
            (slot.LSC.GlobalSC.LessPhysicalDamageValue * slot.Stats.GSC.GlobalSC.LessPhysicalDamageValue * slot.LSC.MagicSC.LessSpellDamageValue * slot.Stats.GSC.MagicSC.LessSpellDamageValue * slot.LSC.GlobalSC.LessDamageValue * slot.Stats.GSC.GlobalSC.LessDamageValue * slot.LSC.GlobalSC.LessAreaDamageValue * slot.Stats.GSC.GlobalSC.LessAreaDamageValue);

        return new Damage(physDmg, magicDmg, baseDamage.True, baseDamage.PercentPhysical, baseDamage.PercentMagic, baseDamage.PercentTrue);
    }

    public static Damage UtilityDamageCalculationForProjectiles(AbilitySlot slot, Damage baseDamage, float baseCritChance, out bool isCritical)
    {
        var magicDmg = (baseDamage.Magic + slot.LSC.MagicSC.FlatSpellDamageValue + slot.Stats.GSC.MagicSC.FlatSpellDamageValue) *
            (1 + slot.LSC.GlobalSC.IncreaseMagicDamageValue + slot.Stats.GSC.GlobalSC.IncreaseMagicDamageValue + slot.LSC.MagicSC.IncreaseSpellDamageValue + slot.Stats.GSC.MagicSC.IncreaseSpellDamageValue + slot.LSC.GlobalSC.IncreaseDamageValue + slot.Stats.GSC.GlobalSC.IncreaseDamageValue) *
            (slot.LSC.GlobalSC.MoreMagicDamageValue * slot.Stats.GSC.GlobalSC.MoreMagicDamageValue * slot.LSC.MagicSC.MoreSpellDamageValue * slot.Stats.GSC.MagicSC.MoreSpellDamageValue * slot.LSC.GlobalSC.MoreDamageValue * slot.Stats.GSC.GlobalSC.MoreDamageValue) *
            (slot.LSC.GlobalSC.LessMagicDamageValue * slot.Stats.GSC.GlobalSC.LessMagicDamageValue * slot.LSC.MagicSC.LessSpellDamageValue * slot.Stats.GSC.MagicSC.LessSpellDamageValue * slot.LSC.GlobalSC.LessDamageValue * slot.Stats.GSC.GlobalSC.LessDamageValue);

        var physDmg = (baseDamage.Physical + slot.LSC.MagicSC.FlatSpellDamageValue + slot.Stats.GSC.MagicSC.FlatSpellDamageValue) *
            (1 + slot.LSC.GlobalSC.IncreasePhysicalDamageValue + slot.Stats.GSC.GlobalSC.IncreasePhysicalDamageValue + slot.LSC.MagicSC.IncreaseSpellDamageValue + slot.Stats.GSC.MagicSC.IncreaseSpellDamageValue + slot.LSC.GlobalSC.IncreaseDamageValue + slot.Stats.GSC.GlobalSC.IncreaseDamageValue) *
            (slot.LSC.GlobalSC.MorePhysicalDamageValue * slot.Stats.GSC.GlobalSC.MorePhysicalDamageValue * slot.LSC.MagicSC.MoreSpellDamageValue * slot.Stats.GSC.MagicSC.MoreSpellDamageValue * slot.LSC.GlobalSC.MoreDamageValue * slot.Stats.GSC.GlobalSC.MoreDamageValue) *
            (slot.LSC.GlobalSC.LessPhysicalDamageValue * slot.Stats.GSC.GlobalSC.LessPhysicalDamageValue * slot.LSC.MagicSC.LessSpellDamageValue * slot.Stats.GSC.MagicSC.LessSpellDamageValue * slot.LSC.GlobalSC.LessDamageValue * slot.Stats.GSC.GlobalSC.LessDamageValue);

        var critChance = (baseCritChance + slot.LSC.MagicSC.FlatSpellCritChanceValue + slot.Stats.GSC.MagicSC.FlatSpellCritChanceValue) *
            (1 + slot.LSC.MagicSC.IncreaseSpellCritChanceValue + slot.Stats.GSC.MagicSC.IncreaseSpellCritChanceValue) *
            (slot.LSC.MagicSC.MoreSpellCritChanceValue * slot.Stats.GSC.MagicSC.MoreSpellCritChanceValue) *
            (slot.LSC.MagicSC.LessSpellCritChanceValue * slot.Stats.GSC.MagicSC.LessSpellCritChanceValue);

        var critMulti = (slot.Stats.BaseSpellCritMultiplier + slot.LSC.GlobalSC.FlatCritMultiplierValue + slot.Stats.GSC.GlobalSC.FlatCritMultiplierValue + slot.LSC.MagicSC.FlatSpellCritMultiplierValue + slot.Stats.GSC.MagicSC.FlatSpellCritMultiplierValue) *
            (1 + slot.LSC.GlobalSC.IncreaseCritMultiplierValue + slot.Stats.GSC.GlobalSC.IncreaseCritMultiplierValue + slot.LSC.MagicSC.IncreaseSpellCritMultiplierValue + slot.Stats.GSC.MagicSC.IncreaseSpellCritMultiplierValue) *
            (slot.LSC.GlobalSC.MoreCritMultiplierValue * slot.Stats.GSC.GlobalSC.MoreCritMultiplierValue * slot.LSC.MagicSC.MoreSpellCritMultiplierValue * slot.Stats.GSC.MagicSC.MoreSpellCritMultiplierValue) *
            (slot.LSC.GlobalSC.LessCritMultiplierValue * slot.Stats.GSC.GlobalSC.LessCritMultiplierValue * slot.LSC.MagicSC.LessSpellCritMultiplierValue * slot.Stats.GSC.MagicSC.LessSpellCritMultiplierValue);

        critMulti /= 100;

        if (critChance >= UnityEngine.Random.Range(0.01f, 100f))
        {
            isCritical = true;
            return new Damage(physDmg * critMulti, magicDmg * critMulti, baseDamage.True, baseDamage.PercentPhysical, baseDamage.PercentMagic, baseDamage.PercentTrue);
        }
        else
        {
            isCritical = false;
            return new Damage(physDmg, magicDmg, baseDamage.True, baseDamage.PercentPhysical, baseDamage.PercentMagic, baseDamage.PercentTrue);
        }
    }

    public static Damage UtilityDamageCalculationForProjectilesNONCRIT(AbilitySlot slot, Damage baseDamage)
    {
        var magicDmg = (baseDamage.Magic + slot.LSC.MagicSC.FlatSpellDamageValue + slot.Stats.GSC.MagicSC.FlatSpellDamageValue) *
            (1 + slot.LSC.GlobalSC.IncreaseMagicDamageValue + slot.Stats.GSC.GlobalSC.IncreaseMagicDamageValue + slot.LSC.MagicSC.IncreaseSpellDamageValue + slot.Stats.GSC.MagicSC.IncreaseSpellDamageValue + slot.LSC.GlobalSC.IncreaseDamageValue + slot.Stats.GSC.GlobalSC.IncreaseDamageValue) *
            (slot.LSC.GlobalSC.MoreMagicDamageValue * slot.Stats.GSC.GlobalSC.MoreMagicDamageValue * slot.LSC.MagicSC.MoreSpellDamageValue * slot.Stats.GSC.MagicSC.MoreSpellDamageValue * slot.LSC.GlobalSC.MoreDamageValue * slot.Stats.GSC.GlobalSC.MoreDamageValue) *
            (slot.LSC.GlobalSC.LessMagicDamageValue * slot.Stats.GSC.GlobalSC.LessMagicDamageValue * slot.LSC.MagicSC.LessSpellDamageValue * slot.Stats.GSC.MagicSC.LessSpellDamageValue * slot.LSC.GlobalSC.LessDamageValue * slot.Stats.GSC.GlobalSC.LessDamageValue);

        var physDmg = (baseDamage.Physical + slot.LSC.MagicSC.FlatSpellDamageValue + slot.Stats.GSC.MagicSC.FlatSpellDamageValue) *
            (1 + slot.LSC.GlobalSC.IncreasePhysicalDamageValue + slot.Stats.GSC.GlobalSC.IncreasePhysicalDamageValue + slot.LSC.MagicSC.IncreaseSpellDamageValue + slot.Stats.GSC.MagicSC.IncreaseSpellDamageValue + slot.LSC.GlobalSC.IncreaseDamageValue + slot.Stats.GSC.GlobalSC.IncreaseDamageValue) *
            (slot.LSC.GlobalSC.MorePhysicalDamageValue * slot.Stats.GSC.GlobalSC.MorePhysicalDamageValue * slot.LSC.MagicSC.MoreSpellDamageValue * slot.Stats.GSC.MagicSC.MoreSpellDamageValue * slot.LSC.GlobalSC.MoreDamageValue * slot.Stats.GSC.GlobalSC.MoreDamageValue) *
            (slot.LSC.GlobalSC.LessPhysicalDamageValue * slot.Stats.GSC.GlobalSC.LessPhysicalDamageValue * slot.LSC.MagicSC.LessSpellDamageValue * slot.Stats.GSC.MagicSC.LessSpellDamageValue * slot.LSC.GlobalSC.LessDamageValue * slot.Stats.GSC.GlobalSC.LessDamageValue);
                
        return new Damage(physDmg, magicDmg, baseDamage.True, baseDamage.PercentPhysical, baseDamage.PercentMagic, baseDamage.PercentTrue);
    }

    public static Damage UtilityDamageOverTimeCalculation(AbilitySlot slot, Damage modifiedDamagePerSec)
    {
        var mDmg = modifiedDamagePerSec.Magic * (1 + slot.LSC.GlobalSC.FlatMagicDOTMultiValue + slot.Stats.GSC.GlobalSC.FlatMagicDOTMultiValue);

        var pDmg = modifiedDamagePerSec.Physical * (1 + slot.LSC.GlobalSC.FlatPhysDOTMultiValue + slot.Stats.GSC.GlobalSC.FlatPhysDOTMultiValue);

        return new Damage(pDmg, mDmg, modifiedDamagePerSec.True, modifiedDamagePerSec.PercentPhysical, modifiedDamagePerSec.PercentMagic, modifiedDamagePerSec.PercentTrue);
    }

    public static Damage UtilityAverageDamageCalculation(AbilitySlot slot, Damage modifiedDamage, float baseCritChance)
    {
        var critChance = (baseCritChance + slot.LSC.MagicSC.FlatSpellCritChanceValue + slot.Stats.GSC.MagicSC.FlatSpellCritChanceValue) *
            (1 + slot.LSC.MagicSC.IncreaseSpellCritChanceValue + slot.Stats.GSC.MagicSC.IncreaseSpellCritChanceValue) *
            (slot.LSC.MagicSC.MoreSpellCritChanceValue * slot.Stats.GSC.MagicSC.MoreSpellCritChanceValue) *
            (slot.LSC.MagicSC.LessSpellCritChanceValue * slot.Stats.GSC.MagicSC.LessSpellCritChanceValue);

        var critMulti = (slot.Stats.BaseSpellCritMultiplier + slot.LSC.GlobalSC.FlatCritMultiplierValue + slot.Stats.GSC.GlobalSC.FlatCritMultiplierValue + slot.LSC.MagicSC.FlatSpellCritMultiplierValue + slot.Stats.GSC.MagicSC.FlatSpellCritMultiplierValue) *
            (1 + slot.LSC.GlobalSC.IncreaseCritMultiplierValue + slot.Stats.GSC.GlobalSC.IncreaseCritMultiplierValue + slot.LSC.MagicSC.IncreaseSpellCritMultiplierValue + slot.Stats.GSC.MagicSC.IncreaseSpellCritMultiplierValue) *
            (slot.LSC.GlobalSC.MoreCritMultiplierValue * slot.Stats.GSC.GlobalSC.MoreCritMultiplierValue * slot.LSC.MagicSC.MoreSpellCritMultiplierValue * slot.Stats.GSC.MagicSC.MoreSpellCritMultiplierValue) *
            (slot.LSC.GlobalSC.LessCritMultiplierValue * slot.Stats.GSC.GlobalSC.LessCritMultiplierValue * slot.LSC.MagicSC.LessSpellCritMultiplierValue * slot.Stats.GSC.MagicSC.LessSpellCritMultiplierValue);

        critMulti /= 100;

        return modifiedDamage * (1 - critChance / 100) + (modifiedDamage * (critChance / 100) * critMulti);
    }

    //-------------------------------------------------------
    public static void SpwanSimpleMagicShot(CH_Stats stats, AbilitySlot slot, ProjectileCreationArgs projectileArgs, Vector2 aim, Action<BaseProjectile, Collider2D> projBehavior)
    {
        Rigidbody2D rigidbody2D;
        BaseProjectile baseProjectile;
        bool isCritical;

        GameObject go = GameObject.Instantiate(projectileArgs.Projectile, stats.Equipment.Player.transform.position,
            Quaternion.identity, stats.Equipment.ProjectileParentObject.transform);

        WeaponBehaviour.IndexOfProjectiles++;

        rigidbody2D = go.GetComponent<Rigidbody2D>();
        baseProjectile = go.GetComponent<BaseProjectile>();

        go.layer = PlayerProjectileLayer;

        rigidbody2D.velocity = (aim - (Vector2)stats.transform.position).normalized * projectileArgs.Speed;

        baseProjectile.ShooterStats = stats;

        baseProjectile.ProjectileBehavior = projBehavior;
        baseProjectile.ChainsLeft = 0;
        baseProjectile.PierceLeft = 0;

        baseProjectile.ProjDestroyTime = Time.time + projectileArgs.Range / projectileArgs.Speed;
        baseProjectile.ProjSpeed = projectileArgs.Speed;
        baseProjectile.ProjIndex = WeaponBehaviour.IndexOfProjectiles;
        baseProjectile.IsMultipleProjWork = false;
        baseProjectile.LayerMask = EnemyLayerMask;

        baseProjectile.Damage = UtilityDamageCalculationForProjectiles(slot, projectileArgs.Damage, projectileArgs.BaseCritChance, out isCritical);
        baseProjectile.IsCritical = isCritical;
    }
    //-------------------------------------------------------

    public static void ProjectileBegaviourMagic(BaseProjectile baseProjectile, Collider2D collision)
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
}

public struct ProjectileCreationArgs
{
    public GameObject Projectile { get; }
    public int ProjectilesAmount { get; }
    public int PierceAmount { get; }
    public int ChainsAmount { get; }
    public Damage Damage { get; }
    public float BaseCritChance { get; }
    public float Speed { get; }
    public float Range { get; }

    public ProjectileCreationArgs(GameObject projectile, Damage baseDamage, float baseCritChance, float baseSpeed, float baseRange, int baseProjectilesAmount = 1, int basePierceAmount = 0, int baseChainsAmount = 0)
    {
        Projectile = projectile;
        ProjectilesAmount = baseProjectilesAmount;
        PierceAmount = basePierceAmount;
        ChainsAmount = baseChainsAmount;
        Damage = baseDamage;
        BaseCritChance = baseCritChance;
        Speed = baseSpeed;
        Range = baseRange;
    }
}