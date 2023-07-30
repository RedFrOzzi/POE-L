using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingWeapon : Weapon
{
    [SerializeField] private float BaseChargeRate;
    [SerializeField] private float BaseMaxCharge;
    [SerializeField] private float BaseChargeDamageModifier;

    public float ChargeRate { get; private set; }
    public float MaxCharge { get; private set; }
    public float ChargeDamageModifier { get; private set; }

    [field: SerializeField] public float Charge { get; private set; }

    private const float initialShotChargeThreshold = 1.5f;


    public override void Initialize()
    {
        Charge = 1f;

        base.Initialize();
    }

    public override void EvaluateLocalStats()
    {
        base.EvaluateLocalStats();

        ChargeRate = (BaseChargeRate + LSC.AttackSC.FlatChargeRateValue) * (1 + LSC.AttackSC.IncreaseChargeRateValue) * LSC.AttackSC.MoreChargeRateValue * LSC.AttackSC.LessChargeRateValue;

        MaxCharge = (BaseMaxCharge + LSC.AttackSC.FlatMaxChargeValue) * (1 + LSC.AttackSC.IncreaseMaxChargeValue) * LSC.AttackSC.MoreMaxChargeValue * LSC.AttackSC.LessMaxChargeValue;

        ChargeDamageModifier = (BaseChargeDamageModifier + LSC.AttackSC.FlatChargeDamageModifierValue) * (1 + LSC.AttackSC.IncreaseChargeDamageModifierValue) * LSC.AttackSC.MoreChargeDamageModifierValue * LSC.AttackSC.LessChargeDamageModifierValue;
    }

    public override void Shoot(Equipment equipment)
    {
        if (IsReloading || equipment.Stats.CurrentAmmo <= 0) { return; }

        Charge = Mathf.Clamp(Charge + ChargeRate * Time.deltaTime * equipment.Stats.CurrentAttackSpeed, 1f, MaxCharge);
    }

    public override void OnShootButtonUp(Equipment equipment)
    {
        if (Charge < initialShotChargeThreshold) { return; }

        ProjectileSpawnBehavior(equipment, ProjectileBehavior);

        equipment.WeaponShot();

        Charge = 1f;
    }

    public override void OnEquipmentChange()
    {
        Charge = 1f;
    }
}
