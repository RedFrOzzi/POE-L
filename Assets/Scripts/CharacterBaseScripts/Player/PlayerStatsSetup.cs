using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsSetup : MonoBehaviour
{
    [SerializeField] private CH_InitialStats initialStats;

    private CH_Stats stats;
    private CH_Animation anim;

    private void Awake()
    {
        stats = GetComponent<CH_Stats>();
        anim = GetComponent<CH_Animation>();
    }

    private void Start()
    {
        stats.StatsInitialization(initialStats.GetInitialStats());
        stats.SetUpCurrentStatsToBase();

        anim.AnimationsSetUp();
        anim.PlayIdle();
    }

}
