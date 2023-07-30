using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Armor : EquipmentItem
{
    private int BaseArmor;
    private int BaseMagicResist;
    private float BaseHP;

    

    [field: SerializeField] public int LocalArmor { get; private set; } = 0;
    [field: SerializeField] public int MagicResist { get; private set; } = 0;
    [field: SerializeField] public float HP { get; private set; } = 0;


    public override void Initialize()
    {
        OnEquipAction = () => { return; };
        OnUnEquipAction = () => { return; };

        SignatureMod.Initialize(this);

        ModsHolder = new(EquipmentSlot, this);

        SignatureMod.ApplySignatureMod(this);

        LocalArmor = BaseArmor;
        MagicResist = BaseMagicResist;
        HP = BaseHP;

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
        //подсчет локальных статов после применения модов
        HP = (BaseHP + LSC.DefanceSC.FlatHPValue) * (1 + LSC.DefanceSC.IncreaseHPValue) * LSC.DefanceSC.MoreHPValue * LSC.DefanceSC.LessHPValue;

        LocalArmor = (int)((BaseArmor + LSC.DefanceSC.FlatArmorValue) * (1 + LSC.DefanceSC.IncreaseArmorValue) * LSC.DefanceSC.MoreArmorValue * LSC.DefanceSC.LessArmorValue);

        MagicResist = (int)((BaseMagicResist + LSC.DefanceSC.FlatMagicResistValue) * (1 + LSC.DefanceSC.IncreaseMagicResistValue) * LSC.DefanceSC.MoreMagicResistValue * LSC.DefanceSC.LessMagicResistValue);
    }

    public void ChangeBaseStats(float health, int armor, int resist)
    {
        BaseHP = health;
        BaseArmor = armor;
        BaseMagicResist = resist;
    }
}
