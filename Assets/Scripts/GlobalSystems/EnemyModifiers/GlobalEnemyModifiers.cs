using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class GlobalEnemyModifiers : MonoBehaviour
{
    public static GlobalEnemyModifiers Instance;

    public StatsChanges ESC { get; private set; } = new();

    private readonly List<CH_Stats> aliveEnemiesStats = new();


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }


    private void Start()
    {
        ESC.OnStatsChange += OnStatsChange;
    }

    private void OnDestroy()
    {
        ESC.OnStatsChange -= OnStatsChange;
    }

    private void OnStatsChange()
    {
    }

    public void AddAliveEnemy(CH_Stats stats)
    {
        aliveEnemiesStats.Add(stats);
    }

    public void RemmoveAliveEnemy(CH_Stats stats)
    {
        if (aliveEnemiesStats.Contains(stats))
            aliveEnemiesStats.Remove(stats);
    }

    public void ResetStatsChanges()
    {
        ESC = new();
    }

    public void ApplyNewStatsToAliveEnemies()
    {
        foreach (CH_Stats enemy in aliveEnemiesStats)
        {
            if (enemy == null) { continue; }

            enemy.GSC.CombineChanges(ESC);
        }
    }

    public void RemovePreviousStatsFromAliveEnemies()
    {
        foreach (CH_Stats enemy in aliveEnemiesStats)
        {
            if (enemy == null) { continue; }

            enemy.GSC.CombineChanges(ESC);
        }
    }
}
