using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Database;

public class SpeedGroundEffect : MonoBehaviour
{
    private string nameID;
    private bool isEngaged;
    private float radius;
    private float speed;
    private string description;
    private LayerMask layerMask;
    private float nextActivation;
    private float expireTime;
    private CH_Stats sourseStats;

    private const float buffDuration = 0.5f;
    private const float tickCooldown = 0.2f;

    public void SetUpEffect(CH_Stats sourseStats, float radius, float speed, float effectDuration, LayerMask layerMask, string nameID)
    {
        this.sourseStats = sourseStats;
        this.nameID = nameID;        
        this.layerMask = layerMask;
        this.radius = radius;
        this.speed = speed;        
        expireTime = Time.time + effectDuration;
        isEngaged = true;
    }

    private void Update()
    {
        if (GameFlowManager.Instance.IsGamePaused) { return; }

        if (isEngaged == false) { return; }

        if (nextActivation > Time.time) { return; }

        nextActivation = Time.time + tickCooldown;

        if (Time.time > expireTime) { Destroy(gameObject); }

        var colliders = Physics2D.OverlapCircleAll(transform.position, radius, layerMask);
        
        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent(out CH_BuffManager buffManager))
            {
                if (speed >= 0)
                {
                    BuffSpeedUp buffSpeedUp = new();

                    buffSpeedUp.SetSpeed(speed)
                        .SetName(nameID)
                        .SetDuration(buffDuration)
                        .ApplyBuff(buffManager, sourseStats);
                }
                else
                {
                    DebuffSlow debuffSlow = new();
                    debuffSlow.SetSlow(speed)
                        .SetName(nameID)
                        .SetDuration(buffDuration)
                        .ApplyBuff(buffManager, sourseStats);
                }
            }
        }
    }





}
