using System;
using UnityEngine;

public class Book : LeftHand
{
    public override void Initialize()
    {
        OnEquipAction = () => { return; };
        OnUnEquipAction = () => { return; };

        SignatureMod.Initialize(this);

        ModsHolder = new(EquipmentSlot, this);

        SignatureMod.ApplySignatureMod(this);

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
    }
}
