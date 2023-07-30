using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

public class StatsChanges
{
    public event Action OnStatsChange;

    public readonly GlobalSC GlobalSC = new();
    public readonly AttackSC AttackSC = new();
    public readonly DefanceSC DefanceSC = new();
    public readonly MagicSC MagicSC = new();
    public readonly UtilitySC UtilitySC = new();
    public StatsChanges()
    {
        GlobalSC.OnStatsChange += OnSubStatsChanges;
        AttackSC.OnStatsChange += OnSubStatsChanges;
        DefanceSC.OnStatsChange += OnSubStatsChanges;
        MagicSC.OnStatsChange += OnSubStatsChanges;
        UtilitySC.OnStatsChange += OnSubStatsChanges;
    }

    public void SwapChanges(StatsChanges changes)
    {
        GlobalSC.SwapChanges(changes.GlobalSC);
        AttackSC.SwapChanges(changes.AttackSC);
        DefanceSC.SwapChanges(changes.DefanceSC);
        MagicSC.SwapChanges(changes.MagicSC);
        UtilitySC.SwapChanges(changes.UtilitySC);

        OnStatsChange?.Invoke();
    }

    public void CombineChanges(StatsChanges changes)
    {
        GlobalSC.CombineChanges(changes.GlobalSC);
        AttackSC.CombineChanges(changes.AttackSC);
        DefanceSC.CombineChanges(changes.DefanceSC);
        MagicSC.CombineChanges(changes.MagicSC);
        UtilitySC.CombineChanges(changes.UtilitySC);

        OnStatsChange?.Invoke();
    }

    public void RemoveChanges(StatsChanges changes)
    {
        GlobalSC.RemoveChanges(changes.GlobalSC);
        AttackSC.RemoveChanges(changes.AttackSC);
        DefanceSC.RemoveChanges(changes.DefanceSC);
        MagicSC.RemoveChanges(changes.MagicSC);
        UtilitySC.RemoveChanges(changes.UtilitySC);

        OnStatsChange?.Invoke();
    }

    private void OnSubStatsChanges()
    {
        OnStatsChange?.Invoke();
    }
}







//public int FlatValue { get; private set; }
//public float IncreaseValue { get; private set; }
//public float MoreValue { get; private set; } = 1f;
//public float LessValue { get; private set; } = 1f;
//public void AddFlat(int value)
//{
//  FlatValue += value;
//  OnStatsChange?.Invoke();
//}
//public void Increase(float percent)
//{
//  IncreaseValue += percent / 100;
//  OnStatsChange?.Invoke();
//}
//public void More(float percent)
//{
//    if (percent > 0f)
//        MoreValue *= 1 + percent / 100;
//    else
//        MoreValue /= 1 - percent / 100;
//
//  OnStatsChange?.Invoke();
//}
//public void Less(float percent)
//{
//    if (percent > 0f)
//        LessValue *= 1 - percent / 100;
//    else
//        LessValue /= 1 + percent / 100;
//
//      OnStatsChange?.Invoke();
//}