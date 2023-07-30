using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Weapon : EquipmentItem, IWeaponItem
{
    [SerializeField] private int BaseMinDamage;
    [SerializeField] private int BaseMaxDamage;
    [SerializeField] private float BaseAttackSpeed;
    [SerializeField] private float BaseCritChance;
    [SerializeField] private float BaseCritMultiplierPercent;
    [SerializeField] private float BaseReloadSpeed;
    [SerializeField] private int BaseAmmoCapacity;
    [SerializeField] private float BaseAccuracy;
    [SerializeField] private float BaseSpreadAngle;
    [SerializeField] private int BaseChainsAmount;
    [SerializeField] private int BasePierceAmount;
    [SerializeField] private int BaseProjectileAmount;
    [SerializeField] private float BaseRange;
    [SerializeField] protected float BaseProjectileSpeed;

    

    public int MinDamage { get; private set; } = 0;
    public int MaxDamage { get; private set; } = 0;
    public float AttackSpeed { get; private set; } = 0;
    public float CritChance { get; private set; } = 0;
    public float CritMultiplier { get; private set; } = 0;    
    public float ReloadSpeed { get; private set; } = 0;
    public int AmmoCapacity { get; private set; } = 1;
    public float Accuracy { get; private set; } = 0;
    public float SpreadAngle { get; private set; } = 1;
    public int ChainsAmount { get; private set; } = 0;
    public int PierceAmount { get; private set; } = 0;
    public int ProjectileAmount { get; private set; } = 0;
    public float Range { get; private set; } = 1;
    public float ProjectileSpeed { get; private set; } = 1;
    [HideInInspector] public bool IsReloading { get; set; } = false;

    public Action<Equipment, Action<BaseProjectile, Collider2D>> ProjectileSpawnBehavior;
    public Action<BaseProjectile, Collider2D> ProjectileBehavior;
    public Action<CH_Stats> ReloadBehavior;

    protected float attackCD;

    public int CurrentAmmo { get; set; } = 0;
    
    public void AddBaseDamage(int damage)
    {
        BaseMinDamage += damage;
        BaseMaxDamage += damage;
    }

    public override void Initialize()
    {
        attackCD = 0f;

        OnEquipAction = () => { return; }; //Changes in signature mods if needed
        OnUnEquipAction = () => { return; }; //Changes in signature mods if needed

        SignatureMod.Initialize(this);

        ModsHolder = new(EquipmentSlot, this);

        SignatureMod.ApplySignatureMod(this); //в родительском классе


        MinDamage = BaseMinDamage;
        MaxDamage = BaseMaxDamage;
        AttackSpeed = BaseAttackSpeed;
        CritChance = BaseCritChance;
        CritMultiplier = BaseCritMultiplierPercent;
        ReloadSpeed = BaseReloadSpeed;
        AmmoCapacity = BaseAmmoCapacity;
        Accuracy = BaseAccuracy;
        SpreadAngle = BaseSpreadAngle;
        Range = BaseRange;
        ProjectileSpeed = BaseProjectileSpeed;
        ChainsAmount = BaseChainsAmount;
        PierceAmount = BasePierceAmount;
        ProjectileAmount = BaseProjectileAmount;

        ModsHolder.GenerateInitialMods();

        EvaluateLocalStats();

        LSC.OnStatsChange += EvaluateLocalStats;
    }

    private void OnDestroy()
    {
        LSC.OnStatsChange -= EvaluateLocalStats;
    }

    public override void EvaluateLocalStats()
    {
        //---AA_DAMAGE--------------------------
        MinDamage = (int)((BaseMinDamage + LSC.AttackSC.FlatAttackDamageValue) *
            (1 + LSC.AttackSC.IncreaseAttackDamageValue) * LSC.AttackSC.MoreAttackDamageValue * LSC.AttackSC.LessAttackDamageValue);

        MaxDamage = (int)((BaseMaxDamage + LSC.AttackSC.FlatAttackDamageValue) *
            (1 + LSC.AttackSC.IncreaseAttackDamageValue) * LSC.AttackSC.MoreAttackDamageValue * LSC.AttackSC.LessAttackDamageValue);
        //---AA_DAMAGE_END----------------------


        CritChance = (BaseCritChance + LSC.AttackSC.FlatAttackCritChanceValue) * (1 + LSC.AttackSC.IncreaseAttackCritChanceValue) * LSC.AttackSC.MoreAttackCritChanceValue * LSC.AttackSC.LessAttackCritChanceValue;

        CritMultiplier = (BaseCritMultiplierPercent + LSC.AttackSC.FlatAttackCritMultiplierValue) * (1 + LSC.AttackSC.IncreaseAttackCritMultiplierValue) * LSC.AttackSC.MoreAttackCritMultiplierValue * LSC.AttackSC.LessAttackCritMultiplierValue;

        Accuracy = (BaseAccuracy + LSC.AttackSC.FlatAccuracyValue) * (1 + LSC.AttackSC.IncreaseAccuracyValue) * LSC.AttackSC.MoreAccuracyValue * LSC.AttackSC.LessAccuracyValue;

        SpreadAngle = BaseSpreadAngle * (1 + LSC.AttackSC.IncreaseSpreadAngleValue) * LSC.AttackSC.MoreSpreadAngleValue * LSC.AttackSC.LessSpreadAngleValue;

        AttackSpeed = BaseAttackSpeed * (1 + LSC.AttackSC.IncreaseAttackSpeedValue) * LSC.AttackSC.MoreAttackSpeedValue * LSC.AttackSC.LessAttackSpeedValue;

        ReloadSpeed = BaseReloadSpeed * (1 + LSC.AttackSC.IncreaseReloadSpeedValue) * LSC.AttackSC.MoreReloadSpeedValue * LSC.AttackSC.LessReloadSpeedValue;

        AmmoCapacity = (int)((BaseAmmoCapacity + LSC.AttackSC.FlatAmmoCapacityValue) * (1 + LSC.AttackSC.IncreaseAmmoCapacityValue) * LSC.AttackSC.MoreAmmoCapacityValue * LSC.AttackSC.LessAmmoCapacityValue);

        Range = BaseRange * (1 + LSC.AttackSC.IncreaseAttackRangeValue) * LSC.AttackSC.MoreAttackRangeValue * LSC.AttackSC.LessAttackRangeValue;

        ProjectileSpeed = (BaseProjectileSpeed + LSC.UtilitySC.FlatProjectileSpeedValue) * (1 + LSC.UtilitySC.IncreaseProjectileSpeedValue) * LSC.UtilitySC.MoreProjectileSpeedValue * LSC.UtilitySC.LessProjectileSpeedValue;

        ProjectileAmount = BaseProjectileAmount + LSC.AttackSC.FlatWeaponProjectileAmountValue;

        ChainsAmount = BaseChainsAmount + LSC.AttackSC.FlatWeaponChainsAmountValue;

        PierceAmount = BasePierceAmount + LSC.AttackSC.FlatWeaponPierceAmountValue;
    }

    public virtual void Shoot(Equipment equipment)
    {
        if (IsReloading) { return; }

        if (equipment.Stats.CurrentAmmo <= 0) { Reload(equipment); }

        if (attackCD > Time.time) { return; }

        attackCD = Time.time + 1 / equipment.Stats.CurrentAttackSpeed;

        ProjectileSpawnBehavior(equipment, ProjectileBehavior);

        equipment.WeaponShot();

        //equipment.Stats.cH_Animation.PlayAttack(equipment.Stats.cH_Animation.AttackClipLengh);
    }

    public virtual void ShootOnce(Equipment equipment) { return; }

    public virtual void OnShootButtonUp(Equipment equipment) { return; }

    public virtual void Reload(Equipment equipment)
    {
        StartCoroutine(ReloadCoroutine(equipment));
    }

    public virtual void StopReloadRoutine()
    {
        StopAllCoroutines();
        IsReloading = false;
    }

    public virtual void OnEquipmentChange() { return; }

    private IEnumerator ReloadCoroutine(Equipment equipment)
    {
        IsReloading = true;

        yield return new WaitForSeconds(1 / equipment.Stats.CurrentReloadSpeed);

        ReloadBehavior(equipment.Stats);
        
        IsReloading = false;
    }

}
