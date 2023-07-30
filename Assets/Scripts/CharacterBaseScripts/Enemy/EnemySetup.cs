using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySetup : MonoBehaviour
{
    [field: SerializeField] public CH_InitialStats InitialStats { get; set; }

    private EnemyNav enemyNav;
    private CH_Stats stats;
    private GameStatistics gameStatistics;
   

    public void OnSpawnSetup(Transform player, float resetDistance, float resetDOTDistance, float resetEnemyPositionMultiplier, float navUpdateCD, Transform projParent, GameObject projPrefab)
    {
        gameStatistics = GameStatistics.instance;

        //--------------COMPONENTS--------------------
        enemyNav = GetComponent<EnemyNav>();
        stats = GetComponent<CH_Stats>();

        //--------------NAVIGATION--------------------
        enemyNav.SetUpNavigation(player, resetDistance, resetDOTDistance, resetEnemyPositionMultiplier, navUpdateCD, projParent, projPrefab);

        //--------------STATS-------------------------        

        StatsValues statsValues = InitialStats.GetInitialStats();

        stats.StatsInitialization(statsValues);

        stats.SetUpCurrentStatsToBase();

        stats.ReplanishAmmo();

        //----------------Behavior----------------------

        enemyNav.SetUpBehaviour();

        //----------------Statistics--------------------

        gameStatistics.AddAliveEnemy();

        //----------------Animations--------------------

        var anim = GetComponent<CH_Animation>();

        anim.AnimationsSetUp();
        anim.PlayWalk();

        //----------------------------------------------
    }
}
