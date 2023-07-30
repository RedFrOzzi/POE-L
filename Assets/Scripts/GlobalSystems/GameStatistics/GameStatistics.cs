using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameStatistics : MonoBehaviour
{

    public static GameStatistics instance = null;

    public event Action OnEnemyKill;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else if (instance = this)
        {
            Destroy(gameObject);
        }

        InitializeGameStatistic();
       
    }

   

    [field: Header("About Enemy")]
    public int EnemiesAlive { get; private set; }
    public int EnemiesDied { get; private set; }
    public float DamageDone { get; private set; }
    public float PhisicalDamageDone { get; private set; }
    public float MagicDamageDone { get; private set; }
    public float TrueDamageDone { get; private set; }
    public float BiggestPlayerHit { get; private set; }


    [field: Header("About Player"), Space(20)]    
    public float BiggestEnemyHit { get; private set; }
    public float DamageGot { get; private set; }
    public float PhisicalDamageGot { get; private set; }
    public float MagicDamageGot { get; private set; }
    public float TrueDamageGot { get; private set; }





    public void DamageStatistics(string tag, Damage damage)
    {
        
        float combinedDamage = damage.CombinedDamage();

        switch (tag) {

            case "Enemy":
                if (BiggestPlayerHit < combinedDamage)
                {
                    BiggestPlayerHit = combinedDamage;
                }

                DamageDone += combinedDamage;
                PhisicalDamageDone += damage.CombinedPhisicalDamage();
                MagicDamageDone += damage.CombinedMagicDamage();
                TrueDamageDone += damage.CombinedTrueDamage();
                break;
            case "Player":
                if (BiggestEnemyHit < combinedDamage)
                {
                    BiggestEnemyHit = combinedDamage;
                }

                DamageGot += combinedDamage;
                PhisicalDamageGot += damage.CombinedPhisicalDamage();
                MagicDamageGot += damage.CombinedMagicDamage();
                TrueDamageGot += damage.CombinedTrueDamage();
                break;
        }        
        
    }

    public void OnDeathStatistic(string tag)
    {
        switch (tag)
        {

            case "Enemy":
                EnemiesDied++;
                EnemiesAlive--;
                OnEnemyKill?.Invoke();
                break;
            case "Player":
                //nothing
                break;
        }
    }

    public void AddAliveEnemy()
    {
        EnemiesAlive++;
    }

    private void InitializeGameStatistic()
    {
       // Debug.Log("GameStatistic initialized");
    }

}
