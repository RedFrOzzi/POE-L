
using System.Collections.Generic;
using Database;

public struct Damage
{
    public Damage(float phisical, float magic, float trueD, float percentPhis, float percentMagic, float percentTrue)
    {
        Physical = phisical;
        Magic = magic;
        True = trueD;
        PercentPhysical = percentPhis;
        PercentMagic = percentMagic;
        PercentTrue = percentTrue;
    }

    public Damage(float physical, float magic, float trueD)
    {
        Physical = physical;
        Magic = magic;
        True = trueD;
        PercentPhysical = 0;
        PercentMagic = 0;
        PercentTrue = 0;
    }

    /// <summary>
    /// Amaunt of Phisical damage
    /// </summary>
    public float Physical { get; private set; }

    /// <summary>
    /// Amaunt of magic damage
    /// </summary>
    public float Magic { get; private set; }

    /// <summary>
    /// Amaunt of true damage
    /// </summary>
    public float True { get; private set; }

    /// <summary>
    /// Amaunt of persent Phisical damage
    /// </summary>
    public float PercentPhysical { get; private set; }

    /// <summary>
    /// Amaunt of persent magic damage
    /// </summary>
    public float PercentMagic { get; private set; }

    /// <summary>
    /// Amaunt of persent true damage
    /// </summary>
    public float PercentTrue { get; private set; }

    public float CombinedDamage()
    {
        return Physical + Magic + True + PercentPhysical + PercentMagic + PercentTrue;
    }

    public float CombinedPhisicalDamage()
    {
        return Physical + PercentPhysical;
    }

    public float CombinedMagicDamage()
    {
        return Magic + PercentMagic;
    }

    public float CombinedTrueDamage()
    {
        return True + PercentTrue;
    }

    public static Damage operator * (Damage d, float f)
    {
        return new Damage(d.Physical * f, d.Magic * f, d.True * f, d.PercentPhysical * f, d.PercentMagic * f, d.PercentTrue * f);
    }
    public static Damage operator * (float f, Damage d)
    {
        return new Damage(d.Physical * f, d.Magic * f, d.True * f, d.PercentPhysical * f, d.PercentMagic * f, d.PercentTrue * f);
    }
    public static Damage operator / (Damage d, float f)
    {
        return new Damage(d.Physical / f, d.Magic / f, d.True / f, d.PercentPhysical / f, d.PercentMagic / f, d.PercentTrue / f);
    }
    public static Damage operator + (Damage d1, Damage d2)
    {
        return new Damage(d1.Physical + d2.Physical, d1.Magic + d2.Magic, d1.True + d2.True, d1.PercentPhysical + d2.PercentPhysical, d1.PercentMagic + d2.PercentMagic, d1.PercentTrue + d2.PercentTrue);
    }
}

public struct DamageArgs
{
    public DamageArgs(Damage damage, bool isCritical, CH_Stats shooterStats, CH_Stats enemyStats, DamageSource damageSource)
    {
        Damage = damage;
        IsCritical = isCritical;
        ShooterStats = shooterStats;
        EnemyStats = enemyStats;
        SourceOfDamage = damageSource;
    }

    public Damage Damage { get; set; }
    public bool IsCritical { get; set; }
    public CH_Stats ShooterStats { get; set; }
    public CH_Stats EnemyStats { get; set; }
    public DamageSource SourceOfDamage { get; set; }

    public enum DamageSource
    {
        MeleeHit, Projectile, AOE, SingleTarget, DOT
    }
}
